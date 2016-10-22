using System;
using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcess : MonoBehaviour
{
	public float vignetSize;
	public Color vignetColor;
	public Color hitColor;
	public float hitDuration;
	public float hitRadius;
	public float hitDistortion;
	public AnimationCurve hitEffectCurve;
	public AnimationCurve hitAlphaCurve;
	public float deathDuration;
	public AnimationCurve deathEffectCurve;
	public Color speedColor;
	public Texture2D noise;
	public Vector4 noiseParams;
	public bool pixelate;
	public int downsample;
	public Shader shader = null;
	public bool updateMaterial;
	public CameraRenderToTexture highLightTexture;

	[HideInInspector]
	public Vector3 hitPosition;
	[HideInInspector]
	public float speedEffect;
	[HideInInspector]
	public bool deathEffectFinished;

	private Transform trsf;
	private Camera cam;

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
		trsf = transform;
		cam = GetComponent<Camera>();

		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}

		if (!shader || !material.shader.isSupported) {
			enabled = false;
			return;
		}

		SetMaterial();
		material.SetFloat("_HitRadius", 0.0f);

		deathEffectFinished = true;

		highLightTexture.SetRendertexture();
	}

	void SetMaterial()
	{
		ReconstructCamera();
		material.SetFloat("_HitRadius", hitRadius);
		material.SetColor("_HitColor", hitColor);
		material.SetFloat("_HitDist", hitDistortion);
		material.SetFloat("_VignetSize", vignetSize);
		material.SetColor("_VignetColor", vignetColor);
		material.SetTexture("_Noise", noise);
		material.SetVector("_NoiseParams", noiseParams);
		material.SetColor("_SpeedColor", speedColor);
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) 
	{
		if(updateMaterial)
		{
			SetMaterial();
		}

		material.SetFloat("_SpeedEffect", speedEffect);
		material.SetTexture("_HighLight", highLightTexture.renderTexture);

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

	void ReconstructCamera()
	{
		float camNear = cam.nearClipPlane;
		float camFar = cam.farClipPlane;
		float camFov = cam.fieldOfView;
		float camAspect = cam.aspect;
		Transform camtr = cam.transform;

		Matrix4x4 frustumCorners = Matrix4x4.identity;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = camtr.right * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = camtr.up * camNear * Mathf.Tan (fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (camtr.forward * camNear - toRight + toTop);
		float camScale = topLeft.magnitude * camFar/camNear;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (camtr.forward * camNear + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (camtr.forward * camNear + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (camtr.forward * camNear - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		frustumCorners.SetRow (0, bottomLeft);
		frustumCorners.SetRow (1, bottomRight);
		frustumCorners.SetRow (2, topLeft);
		frustumCorners.SetRow (3, topRight);

		var camPos= camtr.position;

		material.SetMatrix ("_FrustumCornersWS", frustumCorners);
		material.SetVector ("_CameraWS", camPos);
	}

	public IEnumerator HitEffect()
	{
		Vector3 dir = (hitPosition - trsf.position).normalized;
		material.SetVector("_HitDirection", new Vector4(dir.x,dir.y,dir.z,0.0f));

		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float value = 0.0f;

		while(value < hitDuration)
		{
			ReconstructCamera();
			float ev = hitEffectCurve.Evaluate(value/hitDuration);
			float eva = hitAlphaCurve.Evaluate(value/hitDuration);
			material.SetFloat("_HitRadius", ev*hitRadius);
			material.SetColor("_HitColor", new Color(hitColor.r, hitColor.b, hitColor.b, eva));
			value += Time.deltaTime;
			yield return wait;
		}

		material.SetFloat("_HitRadius", 0.0f);
		material.SetColor("_HitColor", new Color(0.0f, 0.0f, 0.0f, 0.0f));
	}

	public IEnumerator DeathEffect()
	{
		deathEffectFinished = false;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float value = 0.0f;

		while(value < deathDuration)
		{
			ReconstructCamera();
			float ev = deathEffectCurve.Evaluate(value/deathDuration);
			material.SetFloat("_Death", ev);
			value += Time.deltaTime;
			yield return wait;
		}

		material.SetFloat("_Death", 1.0f);
		deathEffectFinished = true;
	}

	public IEnumerator RespawnEffect()
	{
		deathEffectFinished = false;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		float value = deathDuration;

		while(value > 0.0f)
		{
			ReconstructCamera();
			float ev = deathEffectCurve.Evaluate(value/deathDuration);
			material.SetFloat("_Death", ev);
			value -= Time.deltaTime;
			yield return wait;
		}

		material.SetFloat("_Death", 0.0f);
		deathEffectFinished = true;
	}
		
}
