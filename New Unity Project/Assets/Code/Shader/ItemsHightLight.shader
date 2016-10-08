Shader "Pixel/ItemsHightLight" {
	Properties {
		_Color1 ("Color1", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Illum ("Illum", Float) = 1.0
		_IllumColor ("IllumColor ", Color) = (1,1,1,1)
		_Grab ("Grab", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		struct Input {
			fixed4 color : COLOR;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _IllumColor;
		half _Illum;
		half _Grab;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			o.Normal = float3(0.0,0.0,1.0);
			fixed4 c = lerp(_Color1, _Color2, smoothstep(_Color1.a, _Color2.a, IN.screenPos.y/IN.screenPos.w));
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = _IllumColor.xyz*_Illum*IN.color.a + _Grab*(sin(_Time.y*10.0)*0.5+0.5);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
