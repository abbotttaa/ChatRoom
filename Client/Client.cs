using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;

        public void Run_SendReceive()
        {
<<<<<<< HEAD
            Parallel.Invoke( () =>
                             {
                                 Send();
                             },
                             () =>
                             {
                                 Recieve();
                             }
                            );
        }
=======
            Task.Run(() => Send());
            Task receiveFinish = Task.Run(() => Recieve());
            receiveFinish.Wait();
            Run_SendReceive();
        }

>>>>>>> 57efff9e54cef09688a82c72f0bb8db1217aaa60
        public Client(string IP, int port)
        {
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
        }
        public void Send()
        {
            while (true)
            {
                string messageString = UI.GetInput();
                byte[] message = Encoding.ASCII.GetBytes(messageString);
                stream.Write(message, 0, message.Count());
            }
        }
        public void Recieve()
        {
            while (true)
            {
                byte[] recievedMessage = new byte[256];
                stream.Read(recievedMessage, 0, recievedMessage.Length);
                UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
            }
        }
    }
}
