using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;

public class MeshBaking : EditorWindow {

	string groupName = "group";
	private string combined = "_Combined";

	[MenuItem("Tools/Mesh Baking")]

	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(MeshBaking));
	}

	void OnGUI () {
		groupName = EditorGUILayout.TextField ("Group Name", groupName);

		if(GUILayout.Button("Group Selected"))
		{
			Group ();
		}

		if(GUILayout.Button("Combine Group (Must be One Material)"))
		{
			CombineGroup ();
		}

		if(GUILayout.Button("UnCombine Group"))
		{
			UncombineGroup ();
		}

		if(GUILayout.Button("Check Materials"))
		{
			CheckMaterials ();
		}

		if(GUILayout.Button("Remove LODs"))
		{
			RemoveLODs ();
		}

		if(GUILayout.Button("Revert Prefabs"))
		{
			RevertPrefabs();
		}
	}

	void Group ()
	{
		GameObject go = new GameObject ();
		go.name = groupName;
		go.transform.position = Vector3.zero;
		go.transform.rotation = Quaternion.identity;

		Transform goParent = null;

		foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel))
		{
			goParent = t.parent;
			t.SetParent (go.transform);
		}

		if (goParent != null)
		{
			go.transform.SetParent (goParent);
		}
	}

	void CombineGroup()
	{
		Transform pt = Selection.activeTransform;

		if(CheckMaterials () == true)
		{
			Debug.Log ("Cannot combine multiple materials!");
		}
		else if(CheckLods () == true)
		{
			Debug.Log ("Cannot combine LODs!");
		}
		else if (!pt.gameObject.name.Contains (combined) ) 
		{
			MeshRenderer[] renderers = pt.GetComponentsInChildren<MeshRenderer> ();
			Material m = null;

			CombineInstance[] combine = new CombineInstance[renderers.Length];

			int i = 0;
			while (i < renderers.Length) 
			{
				combine[i].mesh = renderers[i].GetComponent<MeshFilter>().sharedMesh;
				combine[i].transform = renderers[i].transform.localToWorldMatrix;
				renderers[i].enabled = false;
				m = renderers[i].sharedMaterial;

				i++;
			}

			MeshFilter meshFilter = pt.gameObject.AddComponent<MeshFilter> ();
			MeshRenderer meshRenderer = pt.gameObject.AddComponent<MeshRenderer> ();
			meshFilter.sharedMesh = new Mesh ();
			meshFilter.sharedMesh.CombineMeshes (combine, true);
			meshFilter.sharedMesh.RecalculateBounds ();
			meshRenderer.material = m;

			pt.gameObject.name = pt.gameObject.name + combined;
			pt.gameObject.isStatic = true;
			Debug.Log ("Group Combined!");
		} 
		else 
		{
			Debug.Log ("Cannot combine already Combined objects!");
		}

		ApplyChanges ();
	}

	void UncombineGroup()
	{
		Transform pt = Selection.activeTransform;

		if (pt.gameObject.name.Contains (combined))
		{
			MeshRenderer[] meshRenderers = pt.GetComponentsInChildren<MeshRenderer> ();

			int i = 0;
			while (i < meshRenderers.Length) {
				meshRenderers [i].enabled = true;
				i++;
			}

			MeshFilter meshFilter = pt.gameObject.GetComponent<MeshFilter> ();
			MeshRenderer meshRenderer = pt.gameObject.GetComponent<MeshRenderer> ();
			DestroyImmediate (meshFilter.sharedMesh);
			DestroyImmediate (meshFilter);
			DestroyImmediate (meshRenderer);

			pt.gameObject.name = pt.gameObject.name.Substring (0, pt.gameObject.name.Length - combined.Length);
			Debug.Log ("Group Uncombined!");
		}
		else 
		{
			Debug.Log ("Cannot Uncombine not combined objects!");
		}

		ApplyChanges ();
	}

	bool CheckMaterials ()
	{
		Debug.Log ("Checking Materials...");

		Transform pt = Selection.activeTransform;

		MeshRenderer[] meshRenderers = pt.GetComponentsInChildren<MeshRenderer> ();

		bool check = false;

		int i = 0;
		while (i < meshRenderers.Length) 
		{
			Debug.Log (meshRenderers[i].sharedMaterial.name);
			if (i > 0 && meshRenderers [i].sharedMaterial.name != meshRenderers [i - 1].sharedMaterial.name) 
			{
				check = true;
			}

			i++;
		}
			
		return check;
	}

	bool CheckLods ()
	{
		Debug.Log ("Checking Lods...");

		Transform pt = Selection.activeTransform;

		LODGroup[] lodGroups = pt.GetComponentsInChildren<LODGroup> ();

		bool check = false;
		for (int i = 0; i < lodGroups.Length; i++) 
		{
			if (lodGroups [i].enabled == true) 
			{
				check = true;
			}
		}

		return check;
	}

	void RemoveLODs()
	{
		Transform pt = Selection.activeTransform;

		LODGroup[] lodGroups = pt.GetComponentsInChildren<LODGroup> ();

		int i = 0;
		while (i < lodGroups.Length) {
			lodGroups [i].enabled = false;
			Renderer[] childRenderer = lodGroups [i].transform.GetComponentsInChildren<Renderer> ();

			int j = 0;
			while (j < childRenderer.Length) {
				if (childRenderer [j].gameObject != null) 
				{
					if (childRenderer [j].gameObject.name.Contains ("LOD")) {
						DestroyImmediate (childRenderer [j].gameObject);
					}
				}
				j++;

				if (lodGroups [i] != null) 
				{
					DestroyImmediate (lodGroups [i]);
				}
			}
			i++;
		}
		Debug.Log ("No More LODs");
		ApplyChanges ();
	}

	void RevertPrefabs ()
	{
		Transform pt = Selection.activeTransform;

		foreach (Transform child in pt) 
		{
			PrefabUtility.RevertPrefabInstance (child.gameObject);
			Debug.Log(child.gameObject.name + " reverted!");
		}
		ApplyChanges ();
	}

	void ApplyChanges ()
	{
		EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
	}

}
