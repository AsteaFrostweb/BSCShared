using System;
using static Grid;

public class RegionCoord : IEquatable<RegionCoord>
{
    public const ushort REGION_SIZE = 64;

    public ushort x;
    public ushort z;

    public RegionCoord(ushort x, ushort z)
    {
        this.x = x;
        this.z = z;
    }

    // -------------------------
    // Conversions
    // -------------------------

    public static UShort3 RegionToGridPosition(RegionCoord regionPos)
        => new UShort3(
            (ushort)(regionPos.x * REGION_SIZE),
            0,
            (ushort)(regionPos.z * REGION_SIZE)
        );

    public static RegionCoord GridToRegionPosition(UShort3 gridPos)
        => new RegionCoord(
            (ushort)(gridPos.x / REGION_SIZE),
            (ushort)(gridPos.z / REGION_SIZE)
        );

    // -------------------------
    // From world integer coords (tile coords)
    // -------------------------
    public static RegionCoord FromWorld(int wx, int wz)
        => new RegionCoord(
            (ushort)(wx / REGION_SIZE),
            (ushort)(wz / REGION_SIZE)
        );

    // -------------------------
    // Dictionary Safety
    // -------------------------

    public bool Equals(RegionCoord other)
    {
        if (other == null) return false;
        return x == other.x && z == other.z;
    }

    public override bool Equals(object obj)
        => obj is RegionCoord rc && Equals(rc);

    public override int GetHashCode()
        => (x << 16) ^ z;

    public static bool operator ==(RegionCoord a, RegionCoord b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(RegionCoord a, RegionCoord b)
        => !(a == b);


    // -------------------------
    // World-space convenience
    // -------------------------
    public int WorldX => x * REGION_SIZE;
    public int WorldZ => z * REGION_SIZE;
}
