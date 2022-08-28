using UnityEngine;


abstract class FeatureProvider
{
    public abstract int?[,] Provide(int[] heights, int chunkX, int chunkY);
}
