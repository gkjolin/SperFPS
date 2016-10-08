using System;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Blur/ZoomBlur")]
public class PixelEffect : MonoBehaviour
{
	public int pixelSize;

    protected void Start()
    {
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
    }
	
    void OnRenderImage (RenderTexture source, RenderTexture destination) {
		RenderTexture buffer = RenderTexture.GetTemporary(source.width/pixelSize, source.height/pixelSize, 0);
		buffer.filterMode = FilterMode.Point;
		Graphics.Blit(source, buffer);
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }
}
