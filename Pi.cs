using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PI
{
    class Program
    {
        static double PiCalc(int k)
        {
            double Pi = 0;
            for(int i = 0;i<k;i++)
            {
                Pi += Math.Pow(-1, i) / (2 * i + 1);
            }
            return Pi * 4;
        }
        private static object _lock = new object();
        private static int nbrThr = Environment.ProcessorCount;





        static  void PiThread(int i)
        {
            double LocalPi = 0;

            UInt32 min = (UInt32)(step * i);
            UInt32 max = (UInt32)(step + min);

            if (i == nbrThr - 1)
            {
                max += rest;
            }

            for (UInt32 j = min; j <     max; j++)
            {
                LocalPi += Math.Pow(-1, j) / (2 * j + 1);
            }

            lock (_lock)
                Pi += LocalPi;

        }

        static UInt32 step;
        static UInt32 rest;
        static double Pi = 0;
        static double PiCalcParallel(int k)
        {

            Thread[] threads = new Thread[nbrThr];
            step =  (UInt32)(k / nbrThr);
            rest = (UInt32)(k % nbrThr);

            for(int i = 0;i<nbrThr;i++)
            {
                int copy = i;
                threads[i] = new Thread(()=> {
                    PiThread(copy);
                });

                threads[i].Start();
                

            }
            for (int j = 0; j < nbrThr; j++)
            {
                threads[j].Join();
            }
            return Pi*4;
        }



        static double ParallelForPi(int k)
        {
            double PiVal = 0;
            Parallel.For(0, k, (j) =>
              {
                  lock (_lock)
                      PiVal += Math.Pow(-1, j) / (2 * j + 1);
              });
            return PiVal*4;
        }


        static void Main(string[] args)
        {
            int K;
            Console.WriteLine("Donnez K");
            K = int.Parse(Console.ReadLine());
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Console.WriteLine("Pi en Seq {0:F15}", PiCalc(K));
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms en sequentielle");
            watch.Reset();
            watch.Start();
            Console.WriteLine("Pi en MultiThread {0:F15}",PiCalcParallel(K));
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms en MultiThread");

            watch.Reset();
            watch.Start();
            Console.WriteLine("Pi en Parrallel {0:F15}", ParallelForPi(K));
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms en Parrallel");

        }
    }
}
