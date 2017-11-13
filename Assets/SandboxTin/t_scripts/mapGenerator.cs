using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class mapGenerator : MonoBehaviour {

	public enum drawMode {noiseMap, colorMap, Mesh};
	public drawMode terrainDrawMode;
	public const int mapChunkSize = 241;
	[Range(0,6)]
	public int editorPreviewLOD;
	public float noiseScale;
	public int octaves;
	[Range(0,1)]
	public float persistence;
	public float lacunarity;
	public int seed;
	public Vector2 offset;
	public float meshHeightMultiplier;

	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;

	public TerrainType[] regions;

	Queue <MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue <MapThreadInfo<MapData>> ();
	Queue <MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue <MapThreadInfo<MeshData>> ();
	public void DrawMapInEditor () {
		MapData mapData = generateMapData(Vector2.zero);
		mapDisplay display = FindObjectOfType<mapDisplay> ();
		if(terrainDrawMode == drawMode.noiseMap) {
			display.drawTexture (textureGenerator.textureFromHeightMap(mapData.heightMap));
		}
		else if (terrainDrawMode == drawMode.colorMap) {
			display.drawTexture (textureGenerator.textureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
		}
		else if (terrainDrawMode == drawMode.Mesh) {
			display.drawMesh(meshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editorPreviewLOD), textureGenerator.textureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
		}
	}

	public void RequestMapData(Vector2 centre, Action<MapData> callback) {
		ThreadStart threadStart = delegate {
			MapDataThread(centre, callback);
		};

		new Thread (threadStart).Start ();
	}

	void MapDataThread (Vector2 centre, Action<MapData> callback) {
		MapData mapData = generateMapData (centre);
		lock (mapDataThreadInfoQueue) {
			mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback,mapData));
		}
	}

	public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback) {
		ThreadStart threadStart = delegate {
			MeshDataThread (mapData, lod, callback);
		};

		new Thread (threadStart).Start ();
	}

	void MeshDataThread (MapData mapData, int lod, Action<MeshData> callback) {
		MeshData meshData = meshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
		lock(meshDataThreadInfoQueue) {
			meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
		}
	}

	void Update () {
		if (mapDataThreadInfoQueue.Count>0) {
			for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
			{
				MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue (); 
				threadInfo.callback(threadInfo.parameter);
			}
		}

		if (meshDataThreadInfoQueue.Count>0) {
			for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
			{
				MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
				threadInfo.callback (threadInfo.parameter);

			}
		}
	}

	MapData generateMapData(Vector2 centre) {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize,mapChunkSize,seed, noiseScale, octaves, persistence, lacunarity, centre + offset);

		Color[] colorMap = new Color[mapChunkSize*mapChunkSize];

		for (int x = 0; x < mapChunkSize; x++) {
			for(int y = 0; y<mapChunkSize; y++) {
				float currentHeight = noiseMap[x,y];
				for(int i=0; i<regions.Length; i++) {
					if (currentHeight<=regions[i].height) {
						colorMap[y*mapChunkSize+x] = regions[i].color;
						break;
					}
				}
			}
		}

		return new MapData(noiseMap, colorMap);
		
	}
 
	void OnValidate() {
		if (lacunarity <1) {
			lacunarity = 1;
		}
		if (octaves <0) {
			octaves = 0;
		}
	}

	struct MapThreadInfo<T> {
		public  Action<T> callback;
		public  T parameter;

		public MapThreadInfo (Action<T> callback, T parameter)
		{
			this.callback = callback;
			this.parameter = parameter;
		}
		
	}
}


[System.Serializable]
 public struct TerrainType {
	 public string name;
	 public float height;
	 public Color color;
 }

 public struct MapData {
	 public  float[,] heightMap;
	 public  Color[] colorMap;

	 public MapData (float[,] heightMap, Color[] colorMap) {
		this.heightMap = heightMap;
		this.colorMap = colorMap;
	 }


 }