Shader "Pixel/Particles/Mask Add Multiplier Distortion" {
Properties {
	_TintColor1 ("Tint Color 1", Color) = (1.0,1.0,1.0,1.0)
	_TintColor2 ("Tint Color 2", Color) = (1.0,1.0,1.0,1.0)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_Multiplier ("Multiplier", Float) = 1.0
	_DistTex ("Distortion Texture", 2D) = "white" {}
	_DistParams ("XY = Noise Tile; ZW = Noise Speed", Vector) = (0.0,0.0,0.0,0.0)
	_DistX ("Distortion X", Float) = 0.0
	_DistY ("Distortion Y", Float) = 0.0
	_MaskStart ("MaskStart", Float) = 0.0
	_MaskEnd ("MaskEnd", Float) = 0.0
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
			#include "MyCGInclude.cginc"

			sampler2D _MainTex;
			sampler2D _DistTex;
			fixed4 _TintColor1;
			fixed4 _TintColor2;
			float _Multiplier;
			float4 _DistParams;
			float _DistX;
			float _DistY;
			float _MaskStart;
			float _MaskEnd;

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
				float fade = 1.0;
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				fade = saturate (_InvFade * (sceneZ-partZ));
				#endif

				fixed n = tex2D(_DistTex, i.texcoord.zw).x-0.5;
				fixed tex = tex2D(_MainTex, i.texcoord.xy + n*float2(_DistX, _DistY)*i.color.z).a;
				fixed alpha = linearSmoothStep(saturate(i.color.a + _MaskStart), saturate(i.color.a + _MaskEnd), tex);
				fixed4 col = lerp(_TintColor1, _TintColor2, alpha)*_Multiplier;
				fixed4 finalCol = fixed4(col.xyz, col.a*alpha*fade);
				UNITY_APPLY_FOG_COLOR(i.fogCoord, finalCol, fixed4(0,0,0,0)); // fog towards black due to our blend mode
				return finalCol;
			}
			ENDCG 
		}
	}	
}
}
