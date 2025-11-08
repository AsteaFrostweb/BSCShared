using BSCShared.Packets;
using BSCShared;

namespace BSCShared.Packets
{
    public class MessagePacket : Packet
    {
        public string message { get; set; }


        public MessagePacket(byte[] _payload)
        {
            type = PacketType.MESSAGE;
            if (_payload != null && _payload.Length > 0)
            {
                int bytesRead;
                message = EndianBitConverter.ToStringBigEndian(_payload, 0, out bytesRead);
                payload = new byte[bytesRead];
                Array.Copy(_payload, 0, payload, 0, bytesRead);
            }
            else
            {
                message = string.Empty;
                payload = Array.Empty<byte>();
            }
        }


        public MessagePacket(string _message)
        {
            type = PacketType.MESSAGE;
            message = _message ?? string.Empty;

            payload = EndianBitConverter.GetStringBytesBigEndian(message);
        }


    }
}