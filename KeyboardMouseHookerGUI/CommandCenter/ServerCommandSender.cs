using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommandCenter
{
    class ServerCommandSender
    {
        private const int PORT = 9990;

        UdpClient client;
        IPEndPoint ip;
        byte[] bytes;

        public void Send(string message)
        {
            this.client = new UdpClient();
            this.ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT);
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            bytes = Encoding.UTF8.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
            

          
        }
    }
}
