using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR 
	using UnityEditor;
#endif


#if UNITY_EDITOR 
[CustomEditor (typeof (mapGenerator))]

public class mapGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		mapGenerator mapgen = (mapGenerator)target;

		if (DrawDefaultInspector ()) {
			if (mapgen.autoUpdate) {
				mapgen.DrawMapInEditor();	
			}
		}

		if (GUILayout.Button("Generate")) {
			mapgen.DrawMapInEditor();
		}
	}
}
#endif