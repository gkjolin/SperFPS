Shader "Pixel/Mob" {
	Properties {
		_Color1 ("_Color1", Color) = (1,1,1,1)
		_Color2 ("_Color2", Color) = (1,1,1,1)
		_ColorLevel ("_ColorLevel", Color) = (1,1,1,1)
		_Illum ("Illum", Float) = 1.0
		_IllumColor ("IllumColor ", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		struct Input {
			fixed4 color : COLOR;
		};

		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _ColorLevel;
		fixed4 _IllumColor;
		half _Illum;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = lerp(lerp(lerp(fixed4(0.0,0.0,0.0,0.0), _Color1, IN.color.r), _ColorLevel, IN.color.g), _Color2, IN.color.b);
			o.Albedo = c.rgb;
			o.Metallic = 0.0;
			o.Smoothness = 1.0;
			o.Alpha = 1.0;
			o.Emission = _IllumColor.xyz*_Illum*IN.color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
