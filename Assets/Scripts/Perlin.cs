using UnityEngine;

public class Perlin
{
    private float Amplitude;
    private float Wavelength;
    private readonly PSNG Generator;
    private readonly int Octaves;
    private readonly float Divisor;
    private SinglePerlin[] PerlinGenerators;

    public Perlin(float amplitude, float wavelength, PSNG psng, int octaves, float divisor)
    {
        Amplitude = amplitude;
        Wavelength = wavelength;
        Generator = psng;
        Octaves = octaves;
        Divisor = divisor;

        PerlinGenerators = new SinglePerlin[Octaves];
        for (int i = 0; i < Octaves; i++)
        {
            PerlinGenerators[i] = new SinglePerlin(amplitude, wavelength, psng);
        }
    }

    public float[] Generate(int width, int wavelengthOffset = 0)
    {
        float[][] octaveResult = new float[Octaves][];

        for (int i = 0; i < Octaves; i++)
        {
            octaveResult[i] = PerlinGenerators[i].Generate(width, wavelengthOffset);
        }

        // Combine octaves
        float[] result = new float[width];
        for (int i = 0; i < width; i++)
        {
            result[i] = 0.0f;
            for (int j = 0; j < Octaves; j++)
            {
                result[i] += octaveResult[j][i];
            }
        }

        return result;
    }
}
