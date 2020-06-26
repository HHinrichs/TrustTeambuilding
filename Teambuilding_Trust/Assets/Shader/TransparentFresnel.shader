Shader "Avatar/TransparentFresnel" 
 {
     Properties 
     {

       _Color ("Base Color", Color) = (0.26,0.19,0.16,0.0)
       _OutlineColor("Outline Color", Color) = (1,1,1,1)
       _Transparency("Transparency",Range(0,1)) = 1.0

       _Smoothness ("Smoothness", Range(0.0,1.0)) = 0.0
       _FresnelStrength("Strength",Range(1.0,8.0)) = 1.0
       _MixColor("Mixing Value", Range(0.0,1.0)) = 0.5

       _ClipY("Clipping Y",Range(-3.0,3.0)) = 0.0
       _ClipDimension ("Clipping Dimension",Range(0,2)) = 0.5
       _ClipStrength("Clipping Width",Range(1.0,10.0)) = 5.0
       _ClippingTexture("ClippingTexture",2D) = "white" {}

      
       [Toggle] _ClipDebug("Clipping Debugging", float) = 0
     }
     SubShader 
     {
       Tags { "Queue" = "Transparent" "RenderType"="Transparent"}

       ZWrite on
       Cull Back
       //Blend SrcAlpha OneMinusSrcAlpha
       Blend SrcColor One
       
       CGPROGRAM
       #pragma surface surf Lambert
       
       struct Input 
       {
       	   float2 uv_MainTex;
           float3 viewDir;
           float3 worldPos;
       };

       
      // float4 _InnerColor;
      // float4 _RimColor;
      float4 _Color;
      float4 _OutlineColor;
      float _Smoothness;
      float _Transparency;
      float _FresnelStrength;
      float _MixColor;

      float _ClipY;
      float _ClipStrength;
      float _ClipDebug;
      float _ClipDimension;
      sampler2D _ClippingTexture;


     


       static const float smoothnessStep = 8.0;
       void surf (Input IN, inout SurfaceOutput o) 
       {
       					

       	   float fresnel = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));

           float powFresnel = pow (fresnel, (smoothnessStep + 1.0) - (_Smoothness * (smoothnessStep + 0.5)));
 
           powFresnel = saturate(powFresnel * _FresnelStrength);
           
           float3 fresnelColor = _OutlineColor * powFresnel;

          
           float3 col = saturate(lerp(_Color * _Transparency, fresnelColor,_MixColor));
           o.Emission =  col;
        
           o.Alpha = _Transparency;


            //ToDo
           float clipValue = tex2D(_ClippingTexture, IN.uv_MainTex).r;
          // o.Emission =  float3(clipValue,clipValue,clipValue);

           float3 localPos = IN.worldPos - mul(unity_ObjectToWorld,float4(0,0,0,1)).xyz;
           //float3 clipDimPos = mul(unity_ObjectToWorld,float4(_ClipDimension,0,0,01)).xyz; // ObjectCoord
           if(localPos.y < _ClipY ){ // Better way with Maps
           		if(localPos.x < _ClipDimension && localPos.x > -_ClipDimension && localPos.z < _ClipDimension && localPos.z > -_ClipDimension)
           		{
           			if(_ClipDebug == 1.0){
	           		float dist = saturate((_ClipY - localPos.y) * _ClipStrength*_ClipStrength);
	           		o.Emission =  float3(1,1.0-lerp(1.0, 0.0, dist),0);
	           		}else{
	           		float dist = saturate((_ClipY - localPos.y) * _ClipStrength*_ClipStrength);
           			o.Emission =  col * lerp(1.0, 0.0, dist);
           			o.Alpha = lerp(1.0, 0.0, dist);
	           }
           		}
           }

       }
       ENDCG
     } 
     Fallback "Diffuse"
   }