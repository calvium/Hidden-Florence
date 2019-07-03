Shader "Custom/frameShader" {
	Properties {
		// _Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Gold (RGB)", 2D) = "white" {}
		_BlackTex ("Black (RGB)", 2D) = "white" {}
		_HeightMap ("Height (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,2)) = 0.5
		_Metallic ("Metallic", Range(0,2)) = 0.0
		_Contrast("Contrast", float) = 60
		_Level("Level", Range(0, 1)) = 0.27
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex, _BlackTex, _HeightMap;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		float _Contrast, _Level;
		// fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 v = tex2D (_HeightMap, IN.uv_MainTex);
			// float4 alpha = saturate((v) * _Contrast);
			float4 alpha =v;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 b = tex2D (_BlackTex, IN.uv_MainTex);


			o.Albedo = c*alpha.r + b*(1-alpha.r);
			// Metallic and smoothness come from slider variables
			o.Metallic = (alpha+0.1)*_Metallic;
			o.Smoothness = (alpha+0.1)*_Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
