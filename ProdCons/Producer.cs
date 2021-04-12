using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ProdCons
{
    public class Producer
    {
        private int ProducersCount { get; set; }
        private int DelayTime { get; set; }
        private ChannelWriter<List<int>> Cw { get; set; }

        public Producer(int producersCount, int delayTime, ChannelWriter<List<int>> cw)
        {
            ProducersCount = producersCount;
            DelayTime = delayTime;
            Cw = cw;
        }

        public Task[] StartNewProducers()
        {
            var producer = new Task[ProducersCount];
            for (var i = 0; i < ProducersCount; i++)
            {
                producer[i] = Task.Factory.StartNew(async () =>
                {
                    do
                    {
                        if (!Cw.TryWrite(GetRndList(4))) continue;
                        Console.WriteLine("Writing into channel. " + Task.CurrentId + " task ");
                        Thread.Sleep(DelayTime);
                    } while (await Cw.WaitToWriteAsync());

                    Cw.Complete();
                });
            }

            return producer;
        }

        private static List<int> GetRndList(int length)
        {
            var rnd = new Random();
            var arr = new List<int>();

            for (var i = 0; i < length; i++)
            {
                arr.Add(rnd.Next());
            }

            return arr;
        }
    }
}