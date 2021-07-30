using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using TitleFetcher.Slave.QueueManager;

namespace TitleFetcher.Slave
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            QueueManagerIns.init = true;
            Console.ReadLine();
        }
    }
}
