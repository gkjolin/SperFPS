Shader "Pixel/Checker" {
	Properties {
		_Color1 ("Color", Color) = (1,1,1,1)
		_Color2 ("Color", Color) = (1,1,1,1)
		_Size("Size", Float) = 1.0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float3 worldPos;
			float3 worldNormal;
		};

		half _Glossiness;
		half _Metallic;
		float _Size;
		fixed4 _Color1;
		fixed4 _Color2;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float3 fracPos = step(frac(IN.worldPos*_Size), 0.5);
			float3 absN = abs(IN.worldNormal);
			float X = abs((fracPos.y+fracPos.z)-1.0)*absN.x;
			float Y = abs((fracPos.z+fracPos.x)-1.0)*absN.y;
			float Z = abs((fracPos.x+fracPos.y)-1.0)*absN.z;
			fixed4 c = lerp(_Color1, _Color2, saturate(X + Y + Z));

			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
