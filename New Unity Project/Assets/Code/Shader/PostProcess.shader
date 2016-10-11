Shader "Hidden/PostProcess"
{
	Properties
	{
		_MainTex("", 2D) = "" {}
		_Noise("_Noise", 2D) = "" {}
		_VignetSize ("VignetSize", Float) = 1.0
		_VignetColor ("VignetColor", Color) = (0.0,0.0,0.0,1.0)
		_HitColor ("_HitColor", Color) = (1.0,0.0,0.0,1.0)
		_HitRadius ("Hit Radius", Float) = 0.0
		_HitDist ("Hit Distortion", Float) = 0.0
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _Noise;

			float _VignetSize;
			fixed4 _VignetColor;
			fixed4 _HitColor;
			float4 _NoiseParams;

			half4 _HitDirection;
			uniform float4x4 _FrustumCornersWS;
			uniform float4 _CameraWS;
			half _RayMultiplier;
			half _HitRadius;
			half _HitDist;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 interpolatedRay : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;

				half index = v.uv.x + v.uv.y*2.0;
				o.interpolatedRay = _FrustumCornersWS[(int)index].xyz;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed hit = max(0.0, sin(saturate(smoothstep(_HitRadius*0.5, _HitRadius, length(normalize(i.interpolatedRay) - _HitDirection)))*3.14159265359));
				fixed4 noise = (tex2D(_Noise, i.uv*_NoiseParams.xy + floor(_Time.y*_NoiseParams.z)*_NoiseParams.w) - fixed4(0.5,0.5,0.5,0.5))*_HitColor.a;

				fixed v = saturate(length(i.uv+noise.xy*_HitDist - half2(0.5,0.5)));
				fixed4 vignet = lerp(half4(1.0,1.0,1.0,1.0), _VignetColor, smoothstep(0.0, _VignetSize, v));

				fixed colDistR = tex2D(_MainTex, i.uv + noise.x*hit*_HitDist).r;
				fixed colDistG = tex2D(_MainTex, i.uv + noise.y*hit*_HitDist).g;
				fixed colDistB= tex2D(_MainTex, i.uv + noise.z*hit*_HitDist).b;

				fixed4 colDist = half4(colDistR, colDistG, colDistB, 0.0);

				return colDist*vignet + (hit*0.5+v)*_HitColor*_HitColor.a;
			}
			ENDCG
		}
	}
}
