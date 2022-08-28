using UnityEngine;

public class GenerateChunkTest : MonoBehaviour
{
    public int Seed;
    public int Width;
    public int Extremity;
    public int Offset;
    public float Smoothness;
    public GameObject DefaultTile;
    public int ChunkCoordinateOffset = 0;
    public int WorldSizeInChunks;
    // private WorldGenerator worldGenerator;


    // Start is called before the first frame update
    //void Start()
    //{
    //    worldGenerator = new WorldGenerator(Seed, Extremity, Smoothness, Width);
    //    Generate();
    //}

    //public void Generate()
    //{
    //    int[] heights = worldGenerator.RoundedHeightsForChunk(ChunkCoordinateOffset);
    //    // SinglePerlin generator = new SinglePerlin(Amplitude, Wavelength, new PSNG(1 / Seed));
    //    for (int i = 0; i < Width; i++)
    //    {
    //        int h = heights[i] + Offset;
    //        for (int j = 0; j < h; j++)
    //        {
    //            GameObject newTile = Instantiate(DefaultTile, Vector3.zero, Quaternion.identity);
    //            newTile.transform.parent = gameObject.transform;
    //            newTile.transform.localPosition = new Vector3(i, j);
    //        }
    //    }
    //}
}
