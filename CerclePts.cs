using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cercle_Parallel
{
    class Program
    {

        static double Range = 50000000;

        static void CerclePts(int start,int End,double R)
        {
            int PtNbr = 0;
            double step = (End - start) / Range;


            double Coeff = (Math.PI / 180);
            for (double i = start;i<= End;i+= step)
            {
                double X = R * Math.Cos(Coeff * i);
                double Y = R * Math.Sin(Coeff * i);
                // Console.WriteLine($"{i} degree {X},{Y}");

                PtNbr += 1;
            }
            Console.WriteLine($"On a caluler {PtNbr} Points");
        }


        private static object _lock = new object();

        private static int nbrThr = Environment.ProcessorCount;
        static double step;
        static double rest;


        static void CercleThread(int i, int start, int End, double R)
        {
            double min = start + (step * i);
            double max = (step + min);

            double Coeff = (Math.PI / 180);

            for (double j = min; j < max; j++)
            {
                double X = R * Math.Cos(Coeff * j);
                double Y = R * Math.Sin(Coeff * j);
            }
        }

        static void CercleParallelFor(int start, int End, double R)
        {
            double step = (End - start) / Range;
            double Coeff = (Math.PI / 180);

            Parallel.For(0, (int)Range + 1, (i) => {
                double j = start + i * step;
                double X = R * Math.Cos(Coeff * j);
                double Y = R * Math.Sin(Coeff * j);
                //Console.WriteLine($"{j} degree {X},{Y}");
            });
        }








        static void CercleParallel(int start, int End, double R)
        {
            step = (Range / nbrThr);
            rest = (Range % nbrThr);

            Thread[] threads = new Thread[nbrThr];

            for (int i = 0; i < nbrThr; i++)
            {
                int copy = i;

                if (i == nbrThr - 1){
                    End += (int)rest;
                }
                threads[i] = new Thread(() => {
                    CercleThread(copy,start,End,R);
                });

                threads[i].Start();
            }
            for (int j = 0; j < nbrThr; j++)
            {
                threads[j].Join();
            }

        }


        static void Main(string[] args)
        {
            Console.WriteLine("Entrer le rayon:");
            double R = double.Parse(Console.ReadLine());

            Console.WriteLine("Entrer l'intervale(en degree format start-end):");
            string[] interval = Console.ReadLine().Split('-');
            int Start = int.Parse(interval[0]);
            int End = int.Parse(interval[1]);

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            CerclePts(Start, End, R);
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms Seq");
            watch.Reset();

            watch.Start();
            CercleParallel(Start, End, R);
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms MultiThread");
            watch.Reset();
            watch.Start();
            CercleParallelFor(Start, End, R);
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms Parallel");



        }
    }
}
