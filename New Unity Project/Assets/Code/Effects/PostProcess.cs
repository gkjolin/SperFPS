using System;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcess : MonoBehaviour
{
	public float vignetSize;
	public Color vignetColor;
	public bool pixelate;
	public int downsample;

	public Shader shader = null;

	static Material m_Material = null;
	protected Material material {
		get {
			if (m_Material == null) {
				m_Material = new Material(shader);
				m_Material.hideFlags = HideFlags.DontSave;
			}
			return m_Material;
		}
	}

	protected void OnDisable() {
		if ( m_Material ) {
			DestroyImmediate( m_Material );
		}
	}

	protected void Start()
	{
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}

		if (!shader || !material.shader.isSupported) {
			enabled = false;
			return;
		}
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {

		material.SetFloat("_VignetSize", vignetSize);
		material.SetColor("_VignetColor", vignetColor);

		if(pixelate)
		{
			RenderTexture buffer = RenderTexture.GetTemporary(source.width/downsample, source.height/downsample, 0, RenderTextureFormat.DefaultHDR);
			buffer.filterMode = FilterMode.Point;
			Graphics.Blit(source, buffer);
			Graphics.Blit(buffer, destination, material);
			RenderTexture.ReleaseTemporary(buffer);
		}
		else
		{
			Graphics.Blit(source, destination, material);
		}
	}
}
