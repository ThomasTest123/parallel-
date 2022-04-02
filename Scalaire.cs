using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;


namespace ParallelExe
{
    class Program
    {

        private static object _lock = new object();
        private static int nbrThr = Environment.ProcessorCount;


        static double ScalaireSeq(double[] V1,double[] V2)
        {

            if (V1.Length != V2.Length)
                throw new Exception("Les vecteurs ne sont pas de la meme taille");

            double Produit = 0;

            for(int i = 0;i<V1.Length;i++)
            {
                Produit += V1[i] * V2[i];
            }
            return Produit;

        }

        static double ScalaireParallelFor(double[] V1, double[] V2)
        {
            if (V1.Length != V2.Length)
                throw new Exception("Les vecteurs ne sont pas de la meme taille");

            double Produit = 0;

            Parallel.For(0, V1.Length, (i) =>
            {
                lock(_lock)
                    Produit += V1[i] * V2[i];
            });

            return Produit;

        }




        static double ScalaireMultiThreading(double[] V1, double[] V2)
        {
            if (V1.Length != V2.Length)
                throw new Exception("Les vecteurs ne sont pas de la meme taille");

            double Produit = 0;
            Thread[] threads = new Thread[nbrThr];
            UInt32 step = (UInt32)(V1.Length / nbrThr);
            UInt32 rest = (UInt32)(V1.Length % nbrThr);
            
            for (int i = 0; i<nbrThr;i++)
            {
                int copy = i;

                threads[i] = new Thread(() =>
                {
                    double ProduitLocal = 0;
                    UInt32 min = (UInt32)(copy * step);
                    
                    UInt32 max = (min + step);
                    if (copy == nbrThr - 1)
                    {
                        max += rest;
                    }

                    for (UInt32 j = min;j<max;j++)
                    {
                         ProduitLocal += V1[j] * V2[j];
                    }

                    lock (_lock)
                        Produit += ProduitLocal;
                });

                threads[i].Start();
            }

            for (int i = 0; i < nbrThr; i++)
            {
                threads[i].Join();
            }

            return Produit;

        }



        public static double[] Fill(int Size)
        {

            Random r = new Random();
            double[] V = new double[Size];
            for(int i = 0;i<Size;i++)
            {
                V[i] = r.Next(10);
            }
            return V;

        }


        static void Main(string[] args)
        {

            double[] V1 = Fill(99900000);
            double[] V2 = Fill(99900000);

            Stopwatch watch = new Stopwatch();
            double Produit;
            watch.Start();

            Produit = ScalaireSeq(V1, V2);
            watch.Stop();

            Console.WriteLine(Produit);
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms en sequentielle");
            watch.Reset();

            

            watch.Start();

            Produit = ScalaireMultiThreading(V1, V2);
            watch.Stop();

            Console.WriteLine(Produit);
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms en MultiThreading");
            watch.Reset();



        }
    }
}
