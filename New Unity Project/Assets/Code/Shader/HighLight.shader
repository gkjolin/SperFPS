Shader "Pixel/HighLight"
{
	Properties
	{
		_Color ("Color", Color) = (1.0,1.0,1.0,1.0)
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			ColorMask 0
			ZWrite On
		}

		Pass
		{
			ColorMask RGB
			Cull Back Lighting Off ZWrite Off// Ztest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				return _Color*(sin(_Time.y*10.0)*0.5+0.5);
			}
			ENDCG
		}
	}
}
