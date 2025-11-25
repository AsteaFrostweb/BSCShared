using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace BSCShared.Packets
{
    public enum PacketType : byte
    {
        NULL,
        MESSAGE,
        PINGPONG,
        NOTITEST,
    }
    public abstract class Packet
    {
        public PacketType type { get; set; }
        public ushort size { get; set; } //Size of the PAYLOAD. Full packet size woud be "size + 1" Bytes 
        public byte[] payload { get; set; }




        public static async Task<Packet> ReadFromStreamAsync(SslStream ns)
        {
            if (ns == null) throw new ArgumentNullException(nameof(ns));

            // Read type byte
            byte[] typeBytes = new byte[1];
            int read = await ns.ReadAsync(typeBytes, 0, 1);
            if (read == 0) return null;

            PacketType packetType = (PacketType)typeBytes[0];
            if (!Enum.IsDefined(typeof(PacketType), packetType))
                throw new InvalidCastException($"Invalid PacketType: {packetType}");
            if (packetType == PacketType.NULL) return null;

            // Read size (2 bytes) using EndianBitConverter
            byte[] sizeBytes = new byte[2];
            read = await ns.ReadAsync(sizeBytes, 0, 2);
            if (read < 2) return null;

            ushort payloadSize = EndianBitConverter.ToUInt16BigEndian(sizeBytes, 0);

            // Read payload
            byte[] payload = new byte[payloadSize];
            int totalRead = 0;
            while (totalRead < payloadSize)
            {
                int bytesRead = await ns.ReadAsync(payload, totalRead, payloadSize - totalRead);
                if (bytesRead == 0)
                    throw new Exception("Stream closed before full payload was read");
                totalRead += bytesRead;
            }

            return PacketFactory.CreatePacket(packetType, payload);
        }




        /// <summary>
        /// Writes this packet to the network stream asynchronously
        /// </summary>
        /// <param name="ns">The SslStream to write to</param>
        public virtual async Task WriteToStreamAsync(SslStream ns)
        {
            if (ns == null) throw new ArgumentNullException(nameof(ns));

            byte[] _payload = payload ?? Array.Empty<byte>();
            ushort _size = (ushort)_payload.Length;

            // Type byte
            byte[] typeBytes = new byte[] { (byte)type };

            // Size bytes (big-endian using EndianBitConverter)
            byte[] sizeBytes = EndianBitConverter.GetBytesBigEndian(_size);

            // Allocate final buffer [type:1][size:2][payload:N]
            byte[] packet = new byte[1 + 2 + _size];
            typeBytes.CopyTo(packet, 0);
            sizeBytes.CopyTo(packet, 1);
            _payload.CopyTo(packet, 3);

            // Send asynchronously
            await ns.WriteAsync(packet, 0, packet.Length);
            await ns.FlushAsync();
        }



    }

    public static class PacketFactory
    {
        private static readonly Dictionary<PacketType, Func<byte[], Packet>> PacketRegistry =
            new Dictionary<PacketType, Func<byte[], Packet>>
            {
                { PacketType.MESSAGE, payload   => new MessagePacket(payload)  },
                { PacketType.PINGPONG, payload  => new PingPongPacket(payload) },
                { PacketType.NOTITEST, payload  => new NotitestPacket(payload) }
            };

        // Create a packet based on PacketType
        public static Packet CreatePacket(PacketType type, byte[] payload)
        {
            if (PacketRegistry.ContainsKey(type))
            {
                return PacketRegistry[type](payload); // Create the specific packet class
            }

            throw new InvalidOperationException($"Unknown PacketType: {type}");
        }
    }
}
