// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DepthRingPass" {

Properties {
   _MainTex ("", 2D) = "white" {} //this texture will have the rendered image before post-processing
   _ExtraTex("", 2D) = "white" {} //texture that is mixed with depthmask
   _RingWidth("ring width", Float) = 0.01
   _RingPassTimeLength("ring pass time", Float) = 2.0
   _Percent("percentage of depthMask", Range(0, 1)) = 0.999
}

SubShader {
Tags { "RenderType"="Opaque" }
Pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

sampler2D _CameraDepthTexture;
float _StartingTime, _Percent;
uniform float _RingPassTimeLength; //the length of time it takes the ring to traverse all depth values
uniform float _RingWidth; //width of the ring
float _RunRingPass = 0; //use this as a boolean value, to trigger the ring pass. It is called from the script attached to the camera.

struct v2f {
   float4 pos : SV_POSITION;
   float4 scrPos:TEXCOORD1;
};

//Our Vertex Shader
v2f vert (appdata_base v){
   v2f o;
   o.pos = UnityObjectToClipPos (v.vertex);
   o.scrPos=ComputeScreenPos(o.pos);
//    o.scrPos.y = 1 - o.scrPos.y;
   return o;
}

sampler2D _MainTex, _ExtraTex; //Reference in Pass is necessary to let us use this variable in shaders

//Our Fragment Shader
half4 frag (v2f i) : COLOR{

   //extract the value of depth for each screen position from _CameraDepthExture
   float depth = (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r) * _Percent;
   float dot = (tex2Dproj(_ExtraTex, i.scrPos).r) * (1-_Percent);

    //float depthValue = Linear01Depth ((tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r + (tex2Dproj(_ExtraTex, i.scrPos)).r)/2);
    // float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
    //float depthValue = 1 - Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);

    float depthValue = Linear01Depth(depth + dot);

   fixed4 orgColor = tex2Dproj(_MainTex, i.scrPos); //Get the orginal rendered color
   float4 invisColor;
   float4 newColor; //the color after the ring has passed
//    fixed4 newColor = tex2Dproj(_MainTex, i.scrPos);
   half4 lightRing; //the ring of light that will pass through the dpeth

   float t = 1 - ((_Time.y - _StartingTime)/_RingPassTimeLength );

   //the script attached to the camera will set _RunRingPass to 1 and then will start the ring pass
   if (_RunRingPass == 1){
      //this part draws the light ring
      if (depthValue < t && depthValue > t - _RingWidth){
         lightRing.r = 1;
         lightRing.g = 0;
         lightRing.b = 0;
         lightRing.a = 1;
         return lightRing;
      } else {
          if (depthValue < t) {
             //this part the ring hasn't pass through yet
            invisColor.r = orgColor.r;
            invisColor.g = orgColor.g;
            invisColor.b = orgColor.b;
            invisColor.a = 0.1;
            return invisColor;
            // newColor = orgColor;
            // newColor.a = 0;
            // return newColor;
          } else {
             //this part the ring has passed through
             //basically taking the original colors and adding a slight red tint to it.
             newColor.r = orgColor.r;
             newColor.g = orgColor.g;
             newColor.b = orgColor.b;
             newColor.a = 1;
             return newColor;
         }
      }
    } else {
        return orgColor;
    }
}
ENDCG
}
}
FallBack "Diffuse"
}