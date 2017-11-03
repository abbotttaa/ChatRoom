using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server   
{
    class Server : IDictionary;
    {
        public static Client client;
        TcpListener server;
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.109"), 9999);//IPAddress.Any, Will allow you to connect multiple people 
            server.Start();
        }
        public void Run()
        {
            AcceptClient();

            while (true)
            {
                string message = client.Recieve();
                Respond(message);
            }
         }
        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new Client(stream, clientSocket);
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
