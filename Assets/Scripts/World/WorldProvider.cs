using UnityEngine;

/// <summary>
/// The <c>WorldProvider</c> class handles specific worlds,
/// their generation and the biomes. A seperate class,
/// <c>BiomeProvider</c>, handles biome generation.
/// </summary>
public class WorldProvider
{
    public int Seed;
    public float HeightExtremity;
    public float HeightSmoothness;
    public float TemperatureSmoothness;
    public float HumiditySmoothness;

    public int Octaves;
    public float OctaveDivisor;

    private BiomeProvider[] BiomeProviders;
    
    public WorldProvider(int seed, BiomeProvider[] biomeProviders, float heightExtremity, float heightSmoothness, float temperatureSmoothness, float humiditySmoothness, int octaves, float octaveDivisor)
    {
        Seed = seed;
        HeightExtremity = heightExtremity;
        HeightSmoothness = heightSmoothness;
        TemperatureSmoothness = temperatureSmoothness;
        HumiditySmoothness = humiditySmoothness;
        Octaves = octaves;
        OctaveDivisor = octaveDivisor;

        BiomeProviders = biomeProviders;
    }

    public int?[,] Provide(int chunkX, int chunkY)
    {
        int?[,] chunkData = new int?[Chunk.Size, Chunk.Size];
        int[] heights = RoundedHeightsForChunk(chunkX);
        float[] temperatures = TemperaturesForChunk(chunkX);
        float[] humidities = HumiditiesForChunk(chunkX);

        for (int bp = 0; bp < BiomeProviders.Length; bp++)
        {
            BiomeProvider biomeProvider = BiomeProviders[bp];
            if (!biomeProvider.IsChunkBiomeProvider(heights, temperatures, humidities))
            {
                // Don't waste computation on a provider that isn't needed
                continue;
            }

            int?[,] biomeProviderData = biomeProvider.Provide(
                Seed, chunkX, chunkY, heights, temperatures, humidities);

            for (int x = 0; x < Chunk.Size; x++)
                for (int y = 0; y < Chunk.Size; y++)
                {
                    if (biomeProviderData[x, y] == null) continue;

                    chunkData[x, y] = biomeProviderData[x, y];
                }
        }

        return chunkData;
    }

    public float[] HeightsForChunk(int chunkCoordinate)
    {
        float[] heights = new float[Chunk.Size];
        for (int i = 0; i < Chunk.Size; i++)
        {
            float blockChunkCoordinate = chunkCoordinate + (float)i / Chunk.Size;
            heights[i] = 0.0f;
            for (int j = 0; j < Octaves; j++)
            {
                float rawHeight = Mathf.PerlinNoise(
                    Mathf.Pow(j, 2) + Seed,
                    (float)blockChunkCoordinate / (HeightSmoothness / Mathf.Pow(OctaveDivisor, j)) + Seed
                ) * HeightExtremity / Mathf.Pow(OctaveDivisor, j);
                heights[i] += rawHeight;
            }
        }

        return heights;
    }

    public int[] RoundedHeightsForChunk(int chunkCoordinate)
    {
        float[] heights = HeightsForChunk(chunkCoordinate);
        int[] roundedHeights = new int[heights.Length];
        for (int i = 0; i < heights.Length; i++)
        {
            roundedHeights[i] = Mathf.RoundToInt(heights[i]);
        }

        return roundedHeights;
    }

    public float[] TemperaturesForChunk(int chunkCoordinate)
    {
        float[] temperatures = new float[Chunk.Size];
        for (int i = 0; i < Chunk.Size; i++)
        {
            float blockChunkCoordinate = chunkCoordinate + (float)i / Chunk.Size;
            float rawTemperature = Mathf.PerlinNoise(0.5f * Seed, (float)blockChunkCoordinate / TemperatureSmoothness + Seed);
            temperatures[i] = rawTemperature;
        }

        return temperatures;
    }

    public float[] HumiditiesForChunk(int chunkCoordinate)
    {
        float[] humidities = new float[Chunk.Size];
        for (int i = 0; i < Chunk.Size; i++)
        {
            float blockChunkCoordinate = chunkCoordinate + (float)i / Chunk.Size;
            float rawHumidity = Mathf.PerlinNoise(2.0f * Seed, (float)blockChunkCoordinate / TemperatureSmoothness + Seed);
            humidities[i] = rawHumidity;
        }

        return humidities;
    }
}

