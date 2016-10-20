Shader "Pixel/Collectable"
{
	Properties
	{
		_RimColor ("Rim Color", Color) = (1.0,1.0,1.0,1.0)
		_RimIn ("Rim In", Float) = 0.0
		_RimOut ("Rim Out", Float) = 1.0
		_SinParams ("Sin Params", Vector) = (0.0,0.0,0.0,0.0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows vertex:vert

		#pragma target 3.0

		fixed4 _RimColor;
		fixed _RimIn;
		fixed _RimOut;
		float _RotationSpeed;
		float4 _SinParams;

		struct Input {
			fixed4 color : COLOR;
			fixed rim;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input,o);
			float4 wp = mul(unity_ObjectToWorld, v.vertex);
			float sinus = sin(_Time.y*_SinParams.y + wp.x*_SinParams.z + wp.z*_SinParams.w)*_SinParams.x;
			v.vertex = v.vertex + float4(0.0, sinus, 0.0, 0.0);
			float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
            float dotProduct = smoothstep(_RimIn, _RimOut, dot(v.normal, viewDir));
            o.rim = dotProduct;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 col = IN.color;
			o.Albedo = col;
			o.Metallic = 0.0;
			o.Smoothness = 1.0;
			o.Alpha = 1.0;
			o.Emission = _RimColor*IN.rim;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
