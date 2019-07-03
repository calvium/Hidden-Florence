// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "custom/ScannerEffect 01"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NonTex("Texture", 2D) = "white" {}
		_DetailTex("Noise Texture", 2D) = "white" {}
		_LineTex("Line Texture", 2D) = "white" {}
		_FadeTex("Texture", 2D) = "white" {}
		_AltTex("Texture", 2D) = "white" {}
		_ScanDistance("Scan Distance", float) = 0
		_StartFade("Start Fade", float) = 2
		_ScanWidth("Scan Width", float) = 10
		_EndFade("End Fade", float) = 2
		_LeadSharp("Leading Edge Sharpness", float) = 10
		_WarpScale ("Warp Scale", Range(0, 1)) = 0
		_rev("Reverse", Int) = 0
		_BlackLevel("Black Level", Range(0, 1.1)) = 0.27
        _Contrast("Contrast", float) = 60
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag alpha
			
			#include "UnityCG.cginc"

			struct VertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			struct VertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_depth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;
			};

			float4 _MainTex_TexelSize;
			float4 _CameraWS;
			float _WarpScale;

			VertOut vert(VertIn v)
			{
				VertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				o.uv_depth = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif				

				o.interpolatedRay = v.ray;

				return o;
			}

			sampler2D _MainTex, _NonTex, _FadeTex, _AltTex;
			sampler2D _DetailTex, _LineTex;
			sampler2D_float _CameraDepthTexture;
			float4 _WorldSpaceScannerPos;
			float _ScanDistance, _ScanWidth, _StartFade, _EndFade;
			float _LeadSharp, _rev, _BlackLevel, _Contrast;


			half4 frag (VertOut i) : SV_Target
			{
				float4 detail = tex2D(_DetailTex, i.uv) + _Time;

				float4 n = tex2D(_FadeTex, i.uv);

				float rawDepth = DecodeFloatRG(	 tex2D(_CameraDepthTexture, i.uv_depth)	);
				float linearDepth = Linear01Depth(rawDepth);
				float4 wsDir = ((linearDepth*0.95) + (n*0.05)) * i.interpolatedRay;
				float3 wsPos = _WorldSpaceCameraPos + wsDir;

				float dist = distance(wsPos, _WorldSpaceScannerPos);

				float4 beforeCol;
				float4 afterCol;


				const float PI = 3.14159;
				float2 pos = i.uv;
				pos.x = sin(pos.y * detail * PI * 2) * _WarpScale;
				

				half4 col = tex2D(_MainTex, i.uv);
				half4 nonCol = tex2D(_NonTex, i.uv);
				float4 linetex = tex2D(_LineTex, pos); 

				float4 noiseTex = saturate(tex2D(_DetailTex, i.uv));
				float4 glowTex = tex2D(_LineTex, i.uv);

				// float4 altFade = tex2D(_AltTex, i.uv);

				if(_rev == 0){

					if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1) {
						return glowTex;
					} else if (dist < _ScanDistance && dist > _ScanDistance - _EndFade){
						float diff = 1 - (_ScanDistance - _EndFade - dist) / (_ScanWidth-_EndFade);
						return lerp((glowTex*noiseTex + col*(1-noiseTex)), col, pow(diff, _LeadSharp));
				
					} else if (dist > _ScanDistance && dist < _ScanDistance + _StartFade){
						float diff = 1 - (_ScanDistance + _StartFade - dist) / (_ScanWidth+_StartFade);
						return lerp(glowTex, nonCol, pow(diff, _LeadSharp));
					} else if (dist > _ScanDistance){
						return nonCol;
					} else {
						return col;
					}
				} else {
					float v = tex2D(_AltTex, i.uv);

					float4 alpha = saturate((v - _ScanDistance/3) * _Contrast);

					float4 glowAlpha = saturate((v - _ScanDistance/3 + _ScanWidth) * _Contrast);
					
					fixed4 q = 	(nonCol * alpha.r) + 
								(glowTex * (glowAlpha.r-alpha.r)) + 
								(col * (1- glowAlpha.r));

					return q;
				}
			}
			ENDCG
		}
	}
}
