Shader "Hidden/PostProcess"
{
	Properties
	{
		_MainTex("", 2D) = "" {}
		_VignetSize ("VignetSize", Float) = 1.0
		_VignetColor ("VignetColor", Color) = (0.0,0.0,0.0,1.0)
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);
				half4 v = lerp(half4(1.0,1.0,1.0,1.0), _VignetColor, smoothstep(0.0, _VignetSize, length(i.uv - half2(0.5,0.5))));

				return col * v;
			}
			ENDCG
		}
	}
}
