using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            Client client = new Client("172.16.15.249", 9999);
            client.Run_SendReceive();
            Console.ReadLine();
        }
    }
}
