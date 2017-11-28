using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlessTerrain : MonoBehaviour {

	const float scale = 5f;
	const float viewerMoveThresholdforChunkUpdate = 25f;
	const float sqrviewerMoveThresholdforChunkUpdate = viewerMoveThresholdforChunkUpdate * viewerMoveThresholdforChunkUpdate;
	public LODInfo[] detailLevels; 
	public static float maxViewDist;
	public Transform viewer;
	public Material mapMaterial;

	public static Vector2 viewerPosition;
	Vector2 viewerPositionOld;
	static mapGenerator MapGenerator;
	int chunkSize;
	int chunksVisibleInViewDst;

	Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
	static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>(); 

	private bool initialized = false;

	void Start() {
//		initTerrain ();
	}

	public void initTerrain(){
		initialized = true;
		terrainChunksVisibleLastUpdate.Clear();
		MapGenerator = FindObjectOfType<mapGenerator>();
		maxViewDist = detailLevels[detailLevels.Length-1].visibleDstThreshold;
		chunkSize = mapGenerator.mapChunkSize-1;
		chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDist/chunkSize);
		UpdateVisibleChunks();
	}

	void Update() {
		if (initialized) {
			viewerPosition = new Vector2 (viewer.position.x, viewer.position.z) / scale;
			if ((viewerPositionOld - viewerPosition).sqrMagnitude > sqrviewerMoveThresholdforChunkUpdate) {
				viewerPositionOld = viewerPosition;
				UpdateVisibleChunks ();
			}
		}
	}

	void UpdateVisibleChunks() {

		for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
		{
			terrainChunksVisibleLastUpdate[i].setVisible(false);
		}

		terrainChunksVisibleLastUpdate.Clear();

		int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x/chunkSize);
		int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y/chunkSize);

		for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++) 
		{
			for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
			{
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);	

				if (terrainChunkDictionary.ContainsKey(viewedChunkCoord)){
					terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk ();
				}
				else {
					terrainChunkDictionary.Add (viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
				}
			}
		}
	}

	public class TerrainChunk {
		
		GameObject meshObject;
		Vector2 position;
		Bounds bounds;

		MeshRenderer meshRenderer;
		MeshFilter meshFilter;

		LODInfo[] detailLevels;
		LODMesh[] lodMeshes;

		MapData mapData;
		bool mapDataReceived;

		int previousLODIndex = -1;
		

		public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material) {
			this.detailLevels = detailLevels;
			position = coord * size;
			bounds = new Bounds(position, Vector2.one * size);
			Vector3 positionV3 = new Vector3(position.x,0,position.y);

			meshObject = new GameObject("Terrain Chunk");
			meshRenderer = meshObject.AddComponent<MeshRenderer>();
			meshFilter = meshObject.AddComponent<MeshFilter>();
			meshRenderer.material = material;
			meshObject.transform.position = positionV3 * scale;
			meshObject.transform.parent = parent;
			meshObject.transform.localScale = Vector3.one * scale;
			setVisible(false);

			lodMeshes = new LODMesh[detailLevels.Length];
			for (int i = 0; i < detailLevels.Length; i++)
			{
				lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
			}

			MapGenerator.RequestMapData(position, onMapDataReceived);
		}

		void onMapDataReceived (MapData mapData) {
			this.mapData = mapData;
			mapDataReceived = true;

			Texture2D texture = textureGenerator.textureFromColorMap(mapData.colorMap, mapGenerator.mapChunkSize, mapGenerator.mapChunkSize);
			meshRenderer.material.mainTexture = texture;
			UpdateTerrainChunk();
		}


		public void UpdateTerrainChunk() {
			if (mapDataReceived) {

				float viewersDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
				bool visible = viewersDstFromNearestEdge <= maxViewDist;

				if(visible) {
					int lodIndex = 0;
					for (int i = 0; i<detailLevels.Length-1; i++) {
						if (viewersDstFromNearestEdge > detailLevels[i].visibleDstThreshold) {
							lodIndex = i+1;
						} else {
							break;
						}
					}
					if (lodIndex != previousLODIndex) {
						LODMesh lodMesh = lodMeshes [lodIndex];
						if(lodMesh.hasMesh) {
							previousLODIndex = lodIndex;
							meshFilter.mesh = lodMesh.mesh;
						}
						else if (!lodMesh.hasRequestedMesh) {
							lodMesh.RequestMesh(mapData);
						}
					}
					terrainChunksVisibleLastUpdate.Add (this);
				}
				setVisible(visible);
			}
		}

		public void setVisible(bool visible) {
			meshObject.SetActive (visible);

		}

		public bool isVisible() {
			return meshObject.activeSelf;
		}
	}

	class LODMesh {
		public Mesh mesh;
		public bool hasRequestedMesh;
		public bool hasMesh;
		int lod;
		System.Action updateCallback;
		public LODMesh(int lod, System.Action updateCallback) {
			this.lod = lod;
			this.updateCallback = updateCallback;
		}

		void OnMeshDataReceived(MeshData meshData){
			mesh = meshData.createMesh ();
			hasMesh = true;

			updateCallback();
		} 

		public void RequestMesh(MapData mapData) {
			hasRequestedMesh = true;
			MapGenerator.RequestMeshData (mapData, lod, OnMeshDataReceived);
		}
	}
	[System.Serializable]
	public struct LODInfo {
		public int lod;
		public float visibleDstThreshold;
	}

}