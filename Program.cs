using System;
using Amazon.SQS.Model;
using awsDemos.Queues;

namespace awsDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Executing Queue");
            DemoQueue queue = new DemoQueue();
            SendMessageResponse response = queue.publish("hello").GetAwaiter().GetResult();

            Console.WriteLine($"Http Status Code : {response.HttpStatusCode}");
        }
    }
}
