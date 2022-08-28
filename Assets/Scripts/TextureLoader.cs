using System.Collections.Generic;
using UnityEngine;

public static class TextureLoader
{
    // http://www.cr31.co.uk/stagecast/wang/blob.html
    static Dictionary<string, object> resourceCache = new Dictionary<string, object>();
    public static T Load<T>(string path) where T : Object
    {
        if (!resourceCache.ContainsKey(path))
            resourceCache[path] = Resources.Load<T>(path);
        return (T)resourceCache[path];
    }

    public static T[] LoadAll<T>(string path) where T : Object
    {
        if (!resourceCache.ContainsKey(path))
            resourceCache[path] = Resources.LoadAll<T>(path);
        
        return (T[])resourceCache[path];
    }

    public static Dictionary<int, int> TileIndexToTextureIndex;

    public static void PopulateTileIndexToTextureIndex() {
        // Reinitialise
        TileIndexToTextureIndex = new Dictionary<int, int>();
        
        int[] baseIndexes = { 0, 1, 5, 7, 17, 21, 23, 29, 31, 85, 87, 95, 119, 127, 255 };
        int textureIndex = 0;
        for (int i = 0; i < baseIndexes.Length; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int index = baseIndexes[i] * (int)Mathf.Pow(4, j);
                index %= 255;
                if (!TileIndexToTextureIndex.ContainsKey(index))
                {
                    TileIndexToTextureIndex.Add(index, textureIndex);
                    MonoBehaviour.print(index.ToString() + " -> " + textureIndex.ToString());
                    textureIndex++;
                }
            }
        }

        TileIndexToTextureIndex.Add(255, 46);
    }

    public static int GetTextureIndex(int?[,] surroundings)
    {
        int? block = surroundings[1, 1];
        // If edge empty
        if (surroundings[1, 0] != block)
        {
            surroundings[0, 0] = null;
            surroundings[2, 0] = null;
        }

        if (surroundings[1, 2] != block)
        {
            surroundings[0, 2] = null;
            surroundings[2, 2] = null;
        }

        if (surroundings[0, 1] != block)
        {
            surroundings[0, 0] = null;
            surroundings[0, 2] = null;
        }

        if (surroundings[2, 1] != block)
        {
            surroundings[2, 0] = null;
            surroundings[2, 2] = null;
        }

        int top = surroundings[1, 0] != null ? 1 : 0;
        int topRight = surroundings[2, 0] != null ? 2 : 0;
        int right = surroundings[2, 1] != null ? 4 : 0;
        int bottomRight = surroundings[2, 2] != null ? 8 : 0;
        int bottom = surroundings[1, 2] != null ? 16 : 0;
        int bottomLeft = surroundings[0, 2] != null ? 32 : 0;
        int left = surroundings[0, 1] != null ? 64 : 0;
        int topLeft = surroundings[0, 0] != null ? 128 : 0;

        int index = top + topRight + right + bottomRight + bottom + bottomLeft + left + topLeft;
        int tileindex;

        //MonoBehaviour.print(index);
        TileIndexToTextureIndex.TryGetValue(index, out tileindex);

        return tileindex;
    }
}