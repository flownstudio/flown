using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
