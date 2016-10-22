using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraRenderToTexture : MonoBehaviour {

	[HideInInspector]
	public RenderTexture renderTexture;
	private Camera cam;

	void Awake () {
		cam = GetComponent<Camera>();
	}

	public void SetRendertexture()
	{
		renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
		cam.targetTexture = renderTexture;
	}

}
