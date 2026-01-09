using System;
using static Grid;

[Serializable]
public class Region
{
    public RegionCoord coord;
    public RegionData data;

    public Region(RegionCoord coord)
    {
        this.coord = coord;
        this.data = new RegionData();
    }

    // -------------------------
    // Tile Access
    // -------------------------
    public Tile GetTile(int x, int z)
    {
        if (x < 0 || x >= RegionData.REGION_SIZE || z < 0 || z >= RegionData.REGION_SIZE)
            return null;
        return data.tiles[x, z];
    }

    public void SetTile(int x, int z, Tile tile)
    {
        if (x < 0 || x >= RegionData.REGION_SIZE || z < 0 || z >= RegionData.REGION_SIZE)
            return;
        data.tiles[x, z] = tile;
    }

    // -------------------------
    // Vertex Height Access
    // -------------------------
    public float GetVertexHeight(int x, int z)
    {
        if (x < 0 || x > RegionData.REGION_SIZE || z < 0 || z > RegionData.REGION_SIZE)
            return 0f;
        return data.tileHeightMap[x, z];
    }

    public void SetVertexHeight(int x, int z, float h)
    {
        if (x < 0 || x > RegionData.REGION_SIZE || z < 0 || z > RegionData.REGION_SIZE)
            return;
        data.tileHeightMap[x, z] = h;
    }

    // -------------------------
    // Vertex Color Access
    // -------------------------
    public ByteColor GetVertexColor(int x, int z)
    {
        if (x < 0 || x > RegionData.REGION_SIZE || z < 0 || z > RegionData.REGION_SIZE)
            return ByteColor.White;
        return data.vertexColors[x, z];
    }

    public void SetVertexColor(int x, int z, ByteColor c)
    {
        if (x < 0 || x > RegionData.REGION_SIZE || z < 0 || z > RegionData.REGION_SIZE)
            return;
        data.vertexColors[x, z] = c;
    }

    // -------------------------
    // Vertex Texture ID Access
    // -------------------------
    public ushort GetVertexTextureID(int x, int z)
    {
        if (x < 0 || x > RegionData.REGION_SIZE || z < 0 || z > RegionData.REGION_SIZE)
            return 0;
        return data.vertexTextureIDs[x, z];
    }

    public void SetVertexTextureID(int x, int z, ushort id)
    {
        if (x < 0 || x > RegionData.REGION_SIZE || z < 0 || z > RegionData.REGION_SIZE)
            return;
        data.vertexTextureIDs[x, z] = id;
    }
}
