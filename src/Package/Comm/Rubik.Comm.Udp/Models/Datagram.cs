using System;
using System.Net;

namespace Rubik.Comm.Udp.Models
{
    public class Datagram
    {
        public string Ip { get; }
        public int Port { get; }
        public Memory<byte> Data { get; }

        internal Datagram()
        {

        }

        internal Datagram(IPEndPoint ipEndPoint, Memory<byte> data)
        {
            Ip = $"{ipEndPoint.Address}";
            Port = ipEndPoint.Port;
            Data = data;
        }
    }
}
