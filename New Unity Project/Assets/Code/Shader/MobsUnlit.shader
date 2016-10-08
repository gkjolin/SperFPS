Shader "Pixel/MobsUnlit"
{
	Properties {
		_Color1 ("Color1", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_IllumColor ("IllumColor ", Color) = (1,1,1,1)
		_Illum ("Illum", Float) = 1.0
		_OutlineColor ("OutlineColor", Color) = (1,1,1,1)
		_Outline ("Outline", Float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"  "LightMode" = "Always"}
		LOD 100

		Pass
		{
			Name "OUTLINE"
			Cull Front
			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			
			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				UNITY_FOG_COORDS(0)
				fixed4 color : COLOR;
			};
			
			uniform float _Outline;
			uniform float4 _OutlineColor;
			
			v2f vert(appdata v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				float3 norm   = normalize(mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				float2 offset = TransformViewToProjection(norm.xy);

				#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE //to handle recent standard asset package on older version of unity (before 5.5)
					o.pos.xy += offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(o.pos.z) * _Outline;
				#else
					o.pos.xy += offset * o.pos.z * _Outline;
				#endif
				o.color = _OutlineColor;
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				UNITY_APPLY_FOG(i.fogCoord, i.color);
				return i.color;
			}
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD2;
				fixed4 color : COLOR;
			};

			half4 _Color1;
			half4 _Color2;
			half4 _IllumColor;
			half _Illum;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.color = v.color;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				half4 baseCol = lerp(_Color1, _Color2, smoothstep(_Color1.a, _Color2.a, i.screenPos.y/i.screenPos.w));
				half4 col = lerp(baseCol, _IllumColor*_Illum, saturate(i.color.a*_Illum));

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
