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
    class Server
    {
        public static Client client;
        TcpListener server;
        public Server()
        {

            server = new TcpListener(IPAddress.Any, 9999);
            server.Start();
        }
        public void Run()
        {
            Task.Run(() => ListeningForClient());
            Task.Run(() => ListenForMessages());
        }
            //var clientThread = new Thread(() => ListeningForClient());
            //ListeningForClient();
            //var serverThread = new Thread(() => ListenForMessages());
            //ListenForMessages();
        
       
        //public void SendQueuedMessage(string message, Queue<string> Q)
        //{
        //    Object locker = new Object();
        //        if (Q != null)
        //        {      
        //            Respond(Q.Dequeue());
        //        }
        //}
        public void ListenForMessages()
        {
            Queue<string> Q = new Queue<string>();
            while (true)
            {
                //string message = client.Recieve();
                if (client.Recieve() != null)
                {
                    string message = client.Recieve();
                    Q.Enqueue(message);
                    Respond(Q.Dequeue());
                    message = null;
                }
            }
        }


        private void ListeningForClient()
        {
            while (true)
            {
                if (server.Pending() == true)
                {
                    clientFound();
                }
            }
        }
        private Client clientFound()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            // loop dict. to notify someone has logged in.
            NetworkStream stream = clientSocket.GetStream();
            return client = new Client(stream, clientSocket);
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
