using System;
using System.ServiceModel;

namespace WordCounterHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(WordCounter.WordCounter)))
            {
                host.Open();
                Console.WriteLine("Host open!");
                Console.ReadKey();
            }
        }
    }
}
