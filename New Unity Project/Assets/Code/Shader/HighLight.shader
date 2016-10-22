Shader "Pixel/HighLight"
{
	Properties
	{
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
		//_Outline ("Outline", Float) = 0.01
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Blend SrcAlpha One
		ColorMask RGB
		Cull Front Lighting Off ZWrite Off Ztest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			fixed4 _Color;
//			half _Outline;

			struct appdata
			{
				float4 vertex : POSITION;
//				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

//				float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
//				float2 offset = TransformViewToProjection(norm.xy);
//
//				#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE //to handle recent standard asset package on older version of unity (before 5.5)
//					o.vertex.xy += offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(o.pos.z) * _Outline;
//				#else
//					o.vertex.xy += offset * o.vertex.z * _Outline;
//				#endif
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color*(sin(_Time.y*10.0)*0.5+0.5);
			}
			ENDCG
		}
	}
}
