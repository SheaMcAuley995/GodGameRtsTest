Shader "Unlit/BasicSpecular"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,0,0,1)
		_Ambient("Ambient", Range(0,1)) = 0.25
		_SpecColor("Specular Color", Color) = (1,1,1,1)
		_Shininess("Shininess", float) = 10
	}
	SubShader
	{

		LOD 100

		Pass
		{		
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				
				float4 vertexClip : SV_POSITION;
				float4 vertexWorld : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _Color;
			float _Ambient;
			float _Shininess;
			

			v2f vert (appdata v)
			{
				v2f o;
				o.vertexClip = UnityObjectToClipPos(v.vertex);
				o.vertexWorld = mul(unity_ObjectToWorld, v.vertex);
				
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldNormal = worldNormal;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 normalDirection = normalize(i.worldNormal);
				float3 viewDirection = normalize(UnityWorldSpaceViewDir(i.vertexWorld));
				float3 lightDirection = normalize(UnityWorldSpaceLightDir(i.vertexWorld));
				//Directional light
				float nl = max(_Ambient, dot(normalDirection, lightDirection));
				float4 diffuseTerm = nl * _Color * _LightColor0;
				//Specular
				float3 reflectionDirection = reflect(-lightDirection, normalDirection);
				float3 specularDot = max(0.0, dot(viewDirection, reflectionDirection));
				float3 specular = pow(specularDot, _Shininess);
				float4 specularTerm = float4(specular, 1) * _SpecColor * _LightColor0;

				//final Color
				float4 finalColor = diffuseTerm + specularTerm;
				return finalColor;

			}
			ENDCG
		}
		pass
		{
				tags{"LightMode" = "ForwardAdd"}
				Blend One One
		};
	}
}
