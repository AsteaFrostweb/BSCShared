using System;
using System.Net.Security;
using static Grid;
namespace BSCShared.Packets
{
    public class MovementPacket : Packet
    {
        public Vector3 destination;

        // Constructor for reading from payload
        public MovementPacket(byte[] _payload)
        {
            type = PacketType.MOVEMENT;

            if (_payload != null)
            {
                payload = _payload;

                Vector3? parsedVec = Vector3.FromBytes(_payload);
                if (parsedVec == null) 
                {
                    Debugging.LogError("Movement Packet", "Error parsing destination from the raw bytes");
                    return;
                }

                destination = parsedVec.Value;
            }           
        }

        public MovementPacket(Vector3 _destination)
        {
            type = PacketType.MOVEMENT;
            destination = _destination;
            payload = destination.ToBytes();

        }

        // Constructor for sending a packet
        public MovementPacket()
        {
            type = PacketType.MOVEMENT;
        }    

    }
}
