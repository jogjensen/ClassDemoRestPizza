using System;

namespace PizzaConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            RestWorker worker = new RestWorker();
            worker.Start();

            Console.ReadLine();
        }
    }
}
