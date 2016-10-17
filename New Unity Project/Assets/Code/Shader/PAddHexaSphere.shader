Shader "Pixel/Particles/Add HexaSphere" {
Properties {
	_TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_Multiplier ("Multiplier", Float) = 1.0
	_PushAmount ("Push Amount", Range(0.0,10.0)) = 1.0
	_PushDephase ("Push Dephase", Float) = 1.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
	Blend One One
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off

	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			float _Multiplier;
			float _PushAmount;
			float _PushDephase;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				float3 newPos = v.vertex.xyz+v.normal*v.color.a*_PushDephase*_PushAmount;
				o.vertex = UnityObjectToClipPos(float4(newPos.xyz, v.vertex.w));
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = _Multiplier * fixed4(i.color.xyz, 1.0) * _TintColor * tex2D(_MainTex, i.texcoord);
				UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0));
				return col;
			}
			ENDCG 
		}
	}	
}
}
