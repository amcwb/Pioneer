using UnityEngine;

public class SinglePerlin
{
    private readonly float Amplitude;
    private readonly float Wavelength;
    private readonly PSNG Generator;
    public float A;
    public float B;

    public SinglePerlin(float amplitude, float wavelength, PSNG psng)
    {
        Amplitude = amplitude;
        Wavelength = wavelength;
        Generator = psng;

        A = Generator.Next();
        B = Generator.Next();
    }

    public SinglePerlin(float amplitude, float wavelength, PSNG psng, float a, float b)
    {
        Amplitude = amplitude;
        Wavelength = wavelength;
        Generator = psng;
        A = a;
        B = b;
    }

    public float[] Generate(int width, int wavelengthOffset = 0)
    {
        float[] results = new float[width];
        for (int x = wavelengthOffset; x < width + wavelengthOffset; x++)
        {
            if (x % Wavelength == 0)
            {
                A = B;
                B = Generator.Next();
                results[x - wavelengthOffset] = A * Amplitude;
            }
            else
            {
                // Interpolate between A and B, where X is relative to wave start.
                // NOTE: Uses SmoothStep instead of cosine interpolation!
                results[x - wavelengthOffset] = Mathf.SmoothStep(
                    A, B, (x % Wavelength) / Wavelength) * Amplitude;
            }
        }

        return results;
    }
}
