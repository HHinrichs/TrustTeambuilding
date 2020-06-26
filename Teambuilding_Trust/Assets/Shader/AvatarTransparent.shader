// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Avatar/AvatarTransparent"
{
	Properties

	{
		_Color ("Color", Color) = (1,0,0,0)
		//_MainTex ("Texture", 2D) = "white" {}
		_Transparency ("Transparency", Range(0,1)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Qeue"="Transparent"}
		LOD 100
		//ZWrite off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				half3 normal : NORMAL;

			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float rim : TEXCOORD1;
			};


			fixed4 _Color;
			float _Transparency;
			static const fixed3 lightPos = fixed3(1,0,0);
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				float3 posWorld = mul(unity_ObjectToWorld,v.vertex).xyz;
				float3 normWorld = v.normal;

				float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
				o.rim = pow(1.0 + dot(I,normWorld),5);
				return o;
			}



			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = float4(_Color.rgb, i.rim);
				return col;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
