using System.Net.Sockets;

namespace BSCShared
{
    public enum PacketType : byte
    {
        PING,
        PONG
    }
    public class Packet
    {
        public PacketType type { get; set; }
        public ushort size { get; set; } //Size of the PAYLOAD. Full packet size woud be "size + 1" Bytes 
        public byte[] payload { get; set; } 

        public Packet() 
        {
            type = 0;
            payload = new byte[0];
        }
        public Packet(PacketType _type, byte[] _payload) 
        {
            type = _type;
            payload = _payload;
            size = Convert.ToUInt16(_payload.Length);
        }
      

        /// <summary>
        /// Reads and returns a packet from the network stream
        /// </summary>
        /// <param name="ns">The netowork stream to read from</param>
        /// <returns>A packet from the network stream</returns>
        /// <exception cref="InvalidCastException">
        /// Thrown when first byte isn't defiend by PacketType enum
        /// </exception>>
        public static Packet ReadFromStream(NetworkStream ns)
        {
            
            //Get first three bytes
            byte[] typeBytes = new byte[1];
            ns.Read(typeBytes, 0, 1);

            //Parse first byte into a PacketType
            PacketType _type = (PacketType)typeBytes[0];
            if (!Enum.IsDefined(typeof(PacketType), _type))              
                throw new InvalidCastException("Unable to convert first byte of packet into defined PacketType");

            //Get second and third bytes
            byte[] sizeBytes = new byte[2];
            ns.Read(sizeBytes, 0, 2);
            ushort _size = Convert.ToUInt16(sizeBytes);
            
            byte[] _payload = new byte[_size];
            ns.Read(_payload, 0, _size);

            return new Packet(_type, _payload);
        }
    }
}
