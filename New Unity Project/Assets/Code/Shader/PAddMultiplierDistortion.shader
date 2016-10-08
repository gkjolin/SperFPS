Shader "Pixel/Particles/Alpha Blended Multiplier Distortion" {
Properties {
	_TintColor ("Tint Color", Color) = (1.0,1.0,1.0,1.0)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_Multiplier ("Multiplier", Float) = 1.0
	_DistTex ("Distortion Texture", 2D) = "white" {}
	_DistParams ("XY = Noise Tile; ZW = Noise Speed", Vector) = (0.0,0.0,0.0,0.0)
	_DistX ("Distortion X", Float) = 0.0
	_DistY ("Distortion Y", Float) = 0.0
	_InvFade ("Soft Particles Factor", Float) = 1.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
	Blend SrcAlpha One
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
			sampler2D _DistTex;
			fixed4 _TintColor;
			float _Multiplier;
			float4 _DistParams;
			float _DistX;
			float _DistY;

			float4 _MainTex_ST;
			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
				UNITY_FOG_COORDS(1)

				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD2;
				#endif
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif

				o.color = v.color;
				o.texcoord.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.texcoord.zw = v.texcoord*_DistParams.xy + _DistParams.zw*v.color.xy;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif
				fixed n = tex2D(_DistTex, i.texcoord.zw).x-0.5;
				fixed4 col = _Multiplier * _TintColor * tex2D(_MainTex, i.texcoord.xy + n*float2(_DistX, _DistY)*i.color.z);
				fixed4 finalCol = fixed4(col.xyz, col.a*i.color.a);
				UNITY_APPLY_FOG_COLOR(i.fogCoord, finalCol, fixed4(0,0,0,0)); // fog towards black due to our blend mode
				return finalCol;
			}
			ENDCG 
		}
	}	
}
}
