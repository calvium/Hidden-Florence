Shader "frames/warpFromAlpha"
{
   Properties
   {
       	_MainTex ("Texture One", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
       	_FadeTex ("Texture Two", 2D) = "white" {}
		_GlowTex ("Glow Texture", 2D) = "white" {}
	   	_InkTex ("Ink Texture", 2D) = "white" {}
	   	_Stencil("StencilNum", int) = 6

		_BlackLevel("Black Level", Range(0, 1.5)) = 0.27
		_GlowBlackLevel("Glow Black Level", Range(0, 1)) = 0.27
        _Contrast("Contrast", float) = 60
		_Speed("Speed", float) = 6

		_Alpha("Alpha", Range(-0.5, 1.5)) = 1

		// _GlitchInterval ("Glitch interval time [seconds]", Float) = 0.16
		// _DispProbability ("Displacement Glitch Probability", Float) = 0.022
		// _DispIntensity ("Displacement Glitch Intensity", Float) = 0.09
		// _ColorProbability("Color Glitch Probability", Float) = 0.02
		// _ColorIntensity("Color Glitch Intensity", Float) = 0.07
		_Stencil("Mask ID", Int) = 1

   }
   SubShader 
   {
	    Stencil{
            Ref [_ID]
            Comp equal
   		}

		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

       	Pass {
            Cull Back
			CGPROGRAM
			// #pragma surface surf Lambert
			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram alpha
			#pragma target 3.0
			#pragma multi_compile DUMMY PIXELSNAP_ON

			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _MainTex, _FadeTex, _InkTex, _GlowTex;
			float4 _MainTex_ST, _FadeTex_ST, _InkTex_ST, _GlowTex_ST;

			float _BlackLevel, _GlowBlackLevel, _Contrast;
			half _Speed;
			fixed _Alpha;

			// float _GlitchInterval;
			// float _DispIntensity;
			// float _DispProbability;
			// float _ColorIntensity;
			// float _ColorProbability;
			// float _WrapDispCoords;
			float _sphere;

			// struct Input {
          	// 	float2 uv_MainTex;
      		// };
      		// sampler2D _MainTex2D;
      		// void surf (Input IN, inout SurfaceOutput o) {
          	// 	o.Albedo = tex2D (_MainTex2D, IN.uv_MainTex).rgb;
      		// }

			struct VertexData {
				float4 position : POSITION;
				float4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators {
				float4 position : SV_POSITION;
				fixed4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				i.color = v.color * _Color;
				return i;
			}

			float rand(float x, float y){
				return frac(sin(x*12.9898 + y*78.233)*43758.5453);
			}




			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				float2 uv = i.uv;
				uv.x = 1 - uv.x;


				float noise = tex2D(_InkTex, 0.3f * uv + _Time.x * float2(0.2f, 0.1f) *_Speed).r
                    * tex2D(_InkTex, uv + _Time.x *_Speed * float2(-0.3f, -0.1f) + float2(0.2f, 0.3f)).r;

				
				float v = tex2D(_InkTex,uv);
				//for fading between colours
				float4 alpha = saturate((v - _BlackLevel + noise) * _Contrast);
				float4 glowAlpha = saturate((v - _BlackLevel + _GlowBlackLevel + noise) * _Contrast);

				//for fading from nothing
				float4 alphaB = saturate((v - _Alpha) * _Contrast);
				float4 glowAlphaB = saturate((v - _Alpha + _GlowBlackLevel) * _Contrast);


                //uv.x = 1 - uv.x;
				fixed4 c = tex2D(_MainTex, uv);
				fixed4 d = tex2D(_FadeTex, uv);

				fixed4 col = 	(c * alpha.r) + 
								(tex2D(_GlowTex, uv) * (glowAlpha.r-alpha.r)) + 
								(d * (1- alpha.r));

				fixed4 colB = 	((col * (1- alphaB.r)) + 
								(tex2D(_GlowTex, uv) * (glowAlphaB.r-alphaB.r)));
				return colB;

				
			}
		ENDCG
		}
	}
}