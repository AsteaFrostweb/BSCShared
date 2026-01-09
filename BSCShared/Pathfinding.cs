using BSCShared;
using System;
using System.Collections.Generic;
using System.Linq;

using static Grid;

public static class Pathfinder
{
    public struct Path
    {
        public UShort2[] path;
        public int Steps => path.Length;

        public byte[] ToBytes() 
        {
            // Number of steps * 4 bytes per UShort2 postition
            byte[] bytes = new byte[Steps * 4];
            int writeIndex = 0;
            foreach (UShort2 step in path) 
            {
                //Generate bytes
                byte[] xBytes = EndianBitConverter.GetBytesBigEndian(step.x);
                byte[] yBytes = EndianBitConverter.GetBytesBigEndian(step.y);

                //Copy to payload
                xBytes.CopyTo(bytes, writeIndex);
                yBytes.CopyTo(bytes, writeIndex + 2);

                //Increment write index
                writeIndex += 4;
            }

            return bytes;
        }

        public static Path FromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length % 4 != 0)
                throw new ArgumentException("Invalid byte array length for Path.");

            int steps = bytes.Length / 4;
            UShort2[] pathArray = new UShort2[steps];

            for (int i = 0; i < steps; i++)
            {
                int offset = i * 4;
                ushort x = EndianBitConverter.ToUInt16BigEndian(bytes, offset);
                ushort y = EndianBitConverter.ToUInt16BigEndian(bytes, offset + 2);
                pathArray[i] = new UShort2(x, y);
            }

            return new Path { path = pathArray };
        }

    }

    public class PathNode
    {
        public UShort2 position { get; set; }
        public PathNode parent { get; set; }
        public int gValue { get; set; }
        public int hValue { get; set; }
        public int fValue => gValue + hValue;

        public PathNode(UShort2 pos, PathNode parent, UShort2? target)
        {
            position = pos;
            this.parent = parent;

            gValue = parent == null ? 0 : parent.gValue + ((pos.x == parent.position.x || pos.y == parent.position.y) ? 10 : 14);
            hValue = target == null ? 0 : (int)UShort2.Distance(pos, target.Value);
        }
    }

    /// <summary>
    /// Find a path from startTile to goalTile using A* on Map.tiles
    /// </summary>
    public static bool FindPath(UShort2 startPos, UShort2 destinationPos, out Path calculatedPath)
    {
        calculatedPath = new Path();

        if (startPos == null || destinationPos == null)
            return false;

        var openList = new List<PathNode>();
        var closedList = new List<PathNode>();

        var startNode = new PathNode(startPos, null, destinationPos);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Get node with lowest F value
            PathNode current = openList.OrderBy(n => n.fValue).First();

            if (current.position == destinationPos)
            {
                calculatedPath = BuildPath(current);
                return true;
            }

            openList.Remove(current);
            closedList.Add(current);

            foreach (var neighbor in GetAdjacentNodes(current, destinationPos))
            {
                if (closedList.Any(n => n.position == neighbor.position))
                    continue;

                var existing = openList.FirstOrDefault(n => n.position == neighbor.position);
                if (existing != null)
                {
                    if (existing.gValue > neighbor.gValue)
                    {
                        existing.gValue = neighbor.gValue;
                        existing.parent = current;
                    }
                }
                else
                {
                    openList.Add(neighbor);
                }
            }
        }

        return false;
    }

    private static Path BuildPath(PathNode endNode)
    {
        var path = new List<UShort2>();
        var current = endNode;
        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }
        path.Reverse();
        return new Path { path = path.ToArray() };
    }

    private static IEnumerable<PathNode> GetAdjacentNodes(PathNode parent, UShort2 target)
    {
        int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int[] dz = { -1, -1, -1, 0, 0, 1, 1, 1 };

        for (int i = 0; i < 8; i++)
        {
            int nx = parent.position.x + dx[i];
            int nz = parent.position.y + dz[i];

            Tile t;
            if (!Map.TryGetTile(nx, nz, out t))
                throw new Exception("Unable to get adjacent path nodes as map tiles aren't laoded");
            
            if (t != null && t.isWalkable)
            {
                yield return new PathNode(t.layerPosition, parent, target);
            }
        }
    }

    /// <summary>
    /// Optional helper to verify that the path is fully walkable
    /// </summary>
    public static bool VerifyPath(Path path)
    {
        foreach (var node in path.path)
        {
            Tile t;
            if(!Map.TryGetTile(node.x, node.y, out t))
                return false;

            if (t == null || !t.isWalkable)
                return false;
        }
        return true;
    }
}
