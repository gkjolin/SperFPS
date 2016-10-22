using UnityEngine;
using System.Collections;

public class HighLightSystem : MonoBehaviour {

	public Material material;
	public bool hightLight;

	private MeshFilter[] meshfilters;

	public void SetUp () {
		meshfilters = GetComponentsInChildren<MeshFilter>();
		hightLight = false;
	}

	void Update()
	{
		if(hightLight == true)
		{
			for(int i = 0; i < meshfilters.Length; i++)
			{
				Graphics.DrawMesh(meshfilters[i].mesh, meshfilters[i].transform.position, meshfilters[i].transform.rotation, material, 14, GameManager.instance.highLightCamera, 0);
			}
		}
	}
}
