using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WorldProviderBehaviour : MonoBehaviour
{
    public int Seed;
    public float HeightExtremity;
    public float HeightSmoothness;
    public float TemperatureSmoothness;
    public float HumiditySmoothness;

    public int Octaves;
    public float OctaveDivisor;

    public WorldProvider worldProvider;
    public BiomeProvider[] biomeProviders;

    public GameObject DefaultTile;

    // testingAAAAA
    [SerializeField] private Sprite[] sprites;

    public Material DefaultMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //DefaultTile.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        //DefaultTile.GetComponent<Renderer>().receiveShadows = true;

        BiomeProvider.EnvironmentOptions bpeo = new BiomeProvider.EnvironmentOptions {
            TemperatureRange = new float[] { 0.3f, 0.7f },
            HumidityRange = new float[] { 0.0f, 1.0f },
            AltitudeRange = new int[] { int.MinValue, int.MaxValue }
        };
        BiomeProvider.Layer[] layers = new BiomeProvider.Layer[]
        {
            new BiomeProvider.Layer { blockId = (int)BlockEnum.BASICDIRT, minHeight = -50, maxHeight = 0 },
            new BiomeProvider.Layer { blockId = (int)BlockEnum.BASICSTONE, minHeight = -100, maxHeight = -10 }
        };

        BiomeProvider.EnvironmentOptions bpeo2 = new BiomeProvider.EnvironmentOptions
        {
            TemperatureRange = new float[] { 0.7f, 0.9f },
            HumidityRange = new float[] { 0.0f, 1.0f },
            AltitudeRange = new int[] { int.MinValue, int.MaxValue }
        };
        BiomeProvider.Layer[] layers2 = new BiomeProvider.Layer[]
        {
            new BiomeProvider.Layer { blockId = (int)BlockEnum.SAND, minHeight = -50, maxHeight = 0 },
            new BiomeProvider.Layer { blockId = (int)BlockEnum.SANDSTONE, minHeight = -100, maxHeight = -10 }
        };

        BiomeProvider.EnvironmentOptions bpeo3 = new BiomeProvider.EnvironmentOptions
        {
            TemperatureRange = new float[] { 0.0f, 0.3f },
            HumidityRange = new float[] { 0.0f, 1.0f },
            AltitudeRange = new int[] { int.MinValue, int.MaxValue }
        };
        BiomeProvider.Layer[] layers3 = new BiomeProvider.Layer[]
        {
            new BiomeProvider.Layer { blockId = (int)BlockEnum.ICE, minHeight = -50, maxHeight = 0 },
            new BiomeProvider.Layer { blockId = (int)BlockEnum.ICE, minHeight = -100, maxHeight = -10 }
        };

        biomeProviders = new BiomeProvider[]
        {
            new BiomeProvider("Forest", bpeo, layers),
            new BiomeProvider("Desert", bpeo2, layers2),
            new BiomeProvider("Icey", bpeo3, layers3)
        };

        worldProvider = new WorldProvider(
            Seed,
            biomeProviders,
            HeightExtremity, HeightSmoothness, TemperatureSmoothness, HumiditySmoothness,
            Octaves, OctaveDivisor);


        //for (int chunkX = -5; chunkX < 20; chunkX++) 
        //    for (int chunkY = -5; chunkY < 5; chunkY++)
        //    {
        //        int?[,] data = worldProvider.Provide(chunkX, chunkY);

        //        for (int j = 0; j < data.GetLength(1); j++)
        //            for (int i = 0; i < data.GetLength(0); i++)
        //            {
        //                if (data[i, j] == null) continue;
        //                GameObject newTile = Instantiate(DefaultTile, Vector3.zero, Quaternion.identity);
        //                SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
        //                renderer.sprite = LoadBlockSpriteOrDebug(data[i, j] ?? 0);
        //                //newTile.transform.parent = gameObject.transform;
        //                newTile.transform.localPosition = new Vector3(
        //                    chunkX * Chunk.Size + i, chunkY * Chunk.Size + j);

        //                UnityEngine.Tilemaps.Tilemap tm = new UnityEngine.Tilemaps.Tilemap();
        //            }
        //    }


        // testingAAAAA
        TextureLoader.PopulateTileIndexToTextureIndex();
        for (int chunkX = -1; chunkX < 5; chunkX++)
            for (int chunkY = -1; chunkY < 5; chunkY++)
            {
                int?[,] data = worldProvider.Provide(chunkX, chunkY);

                for (int j = 0; j < data.GetLength(1); j++)
                    for (int i = 0; i < data.GetLength(0); i++)
                    {
                        if (data[i, j] == null) continue;
                        GameObject newTile = Instantiate(DefaultTile, Vector3.zero, Quaternion.identity);
                        SpriteRenderer renderer = newTile.GetComponent<SpriteRenderer>();
                        renderer.sprite = LoadTiledSpriteOrDebug(data[i, j] ?? 0, GetSurroundings(i, j, data));
                        renderer.material = DefaultMaterial;
                        //newTile.transform.parent = gameObject.transform;
                        newTile.transform.localPosition = new Vector3(
                            chunkX * Chunk.Size + i, chunkY * Chunk.Size + j);


                        UnityEngine.Tilemaps.Tilemap tm = new UnityEngine.Tilemaps.Tilemap();
                    }
            }
    }

    // testingAAAAA
    public static int?[,] GetSurroundings(int x, int y, int?[,] data)
    {
        int?[,] surroundings = new int?[3, 3];
        for (int i = -1; i < 2; i++) 
            for (int j = -1; j < 2; j++)
            {
                int thisX = x + i;
                int thisY = y + j;
                if (thisX < 0 || thisX >= data.GetLength(0) || thisY < 0 || thisY >= data.GetLength(1))
                    continue;

                surroundings[i + 1, 1 - j] = data[thisX, thisY];
            }


        return surroundings;
    }

    public static Sprite LoadBlockSprite(int blockId)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "Scripts", blockId.ToString());
        return LoadNewSprite(filePath, 16.0f);
    }

    // testingAAAAA
    public Sprite LoadTiledSpriteOrDebug(int blockId, int?[,] surroundings)
    {
        //return sprites[TextureLoader.GetTextureIndex(surroundings)];

        Sprite[] sprites = TextureLoader.LoadAll<Sprite>("Tiles/" + blockId.ToString());

        if (sprites.Length == 0)
        {
            sprites = TextureLoader.LoadAll<Sprite>("Tiles/0");
        }

        Sprite sprite = sprites[TextureLoader.GetTextureIndex(surroundings)];
        sprite.texture.filterMode = FilterMode.Point;
        
        return sprite;
    }

    public static Sprite LoadBlockSpriteOrDebug(int blockId)
    {

        Texture2D texture2D = VResources.Load<Texture2D>(blockId.ToString());

        if (texture2D == null)
        {
            texture2D = VResources.Load<Texture2D>("0");
        }

        texture2D.filterMode = FilterMode.Point;

        return ConvertTextureToSprite(texture2D, 16.0f);
    }

    public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }

    public static Sprite ConvertTextureToSprite(Texture2D texture, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        // Converts a Texture2D to a sprite, assign this texture to a new sprite and return its reference
        Sprite NewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }

    public static Texture2D LoadTexture(string FilePath)
    {
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
