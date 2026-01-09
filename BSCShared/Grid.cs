using BSCShared;
using System;




public static class Grid
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        // Constructor
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float Magnitude => (float)Math.Sqrt(x * x + y * y + z * z);

        public float SqrMagnitude => x * x + y * y + z * z;

        public Vector3 Normalized
        {
            get
            {
                float mag = Magnitude;
                return mag > 1e-5f ? this / mag : new Vector3(0, 0, 0);
            }
        }

        // Operators
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator -(Vector3 a) => new Vector3(-a.x, -a.y, -a.z);
        public static Vector3 operator *(Vector3 a, float d) => new Vector3(a.x * d, a.y * d, a.z * d);
        public static Vector3 operator *(float d, Vector3 a) => a * d;
        public static Vector3 operator /(Vector3 a, float d) => new Vector3(a.x / d, a.y / d, a.z / d);
        public static bool operator ==(Vector3 a, Vector3 b) => Math.Abs(a.x - b.x) < 1e-5f && Math.Abs(a.y - b.y) < 1e-5f && Math.Abs(a.z - b.z) < 1e-5f;
        public static bool operator !=(Vector3 a, Vector3 b) => !(a == b);

        // Dot product
        public static float Dot(Vector3 a, Vector3 b) => a.x * b.x + a.y * b.y + a.z * b.z;

        // Cross product
        public static Vector3 Cross(Vector3 a, Vector3 b) =>
            new Vector3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
            );

        public static Vector3? FromBytes(byte[] payload) 
        {
            if (payload.Length != 12)
                return null;

            float x = 0f, y = 0f, z = 0f;
            byte[] xBytes = new byte[4], yBytes = new byte[4], zBytes = new byte[4];

            Array.Copy(payload, 0, xBytes, 0, 4);
            Array.Copy(payload, 4, yBytes, 0, 4);
            Array.Copy(payload, 8, zBytes, 0, 4);

            x = EndianBitConverter.ToSingleBigEndian(xBytes);
            y = EndianBitConverter.ToSingleBigEndian(yBytes);
            z = EndianBitConverter.ToSingleBigEndian(zBytes);

            return new Vector3(x, y, z);
        }
        public byte[] ToBytes() 
        {
            byte[] payload = new byte[12];

            byte[] xBytes = EndianBitConverter.GetBytesBigEndian(x);
            byte[] yBytes = EndianBitConverter.GetBytesBigEndian(y);
            byte[] zBytes = EndianBitConverter.GetBytesBigEndian(z);

            Array.Copy(xBytes, 0, payload, 0, 4);
            Array.Copy(yBytes, 0, payload, 4, 4);
            Array.Copy(zBytes, 0, payload, 8, 4);

            return payload;
        }

        public static float Distance(Vector3 a, Vector3 b) => (a - b).Magnitude;

        public override string ToString() => $"({x}, {y}, {z})";

        public static Vector3 Zero => new Vector3(0f, 0f, 0f);

        public static Vector3 One => new Vector3(1f, 1f, 1f);

        public static Vector3 Up => new Vector3(0f, 1f, 0f);

        public static Vector3 Down => new Vector3(0f, -1f, 0f);

        public static Vector3 Left => new Vector3(-1f, 0f, 0f);

        public static Vector3 Right => new Vector3(1f, 0f, 0f);

        public static Vector3 Forward => new Vector3(0f, 0f, 1f);

        public static Vector3 Back => new Vector3(0f, 0f, -1f);
    }


    public enum Layer : ushort
    {
        //Surface level
        SURFACE = 0,

        //Notable Floors
        FLOOR_1 = 1

    };
    public const float GRID_SPACING = 1.0f;

    [Serializable]
    public struct UShort4
    {
        public ushort x, y, z, w;

        public UShort4(ushort _x, ushort _y, ushort _z, ushort _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }
        public static bool operator ==(UShort4 a, UShort4 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
        }

        public static bool operator !=(UShort4 a, UShort4 b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z}, {w})";
        }
    }

    // -----------------------------------------------------
    // Short3 Struct
    // -----------------------------------------------------
    [Serializable]
    public struct UShort3
    {
        public ushort x, y, z;

        public UShort3(ushort _x, ushort _y, ushort _z)
        {
            x = _x; y = _y; z = _z;
        }

        public static bool operator ==(UShort3 a, UShort3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(UShort3 a, UShort3 b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

    }
    [Serializable]
    public struct UShort2
    {
        public ushort x, y;

        public UShort2(ushort _x, ushort _y)
        {
            x = _x;
            y = _y;
        }

        public static double Distance(UShort2 a, UShort2 b) => Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));

        public static bool operator ==(UShort2 a, UShort2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(UShort2 a, UShort2 b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }

    // -----------------------------------------------------
    // Conversion Functions
    // -----------------------------------------------------

    /// <summary>
    /// Converts world position to grid coordinates as Short3.
    /// </summary>
    public static UShort3 WorldToGridSpace(Vector3 worldSpace)
    {
        Vector3 gs = worldSpace / GRID_SPACING;

        return new UShort3(
            (ushort)Math.Floor(gs.x + 0.005f),
            (ushort)Math.Floor(gs.y + 0.005f),
            (ushort)Math.Floor(gs.z + 0.005f)
        );
    }

    /// <summary>
    /// Converts grid coordinates (Short3) back to world position.
    /// </summary>
    /// 
    public static Vector3 GridToWorldSpace(UShort3 gridSpace)
    {
        return new Vector3(
            gridSpace.x * GRID_SPACING,
            gridSpace.y * GRID_SPACING,
            gridSpace.z * GRID_SPACING
        );
    }
    public static Vector3 GridToWorldSpace(UShort4 gridSpace)
    {
        return new Vector3(
            gridSpace.x * GRID_SPACING,
            gridSpace.y * GRID_SPACING,
            gridSpace.z * GRID_SPACING
        );
    }

    // -----------------------------------------------------
    // Tile Boundaries
    // -----------------------------------------------------

    /// <summary>
    /// Returns the bottom-left grid coordinate in Short3.
    /// </summary>
    public static UShort3 GetBottomLeft(Vector3 gridSpace)
    {
        return new UShort3(
            (ushort)Math.Floor(gridSpace.x + 0.005f),
            (ushort)Math.Floor(gridSpace.y + 0.005f),
            (ushort)Math.Floor(gridSpace.z + 0.005f)
        );
    }

    /// <summary>
    /// Returns the top-right grid coordinate in Short3.
    /// </summary>
    public static UShort3 GetTopRight(Vector3 gridSpace)
    {
        return new UShort3(
            (ushort)Math.Ceiling(gridSpace.y),
            (ushort)Math.Ceiling(gridSpace.x),
            (ushort)Math.Ceiling(gridSpace.z)
        );
    }

    /// <summary>
    /// Returns the center point of a grid tile in world space.
    /// </summary>
    public static Vector3 GetCenter(UShort3 gridSpace)
    {
        float halfTileSize = GRID_SPACING / 2f;

        return new Vector3(
            gridSpace.x * GRID_SPACING + halfTileSize,
            gridSpace.y * GRID_SPACING + halfTileSize,
            gridSpace.z * GRID_SPACING + halfTileSize
        );
    }


}
