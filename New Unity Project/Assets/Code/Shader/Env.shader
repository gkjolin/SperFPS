Shader "Pixel/Env" {
	Properties {
		_Color1 ("Color1", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
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
			float3 worldPos;
			float3 worldNormal;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _IllumColor;
		half _Illum;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float3 fracPos = step(frac(IN.worldPos*0.5), 0.5);
			float3 absN = abs(IN.worldNormal);
			float X = abs((fracPos.y+fracPos.z)-1.0)*absN.x;
			float Y = abs((fracPos.z+fracPos.x)-1.0)*absN.y;
			float Z = abs((fracPos.x+fracPos.y)-1.0)*absN.z;
			fixed check = saturate(X + Y + Z);

			fixed4 c = lerp(_Color1, _Color2, smoothstep(_Color1.a, _Color2.a, IN.screenPos.y/IN.screenPos.w)) + check*0.05;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = _IllumColor.xyz*_Illum*IN.color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
