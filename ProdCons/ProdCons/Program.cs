using System;
using System.Collections.Generic;
using System.Linq;


namespace ProdCons
{
    class Program
    {
        private static int[] getRandomArr(int length) {
            Random rnd = new Random();
            int[] arr = new int[length];
            for (int i = 0; i < length; i++) {
                arr[i] = rnd.Next();
            }
            return arr;
        }

        private static List<int[]> getRandList(int length,int arrLength)
        {
            List<int[]> arrays = new List<int[]>(length);
            for (int i = 0; i < length; i++)
            {
                arrays.Add(getRandomArr(arrLength));
            }

            return arrays;
        }
        
        static void Main(string[] args)
        {
            List<int[]> arrays = getRandList(4, 5);
            
            foreach (var it in arrays)
            {
                Console.WriteLine("Arr: " + string.Join(", ", it.Select(_=>_.ToString()).ToArray()));
            }
            Console.WriteLine("------------------------");

            var pc = new ProducerConsumer(arrays, 2, 2, 1000);
            pc.StartChannel();
        }
    }
}