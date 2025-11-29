using System;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace BSCShared.Packets
{
    public class PingPongPacket : Packet
    {
        public DateTime sendTime;

        // Constructor for reading from payload
        public PingPongPacket(byte[] _payload)
        {           

            type = PacketType.PINGPONG;

            if (_payload != null && _payload.Length >= 8) // only 8 bytes for ticks
            {
                payload = _payload;

                // Read sendTime (ticks) as big-endian Int64
                long ticks = EndianBitConverter.ToInt64BigEndian(_payload, 0);
                sendTime = new DateTime(ticks, DateTimeKind.Utc);
            }
            else
            {
                // Fallback for invalid payload
                sendTime = DateTime.MinValue;
                payload = Array.Empty<byte>();
            }
        }

        // Constructor for sending a packet
        public PingPongPacket()
        {
            type = PacketType.PINGPONG;
        }

        // Write to stream
        public override async Task WriteToStreamAsync(SslStream ns)
        {
            sendTime = DateTime.UtcNow;
            AssignPayload();

            await base.WriteToStreamAsync(ns);
        }

        // Convert sendTime to payload bytes (big-endian)
        private void AssignPayload()
        {
            payload = EndianBitConverter.GetBytesBigEndian(sendTime.Ticks);
        }

        // Background pulse for sending ping packets
        public static async Task Pulse(SslStream stream, float interval, CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {                
                await new PingPongPacket().WriteToStreamAsync(stream); // Send PingPongPacket

                await Task.Delay(TimeSpan.FromSeconds(interval), cts.Token); // Wait interval asynchronously
            }
        }
    }
}
