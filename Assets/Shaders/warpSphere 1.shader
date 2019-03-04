Shader "frames/warpSphere_01"
{
   Properties
   {
       	_MainTex ("Texture One", 2D) = "white" {}
       	_FadeTex ("Texture Two", 2D) = "white" {}
		_GlowTex ("Glow Texture", 2D) = "white" {}
	   	_InkTex ("Ink Texture", 2D) = "white" {}
	   	_Stencil("StencilNum", int) = 6

		_BlackLevel("Black Level", Range(0, 1.5)) = 0.27
		_GlowBlackLevel("Glow Black Level", Range(0, 1)) = 0.27
        _Contrast("Contrast", float) = 60
		_Speed("Speed", float) = 6

		_Alpha("Alpha", Range(0, 1.5)) = 1

		_GlitchInterval ("Glitch interval time [seconds]", Float) = 0.16
		_DispProbability ("Displacement Glitch Probability", Float) = 0.022
		_DispIntensity ("Displacement Glitch Intensity", Float) = 0.09
		_ColorProbability("Color Glitch Probability", Float) = 0.02
		_ColorIntensity("Color Glitch Intensity", Float) = 0.07

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
						#pragma target 3.0
			#pragma multi_compile DUMMY PIXELSNAP_ON

			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _MainTex, _FadeTex, _InkTex, _GlowTex;
			float4 _MainTex_ST, _FadeTex_ST, _InkTex_ST, _GlowTex_ST;

			float _BlackLevel, _GlowBlackLevel, _Contrast;
			half _Speed;
			fixed _Alpha;

			float _GlitchInterval;
			float _DispIntensity;
			float _DispProbability;
			float _ColorIntensity;
			float _ColorProbability;
			float _WrapDispCoords;
			float _sphere;

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

				//This ensures that the shader only generates new random variables every [_GlitchInterval] seconds, e.g. every 0.5 seconds
				//During each interval the value wether the glitch occurs and how much the sprites glitches stays the same
				// float intervalTime = floor(_Time.y / _GlitchInterval) * _GlitchInterval;

				// //Second value increased by arbitrary number just to get more possible different random values
				// float intervalTime2 = intervalTime + 2.793;

				// //These values depend on time and the x/y translation of that sprite (top right and middle right value in the transformation matrix are translation)
				// //The transformation matrix values are included so sprites with differen x/y values don't glitch at the same time

				// float timePositionVal = intervalTime;
				// float timePositionVal2 = intervalTime2;

				// //Random chance that the displacement glich or color glitch occur
				// float dispGlitchRandom = rand(timePositionVal, -timePositionVal);
				// float colorGlitchRandom = rand(timePositionVal, timePositionVal);

				// //Precalculate color channel shift
				// float rShiftRandom = (rand(-timePositionVal, timePositionVal) - 0.5) * _ColorIntensity;
				// float gShiftRandom = (rand(-timePositionVal, -timePositionVal) - 0.5) * _ColorIntensity;
				// float bShiftRandom = (rand(-timePositionVal2, -timePositionVal2) - 0.5) * _ColorIntensity;

				// //For the displacement glitch, the sprite is divided into strips of 0.2 * sprite height (5 stripes)
				// //This value is the random offset each of the strip boundries get either up or down
				// //Without this, each strip would be exactly a 5th of the sprite height, with this their height is slightly randomised
				// float shiftLineOffset = float((rand(timePositionVal2, timePositionVal2) - 0.5) / 50);

				// //If the randomly rolled value is below the probability boundry and the displacement effect is turned on, apply the displacement effect
				// if(dispGlitchRandom < _DispProbability){
				// 	uv.x += (rand(floor(uv.y / (0.05 + shiftLineOffset)) - timePositionVal, floor(uv.y / (0.05 + shiftLineOffset)) + timePositionVal) - 0.5) * _DispIntensity;
				// 	//Prevent the texture coordinate from going into other parts of the texture, especially when using texture atlases
				// 	//Instead, loop the coordinate between 0 and 1
				// 	if(_WrapDispCoords == 1){
				// 		uv.x = fmod(uv.x, 1);
				// 	}
				// 	else{
				// 		uv.x = clamp(uv.x, 0, 1);
				// 	}
				// }

				//Sample the texture at the normal position and at the shifted color channel positions
				// fixed4 normalC = tex2D(_MainTex, uv);
				// fixed4 rShifted = tex2D(_MainTex, float2(uv.x + rShiftRandom, uv.y + rShiftRandom));
				// fixed4 gShifted = tex2D(_MainTex, float2(uv.x + gShiftRandom, uv.y + gShiftRandom));
				// fixed4 bShifted = tex2D(_MainTex, float2(uv.x + bShiftRandom, uv.y + bShiftRandom));

				// fixed4 normalD = tex2D(_FadeTex, uv);
				// fixed4 rShifted2 = tex2D(_FadeTex, float2(uv.x + rShiftRandom, uv.y + rShiftRandom));
				// fixed4 gShifted2 = tex2D(_FadeTex, float2(uv.x + gShiftRandom, uv.y + gShiftRandom));
				// fixed4 bShifted2 = tex2D(_FadeTex, float2(uv.x + bShiftRandom, uv.y + bShiftRandom));
				
				// fixed4 c = fixed4(0.0,0.0,0.0,0.0);

				// fixed4 d = fixed4(0.0,0.0,0.0,0.0);

				//If the randomly rolled value is below the probability boundry and the color effect is turned on, apply the color glitch effect
				//Sets the output color to the shifted r,g,b channels and averages their alpha
				// if(colorGlitchRandom < _ColorProbability){
				// 	c.r = rShifted.r;
				// 	c.g = gShifted.g;
				// 	c.b = bShifted.b;
				// 	c.a = (rShifted.a + gShifted.a + bShifted.a) / 3;

				// 	d.r = rShifted2.r;
				// 	d.g = gShifted2.g;
				// 	d.b = bShifted2.b;
				// 	d.a = (rShifted2.a + gShifted2.a + bShifted2.a) / 3;
				// } else {
				// 	c = normalC;
				// 	d = normalD;
				// }

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