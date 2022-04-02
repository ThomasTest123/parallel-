private static object _lock = new object();
private static int nbrThr = Environment.ProcessorCount;

static void Sequentielle(int k)
{



}

static void ParallelFor(int k)
{
Parallel.For(Start_index, End_index, (i) =>
    {
        //lock(_lock){  } if you want to lock something
    });

}


static void ThreadMethod(int i)
{
    UInt32 min = (UInt32)(step * i);
    UInt32 max = (UInt32)(step + min);
    if (i == nbrThr - 1)
    {
        max += rest;
    }
    for (UInt32 j = min; j <     max; j++)
    {

    }

}


static UInt32 step;
static UInt32 rest;
static void MultiThreading(int k)
{
    Thread[] threads = new Thread[nbrThr];
    step = (UInt32)(k / nbrThr);
    rest = (UInt32)(K % nbrThr);
            
    for (int i = 0; i<nbrThr;i++)
    {
        int copy = i;
        threads[i] = new Thread(() =>{ThreadMethod(copy)});
        threads[i].Start();
    }

    for (int i = 0; i < nbrThr; i++)
    {
        threads[i].Join();
    }
}


static void Main(string[] args)
{
    int K;
    Console.WriteLine("Donnez K");
    K = int.Parse(Console.ReadLine());
    Stopwatch watch = new Stopwatch();
    watch.Start();
    sequentielle(K)
    Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms en sequentielle");
    watch.Reset();
    watch.Start();
    ParallelFor(K)
    watch.Stop();
    Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms en ParallelFor");
    watch.Reset();
    watch.Start();
    MultiThread(K);
    watch.Stop();
    Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms en Multithreading");

}