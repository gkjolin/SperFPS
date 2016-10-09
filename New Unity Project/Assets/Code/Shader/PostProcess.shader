Shader "Hidden/PostProcess"
{
	Properties
	{
		_MainTex("", 2D) = "" {}
		_VignetSize ("VignetSize", Float) = 1.0
		_VignetColor ("VignetColor", Color) = (0.0,0.0,0.0,1.0)
		_HitColor ("_HitColor", Color) = (1.0,0.0,0.0,1.0)
		_HitRadius ("Hit Radius", Float) = 0.0
		_RayMultiplier ("_RayMultiplier", Float) = 0.003
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

			float _VignetSize;
			fixed4 _VignetColor;
			fixed4 _HitColor;

			half4 _HitDirection;
			uniform float4x4 _FrustumCornersWS;
			uniform float4 _CameraWS;
			half _RayMultiplier;
			half _HitRadius;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 interpolatedRay : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;

				half index = v.uv.x + v.uv.y*2.0;
				o.interpolatedRay = _FrustumCornersWS[(int)index];
				o.interpolatedRay.w = index;

				return o;
			}
			
			sampler2D _MainTex;


			fixed4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);
				half v = length(i.uv - half2(0.5,0.5));
				half4 vignet = lerp(half4(1.0,1.0,1.0,1.0), _VignetColor, smoothstep(0.0, _VignetSize, v));

				half hit = 1.0 - saturate(smoothstep(_HitRadius, _HitRadius+0.25, length(i.interpolatedRay.xyz*_RayMultiplier - _HitDirection.xyz)));

				return col*vignet + (hit + v*0.5)*_HitColor*_HitColor.a;
			}
			ENDCG
		}
	}
}
