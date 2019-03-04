// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "custom/ScannerEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NonTex("Texture", 2D) = "white" {}
		_DetailTex("Noise Texture", 2D) = "white" {}
		_LineTex("Line Texture", 2D) = "white" {}
		_ScanDistance("Scan Distance", float) = 0
		_StartFade("Start Fade", float) = 2
		_ScanWidth("Scan Width", float) = 10
		_EndFade("End Fade", float) = 2
		_LeadSharp("Leading Edge Sharpness", float) = 10
		_LeadColor("Leading Edge Color", Color) = (1, 1, 1, 0)
		_MidColor("Mid Color", Color) = (1, 1, 1, 0)
		_TrailColor("Trail Color", Color) = (1, 1, 1, 0)
		_HBarColor("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)

		_WarpScale ("Warp Scale", Range(0, 1)) = 0

		_GlowColor1("Glow Colour", Color) = (0,0,0,0)
		_GlowColor2("Glow Colour", Color) = (0,0,0,0)

		_GlowTex("Glow Texture", 2D) = "white" {}



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

			sampler2D _MainTex, _NonTex, _GlowTex;
			sampler2D _DetailTex, _LineTex;
			sampler2D_float _CameraDepthTexture;
			float4 _WorldSpaceScannerPos;
			float _ScanDistance;
			float _ScanWidth, _StartFade, _EndFade;
			float _LeadSharp;
			float4 _LeadColor;
			float4 _MidColor;
			float4 _TrailColor;
			float4 _HBarColor;
			float4 _GlowColor1, _GlowColor2;

			float4 horizBars(float2 p)
			{
				// return 1 - saturate(round(abs(frac(p.y * 100) * 2)));
				return 1 - saturate(tex2D(_DetailTex, p));
			}

			float4 horizTex(float2 p)
			{
				return tex2D(_DetailTex, float2(p.x * 30, p.y * 40));
			}

			half4 frag (VertOut i) : SV_Target
			{
				
				// half4 col = tex2D(_MainTex, i.uv);
				// half4 nonCol = tex2D(_NonTex, i.uv);

				float4 detail = tex2D(_DetailTex, i.uv) + _Time;
				// float4 linetex = tex2D(_LineTex, i.uv);

				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
				float linearDepth = Linear01Depth(rawDepth);
				float4 wsDir = linearDepth * i.interpolatedRay;
				float3 wsPos = _WorldSpaceCameraPos + wsDir;
				half4 scannerCol = half4(0, 0, 0, 0);

				float dist = distance(wsPos, _WorldSpaceScannerPos);

				// float2 uvn = TRANSFORM_TEX(_DetailTex.xy + _Time);
				float4 beforeCol;
				float4 afterCol;


				const float PI = 3.14159;
				float2 pos = i.uv;
				pos.x = sin(pos.y * detail * PI * 2) * _WarpScale;
				

				half4 col = tex2D(_MainTex, i.uv);
				half4 nonCol = tex2D(_NonTex, i.uv);
				float4 linetex = tex2D(_LineTex, pos); 

				// float4 noiseTex = saturate(tex2D(_DetailTex, i.uv));

				// float4 glowTex = tex2D(_GlowTex, i.uv) * (_GlowColor1 * noiseTex.r) + (_GlowColor2 * (1-noiseTex.r));
				float4 glowTex = tex2D(_GlowTex, i.uv);

				float4 invis = (0,0,0,0);

				// half4 n = tex2D(_DetailTex, pos);

				// float4 endFade = (_GlowColor1 * noiseTex.r) + (_GlowColor2 * (1-noiseTex.r));
				
				// float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);

				if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1)
				{
					float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);
					half4 edge = lerp(_MidColor, invis, pow(diff, _LeadSharp));
					scannerCol = lerp(_TrailColor, edge, diff) + horizBars(i.uv) * _HBarColor;
					scannerCol *= diff;
					return glowTex;
				
				} else if (dist < _ScanDistance && dist > _ScanDistance - _EndFade){
					float diff = 1 - (_ScanDistance - _EndFade - dist) / (_ScanWidth-_EndFade);
					return lerp(glowTex, col, pow(diff, _LeadSharp));
				
				} else if (dist > _ScanDistance && dist < _ScanDistance + _StartFade){
					float diff = 1 - (_ScanDistance + _StartFade - dist) / (_ScanWidth+_StartFade);
					return lerp(glowTex, nonCol, pow(diff, _LeadSharp));

				} else if (dist > _ScanDistance){
					// beforeCol.r = (col.r+0.3)*0.5;
					// beforeCol.g = col.g * 0.5;
					// beforeCol.b = col.b * 0.5;
					// beforeCol.a = 0.1;
					// return beforeCol;
					return nonCol;
				} else {
					// afterCol.r = (col.r+0.3)*0.5;
					// afterCol.g = col.g;
					// afterCol.b = col.b;
					// afterCol.a = 1;
					// return afterCol;
					return col;
				}

				// return beforeCol + scannerCol + afterCol;
			}
			ENDCG
		}
	}
}
