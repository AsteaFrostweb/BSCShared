using System.Collections.Generic;
using static Grid;

public static class Map
{
    // Assigned by client/server bootstrap
    public static IRegionLoader regionLoader;

    private static readonly Dictionary<RegionCoord, Region> loadedRegions =
        new Dictionary<RegionCoord, Region>();

    public static IReadOnlyDictionary<RegionCoord, Region> LoadedRegions => loadedRegions;

    private const int VERT_SIZE = RegionCoord.REGION_SIZE + 3;

    // =========================================================
    // INTERNAL REGION REGISTRATION (loaders only)
    // =========================================================
    internal static void AddRegion(Region region)
    {
        if (region == null)
            return;

        loadedRegions[region.coord] = region;
    }

    internal static bool RemoveRegion(RegionCoord coord)
    {
        return loadedRegions.Remove(coord);
    }

    internal static bool TryGetRegion(RegionCoord coord, out Region region)
    {
        return loadedRegions.TryGetValue(coord, out region);
    }

    // Compatibility helpers
    public static void RegisterRegion(Region region) => AddRegion(region);
    public static bool UnregisterRegion(RegionCoord coord) => RemoveRegion(coord);

    // =========================================================
    // INTERNAL HELPERS
    // =========================================================
    private static RegionCoord RegionOfTile(int wx, int wz)
        => RegionCoord.FromWorld(wx, wz);

    /// <summary>
    /// Enumerates all owning (region, localX, localZ) slots for a WORLD vertex.
    /// A vertex may belong to up to 4 regions (halo-aware).
    /// NO loading occurs here.
    /// </summary>
    private static IEnumerable<(Region region, int lx, int lz)>
        EnumerateOwningVertexSlots(int wx, int wz)
    {
        for (int ox = -1; ox <= 0; ox++)
            for (int oz = -1; oz <= 0; oz++)
            {
                RegionCoord rc = RegionCoord.FromWorld(wx + ox, wz + oz);

                if (!loadedRegions.TryGetValue(rc, out Region region) || region == null)
                    continue;

                int lx = wx - region.coord.WorldX;
                int lz = wz - region.coord.WorldZ;

                if (lx < 0 || lz < 0 || lx >= VERT_SIZE || lz >= VERT_SIZE)
                    continue;

                yield return (region, lx, lz);
            }
    }

    // =========================================================
    // STREAMING-SAFE QUERIES
    // =========================================================
    public static bool IsVertexAvailable(int wx, int wz)
    {
        foreach (var _ in EnumerateOwningVertexSlots(wx, wz))
            return true;

        return false;
    }

    // -------------------------
    // HEIGHT
    // -------------------------
    public static bool TryGetVertexHeight(int wx, int wz, out float h)
    {
        foreach (var (region, lx, lz) in EnumerateOwningVertexSlots(wx, wz))
        {
            h = region.data.tileHeightMap[lx, lz];
            return true;
        }

        h = 0f;
        return false;
    }

    public static bool TrySetVertexHeight(int wx, int wz, float h)
    {
        bool any = false;

        foreach (var (region, lx, lz) in EnumerateOwningVertexSlots(wx, wz))
        {
            region.data.tileHeightMap[lx, lz] = h;
            regionLoader?.MarkDirty(region.coord);
            any = true;
        }

        return any;
    }

    // -------------------------
    // COLOR
    // -------------------------
    public static bool TryGetVertexColor(int wx, int wz, out ByteColor c)
    {
        foreach (var (region, lx, lz) in EnumerateOwningVertexSlots(wx, wz))
        {
            c = region.data.vertexColors[lx, lz];
            return true;
        }

        c = ByteColor.White;
        return false;
    }

    public static bool TrySetVertexColor(int wx, int wz, ByteColor c)
    {
        bool any = false;

        foreach (var (region, lx, lz) in EnumerateOwningVertexSlots(wx, wz))
        {
            region.data.vertexColors[lx, lz] = c;
            regionLoader?.MarkDirty(region.coord);
            any = true;
        }

        return any;
    }

    // -------------------------
    // TEXTURE ID
    // -------------------------
    public static bool TryGetVertexTextureID(int wx, int wz, out ushort id)
    {
        foreach (var (region, lx, lz) in EnumerateOwningVertexSlots(wx, wz))
        {
            id = region.data.vertexTextureIDs[lx, lz];
            return true;
        }

        id = 0;
        return false;
    }

    public static bool TrySetVertexTextureID(int wx, int wz, ushort id)
    {
        bool any = false;

        foreach (var (region, lx, lz) in EnumerateOwningVertexSlots(wx, wz))
        {
            region.data.vertexTextureIDs[lx, lz] = id;
            regionLoader?.MarkDirty(region.coord);
            any = true;
        }

        return any;
    }

    // -------------------------
    // PBR (ByteColor)
    // -------------------------
    public static bool TryGetVertexPBR(int wx, int wz, out ByteColor pbr)
    {
        foreach (var (region, lx, lz) in EnumerateOwningVertexSlots(wx, wz))
        {
            pbr = region.data.vertexPBRValues[lx, lz];
            return true;
        }

        pbr = new ByteColor(0, 0, 0, 255);
        return false;
    }

    public static bool TrySetVertexPBR(int wx, int wz, ByteColor pbr)
    {
        bool any = false;

        foreach (var (region, lx, lz) in EnumerateOwningVertexSlots(wx, wz))
        {
            region.data.vertexPBRValues[lx, lz] = pbr;
            regionLoader?.MarkDirty(region.coord);
            any = true;
        }

        return any;
    }

    // =========================================================
    // TILE ACCESS (NO IMPLICIT LOADING)
    // =========================================================
    public static bool TryGetTile(int wx, int wz, out Tile tile)
    {
        tile = null;

        RegionCoord rc = RegionOfTile(wx, wz);
        if (!loadedRegions.TryGetValue(rc, out Region region) || region == null)
            return false;

        int lx = wx - region.coord.WorldX;
        int lz = wz - region.coord.WorldZ;

        if (lx < 0 || lz < 0 ||
            lx >= RegionCoord.REGION_SIZE ||
            lz >= RegionCoord.REGION_SIZE)
            return false;

        tile = region.data.tiles[lx, lz];
        return true;
    }

    public static bool TrySetTile(int wx, int wz, Tile tile)
    {
        RegionCoord rc = RegionOfTile(wx, wz);

        if (!loadedRegions.TryGetValue(rc, out Region region) || region == null)
            return false;

        int lx = wx - region.coord.WorldX;
        int lz = wz - region.coord.WorldZ;

        if (lx < 0 || lz < 0 ||
            lx >= RegionCoord.REGION_SIZE ||
            lz >= RegionCoord.REGION_SIZE)
            return false;

        region.data.tiles[lx, lz] = tile;
        regionLoader?.MarkDirty(region.coord);
        return true;
    }
}
