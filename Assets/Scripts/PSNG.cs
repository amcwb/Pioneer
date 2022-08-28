using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSNG
{
    private const long M = 4294967296;
    private const long A = 1664525;
    private const int C = 1;
    private float Z;
    public PSNG(int seed)
    {
        Z = seed * M;
    }

    public void SetInternalState(int z)
    {
        Z = z;
    }

    public float GetInternalState()
    {
        return Z;
    }

    public float Next()
    {
        Z = (A * Z + C) % M;
        return Z / M - 0.5f;
    }
}
