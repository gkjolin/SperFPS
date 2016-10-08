
Shader "Pixel/EnvUnlit"
{
	Properties {
		_Color1 ("Color1", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_IllumColor ("IllumColor ", Color) = (1,1,1,1)
		_Illum ("Illum", Float) = 1.0
		_TexParams1("_TexParams1", Vector) = (0.0,0.0,0.0,0.0)
		_TexParams2("_TexParams2", Vector) = (0.0,0.0,0.0,0.0)
		_TexParams3("_TexParams3", Vector) = (0.0,0.0,0.0,0.0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"  "LightMode" = "Always"}
		LOD 100

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
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float3 worldPos : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD2;
				fixed4 color : COLOR;
			};

			half4 _Color1;
			half4 _Color2;
			half4 _IllumColor;
			half _Illum;
			half4 _TexParams1;
			half4 _TexParams2;
			half4 _TexParams3;

			float rand(float3 uv)
			{   
			    float n = frac(sin(dot(uv.xy ,float2(12.9898,78.233))) * 43758.5453);    
			    return n;
			}


			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				//o.worldPos = v.vertex;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.color = v.color;
				return o;
			}
			
			half4 frag (v2f i) : SV_Target
			{
				half n = max(max(
				rand(floor(i.worldPos.xyz*_TexParams1.xyz)), 
				rand(floor(i.worldPos.xyz*_TexParams2.xyz))), 
				rand(floor(i.worldPos.xyz*_TexParams3.xyz)));

				half n2 = max(max(
				rand(floor(i.worldPos.xyz*_TexParams1.xyz*_TexParams3.w)), 
				rand(floor(i.worldPos.xyz*_TexParams2.xyz*_TexParams3.w))), 
				rand(floor(i.worldPos.xyz*_TexParams3.xyz*_TexParams3.w)));

				half4 baseCol = lerp(_Color1, _Color2, smoothstep(_Color1.a, _Color2.a, (i.screenPos.y/i.screenPos.w))) + n*_TexParams1.w + n2*_TexParams2.w;
				half4 col = lerp(baseCol, _IllumColor*_Illum, saturate(i.color.a*_Illum));

				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
