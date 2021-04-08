using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ProdCons
{
   public class ProducerConsumer
    {
        private int producersCount{ get; set; }
        private int consumersCount { get; set; }
        private int delayTime { get; set;}
        private Channel<int[]> channel { get; set; }
        private List<int[]> arrays { get; set; }

        public ProducerConsumer(List<int[]> arr,int prodCount,int consCount,int delay)
        {
            producersCount = prodCount;
            consumersCount = consCount;
            delayTime = delay;
            arrays = arr;
            channel = Channel.CreateBounded<int[]>(arrays.Count);
        }
        
        public static int getMedian(int[] arr)
        {
            int median;
            int length = arr.Length;
            Array.Sort(arr);
            if (length % 2 == 0)
                median = (arr[length / 2] + arr[length / 2 - 1]) / 2;
            else
                median = arr[(length - 1) / 2];

            return median;
        }
        
        public void StartChannel()
        {
            Task producer = Task.Factory.StartNew(() => {
                Parallel.ForEach(arrays, new ParallelOptions {
                    MaxDegreeOfParallelism = producersCount
                }, item =>
                {
                    //thr.Delay(delayTime);
                   // Thread.Sleep(delayTime);
                    if (channel.Writer.TryWrite(item))
                    {
                        Console.WriteLine("Writing into channel. " + Task.CurrentId + " task ");
                        Thread.Sleep(delayTime);
                    }
                });
            channel.Writer.Complete();
            });  
            
            Task[] consumer = new Task[consumersCount];
            for (int i = 0; i < consumer.Length; i++)
            {
                consumer[i] = Task.Factory.StartNew(async () =>
                {
                    while (await channel.Reader.WaitToReadAsync())
                    {
                        if (channel.Reader.TryRead(out var data))
                        {
                            Console.WriteLine("Reading from channel. Median of an array is " + getMedian(data));
                        }
                    }
                });
            }

            producer.Wait();
            Task.WaitAll(consumer);
        }
    }
}