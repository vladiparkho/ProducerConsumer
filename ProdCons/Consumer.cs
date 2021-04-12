using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ProdCons
{
    public class Consumer
    {
        private int ConsumersCount { get; set; }
        private ChannelReader<List<int>> Cr { get; set; }

        public Consumer(int consumersCount, ChannelReader<List<int>> cr)
        {
            ConsumersCount = consumersCount;
            Cr = cr;
        }

        public Task[] StartNewConsumers()
        {
            var consumer = new Task[ConsumersCount];
            for (var i = 0; i < consumer.Length; i++)
            {
                consumer[i] = Task.Factory.StartNew(async () =>
                {
                    do
                    {
                        if (Cr.TryRead(out var data))
                        {
                            Console.WriteLine("Reading from channel. Median of an array is " + GetMedian(data));
                        }
                    } while (await Cr.WaitToReadAsync());
                });
            }

            return consumer;
        }

        private static int GetMedian(List<int> arr)
        {
            var length = arr.Count;
            arr.Sort();
            return (length % 2 != 0) ? (arr[(length - 1) / 2]) : ((arr[length / 2] + arr[length / 2 - 1]) / 2);
        }
    }
}