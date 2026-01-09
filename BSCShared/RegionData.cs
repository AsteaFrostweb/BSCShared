using System;
using static Grid;

[Serializable] // you can remove this later
public class RegionData
{
    public const int REGION_SIZE = RegionCoord.REGION_SIZE;
    public const int VERT_SIZE = REGION_SIZE + 3;

    // Tiles (pathfinding / gameplay)
    public Tile[,] tiles = new Tile[REGION_SIZE, REGION_SIZE];

    // Vertex data
    public float[,] tileHeightMap = new float[VERT_SIZE, VERT_SIZE];    
    public ByteColor[,] vertexColors = new ByteColor[VERT_SIZE, VERT_SIZE];
    public ushort[,] vertexTextureIDs = new ushort[VERT_SIZE, VERT_SIZE];
    public ByteColor[,] vertexPBRValues = new ByteColor[VERT_SIZE, VERT_SIZE];

    public RegionData()
    {
        // Tiles
        for (int x = 0; x < REGION_SIZE; x++)
            for (int z = 0; z < REGION_SIZE; z++)
                tiles[x, z] = new Tile(new UShort2((ushort)x, (ushort)z), 0, 0);

        // Vertices
        for (int x = 0; x < VERT_SIZE; x++)
            for (int z = 0; z < VERT_SIZE; z++)
            {
                tileHeightMap[x, z] = 0f;
                vertexColors[x, z] = ByteColor.White;
                vertexTextureIDs[x, z] = 0;
                vertexPBRValues[x, z] = new ByteColor(0, 0, 0, 255); // sensible default             
            }
    }
}
