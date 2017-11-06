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
        Queue<string> messageQueue;
        Dictionary<int, Client> users;
        int clientID = 0;
        public Server()
        {

            server = new TcpListener(IPAddress.Any, 9999);
            messageQueue = new Queue<string>();
            users = new Dictionary<int, Client>();
            server.Start();
        }
        public void Run()
        {

            Task.Run(() =>ListeningForClient());
        }
        private void AddUserToDictionary(Client client)
        {
            users.Add(clientID, client);
            client.UserId = clientID;
            clientID++;
        }

        public void ListenForMessages(TcpClient clientSocket, Client client)
        {
            
            while (true)
            {
                Task<string> message = Task.Run(() => client.Recieve());
                message.Wait();
                Task<string>[] messages = new Task<string>[] { message };
                string nextMessage = messages[0].Result;
                messageQueue.Enqueue(nextMessage);
                Respond(messageQueue.Dequeue());
                //message = null;

            }
        }
        private void ListeningForClient()
        {
            while (true)
            {
                if (server.Pending() == true)
                {
                    clientFoundAndAdding();
                }
            }
        }
        private Client clientFoundAndAdding()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            // loop dict. to notify someone has logged in.
            NetworkStream stream = clientSocket.GetStream();
            Client newClient = new Client(stream, clientSocket);
            AddUserToDictionary(newClient);
            Task.Run(() => ListenForMessages(clientSocket, client));
            return newClient;
            
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
