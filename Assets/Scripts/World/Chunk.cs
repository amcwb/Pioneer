using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public const int Size = 32;

    private int[,] Tiles;
    private Dictionary<string, Object>[,] Data;

    public Chunk()
    {
        Tiles = new int[Size, Size];
    }

    public Chunk(int [,] tiles, Dictionary<string, Object>[,] data)
    {
        Tiles = tiles;
        Data = data;
    }
}
