using UnityEngine;
using System.Collections;
using UnityEditor;

public class ColliderProcessor : AssetPostprocessor {

	void OnPreprocessModel()
	{
		if(assetPath.Contains("_Coll"))
		{
			ModelImporter modelImporter = (ModelImporter)assetImporter;
			modelImporter.importMaterials = false;
		}
	}

	void OnPostprocessModel(GameObject go)
	{
		if(assetPath.Contains("_Coll"))
		{
			Renderer[] rdrs = go.GetComponentsInChildren<Renderer>();

			for(int i = 0; i < rdrs.Length; i++)
			{
				MeshFilter mf = rdrs[i].gameObject.GetComponent<MeshFilter>();
				if(rdrs[i].gameObject.name.Contains("_Box"))
				{
					rdrs[i].gameObject.AddComponent<BoxCollider>();
					UnityEngine.Object.DestroyImmediate(rdrs[i]);
					UnityEngine.Object.DestroyImmediate(mf);
				}
				else if(rdrs[i].gameObject.name.Contains("_Mesh"))
				{
					rdrs[i].gameObject.AddComponent<MeshCollider>();
					UnityEngine.Object.DestroyImmediate(rdrs[i]);
					UnityEngine.Object.DestroyImmediate(mf);
				}
			}
		}

		if(assetPath.Contains("_LD"))
		{
			Renderer[] rdrs = go.GetComponentsInChildren<Renderer>();

			for(int i = 0; i < rdrs.Length; i++)
			{
				if(rdrs[i].gameObject.name.Contains("_Box"))
				{
					rdrs[i].gameObject.AddComponent<BoxCollider>();
				
				}
				else if(rdrs[i].gameObject.name.Contains("_Mesh"))
				{
					rdrs[i].gameObject.AddComponent<MeshCollider>();
				}
				rdrs[i].gameObject.isStatic = true;
			}
		}
	}
}
