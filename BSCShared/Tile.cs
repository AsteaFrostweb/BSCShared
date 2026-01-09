
using System;
using System.Collections.Generic;
using static Grid;

[Serializable]
public class Tile
{
    //Poperties and Fields
    public UShort2 layerPosition { get; set; } // X,Z
    public ushort layer { get; set; }                // layer index
    public bool isWalkable { get; set; }             // optional


    public Tile() { }

    public Tile(UShort2 layerPosition, ushort layer, ushort textureID)
    {
        this.layerPosition = layerPosition;
        this.layer = layer;
    }
}
