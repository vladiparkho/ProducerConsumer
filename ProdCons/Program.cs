using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdCons
{
    internal static class Program
    {
        private static void Main()
        {
            const int producersCount = 2;
            const int consumersCount = 2;
            const int delayTime = 1000;

            var channel = System.Threading.Channels.Channel.CreateUnbounded<List<int>>();
            var producer = new Producer(producersCount, delayTime, channel.Writer);
            var consumer = new Consumer(consumersCount, channel.Reader);

            var prodTasks = producer.StartNewProducers();
            var consTasks = consumer.StartNewConsumers();

            Task.WaitAll(prodTasks);
            Task.WaitAll(consTasks);
        }
    }
}