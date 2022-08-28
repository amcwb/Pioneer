using UnityEngine;

/// <summary>
/// The <c>BiomeProvider</c> class handles biomes,
/// their blocks and features.
/// </summary>
public class BiomeProvider
{
    public struct EnvironmentOptions
    {
        public float[] TemperatureRange;
        public float[] HumidityRange;
        public int[] AltitudeRange;
    }

    public struct Layer
    {
        public int blockId;
        public int minHeight;
        public int maxHeight;

        public int Range()
        {
            return maxHeight - minHeight;
        }
    }

    public string Name;
    private EnvironmentOptions environmentOptions;
    private Layer[] layers;

    public BiomeProvider(string name, EnvironmentOptions options, Layer[] biomeLayers)
    {
        Name = name;
        environmentOptions = options;
        layers = biomeLayers;
    }

    public bool IsBiomeProvider(int height, float temperature, float humidity)
    {
        return temperature >= environmentOptions.TemperatureRange[0]
                && temperature <= environmentOptions.TemperatureRange[1]
                && humidity >= environmentOptions.HumidityRange[0]
                && humidity <= environmentOptions.HumidityRange[1]
                && height >= environmentOptions.AltitudeRange[0]
                && height <= environmentOptions.AltitudeRange[1];
    }

    public bool IsChunkBiomeProvider(int[] heights, float[] temperatures, float[] humidities)
    {
        for (int i = 0; i < heights.Length; i++)
        {
            if (IsBiomeProvider(heights[i], temperatures[i], humidities[i])) return true;
        }

        return false;
    }

    public int?[,] Provide(int seed, int chunkX, int chunkY, int[] heights, float[] temperatures, float[] humidities, bool checkConditions = true)
    {
        int?[,] result = new int?[Chunk.Size, Chunk.Size];


        for (int x = 0; x < Chunk.Size; x++)
        {
            // Ensure that the temperatures and humidities fit the biome.
            // As well as the altitude.
            float currentTemperature = temperatures[x];
            float currentHumidity = humidities[x];
            int currentAltitude = heights[x];
            if (checkConditions && !IsBiomeProvider(currentAltitude, currentTemperature, currentHumidity)) continue;
            
            int realX = chunkX * Chunk.Size + x;
            for (int l = layers.Length - 1; l >= 0; l--)
            {
                int topY = heights[x] + layers[l].maxHeight;
                int bottomY = topY - Mathf.RoundToInt(
                    Mathf.PerlinNoise(l + seed, realX + seed) * layers[l].Range());
                
                for (int i = topY; i > bottomY; i--)
                {
                    int relativeY = i - chunkY * Chunk.Size;

                    if (relativeY >= 0 && relativeY < Chunk.Size)
                    {
                        result[x, relativeY] = layers[l].blockId;
                    }
                }
            }
        }

        return result;
    }
}

