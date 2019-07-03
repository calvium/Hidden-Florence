Shader "frames/warpSphere_00"
{

   Properties
   {
       	_MainTex ("Texture One", 2D) = "white" {}
       	_FadeTex ("Texture Two", 2D) = "white" {}
		_GlowTex ("Glow Texture", 2D) = "white" {}
	   	_InkTex ("Ink Texture", 2D) = "white" {}
	   	_Stencil("StencilNum", int) = 6

		_BlackLevel0("Black Level", Range(0, 1.1)) = 0.27
		_GlowBlackLevel("Glow Black Level", Range(0, 1)) = 0.27
        _Contrast("Contrast", float) = 60
		_Speed("Speed", float) = 6

		_Alpha("Alpha", Range(0, 1)) = 1
   }
   SubShader 
   {
	    Stencil{
			Ref 1
			Comp[_Stencil]
   		}

		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

       	Pass {
            Cull Front
			CGPROGRAM

			#pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram alpha

			#include "UnityCG.cginc"

			sampler2D _MainTex, _FadeTex, _InkTex, _GlowTex;
			float4 _MainTex_ST;
			//fixed _AlphaMultiplyer, _FadeAway;

			float _BlackLevel0, _GlowBlackLevel, _Contrast;
			half _Speed;
			fixed _Alpha;


			struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				//float2 uvSplat : TEXCOORD1;
			};

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//i.uvSplat = v.uv;
				return i;
			}

			float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				float2 uv = i.uv;

				float noise = tex2D(_InkTex, 0.3f * uv + _Time.x * float2(0.2f, 0.1f) *_Speed).r
                    * tex2D(_InkTex, uv + _Time.x *_Speed * float2(-0.3f, -0.1f) + float2(0.2f, 0.3f)).r;

				
				float v = tex2D(_InkTex,uv);

				float4 alpha = saturate((v - _BlackLevel0) * _Contrast);

				float4 glowAlpha = saturate((v - _BlackLevel0 + _GlowBlackLevel) * _Contrast);




                uv.x = 1 - uv.x;

				//fixed alpha = tex2D(_MainTex, uv).a * _FadeAway;

				// return
				// 	(tex2D(_MainTex, uv)* (0 + _AlphaMultiplyer)+
				// 	tex2D(_FadeTex, uv) * (1 - _AlphaMultiplyer)) * alpha;
				fixed4 col = 	((tex2D(_MainTex, uv) * alpha.r) + 
								(tex2D(_GlowTex, uv) * (glowAlpha.r-alpha.r)) + 
								(tex2D(_FadeTex, uv) * (1- glowAlpha.r))) * _Alpha;

				return col;
			}

			ENDCG
		}
	}
}