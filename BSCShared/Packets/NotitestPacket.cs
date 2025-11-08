using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;

namespace BSCShared.Packets
{
    public class NotitestPacket : Packet
    {
        public string entityUID { get; set; }


        public NotitestPacket(byte[] _payload)
        {
            type = PacketType.NOTITEST;
            if (_payload != null && _payload.Length > 0)
            {
                int bytesRead;
                entityUID = EndianBitConverter.ToStringBigEndian(_payload, 0, out bytesRead);
                payload = new byte[bytesRead];
                Array.Copy(_payload, 0, payload, 0, bytesRead);
            }
            else
            {
                throw new ArgumentException("Unable to convert bytes in entityUID");
            }
        }


        public NotitestPacket(string _entityUID)
        {
            type = PacketType.NOTITEST;
            entityUID = _entityUID;

            payload = EndianBitConverter.GetStringBytesBigEndian(entityUID);
        }


    }
}
