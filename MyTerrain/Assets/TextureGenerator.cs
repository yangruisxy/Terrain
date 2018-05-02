using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator{
    public static Texture2D Texture2DFromColorMap(Color[] colorMap, int width, int length)
    {
        Texture2D texture = new Texture2D(width, length);
        //texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }
}
