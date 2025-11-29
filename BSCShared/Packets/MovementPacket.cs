using System;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace BSCShared.Packets
{
    public class MovementPacket : Packet
    {
        public ushort x;
        public ushort y;
        public ushort z;

        // Constructor for reading from payload
        public MovementPacket(byte[] _payload)
        {
            type = PacketType.MOVEMENT;

            if (_payload != null && _payload.Length >= 6)
            {
                payload = _payload;

                x = EndianBitConverter.ToUInt16BigEndian(_payload, 0);
                y = EndianBitConverter.ToUInt16BigEndian(_payload, 2);
                z = EndianBitConverter.ToUInt16BigEndian(_payload, 4);               
            }           
        }
        public MovementPacket(ushort _x, ushort _y, ushort _z)
        {
            type = PacketType.MOVEMENT;

            payload = new byte[6];
                     

            byte[] xBytes = EndianBitConverter.GetBytesBigEndian(_x);
            byte[] yBytes = EndianBitConverter.GetBytesBigEndian(_y);
            byte[] zBytes = EndianBitConverter.GetBytesBigEndian(_z);

            xBytes.CopyTo(payload, 0);
            xBytes.CopyTo(payload, 2);
            xBytes.CopyTo(payload, 4);

        }

        // Constructor for sending a packet
        public MovementPacket()
        {
            type = PacketType.MOVEMENT;
        }    

    }
}
