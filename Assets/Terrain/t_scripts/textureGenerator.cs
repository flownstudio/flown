using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class textureGenerator {
	
	public static Texture2D textureFromColorMap(Color[] colorMap,int width, int height) {
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colorMap);
		texture.Apply();
		return texture;
	} 

	public static Texture2D textureFromHeightMap (float[,] heightMap) {
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);

		Texture2D texture = new Texture2D (width, height);

		Color[] colorMap = new Color[width*height];
		for(int x=0; x<width; x++) {
			for(int y = 0; y<height; y++) {
				colorMap[y*width + x] = Color.Lerp(Color.black, Color.white, heightMap [x,y]); 
			
			}
		}
		return textureFromColorMap (colorMap, width, height);
	}
}
