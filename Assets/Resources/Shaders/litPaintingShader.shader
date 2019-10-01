Shader "Custom/litPaintingShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_AlphaTex ("Albedo (RGB)", 2D) = "white" {}
		_GlowTex ("Glow (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Level ("Level", Range(-0.2,1.2)) = 0.0
		_Contrast("Contrast", float) = 60
		_GlowLevel("Glow Level", float) = 60
	}
	SubShader {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200
   
        CGPROGRAM
 
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

		sampler2D _MainTex, _AlphaTex, _GlowTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Level, _Contrast, _GlowLevel;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			
			fixed4 a = tex2D (_AlphaTex, IN.uv_MainTex);
			float4 alpha = saturate((a - _Level) * _Contrast);
			float4 glowAlpha = saturate((a - _Level+_GlowLevel) * _Contrast);
			

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 g = tex2D (_GlowTex, IN.uv_MainTex);
			o.Albedo = (c.rgb * alpha.r) + (g.rgb * (glowAlpha.r-alpha.r)*2);
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = (glowAlpha.r*c.a);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
