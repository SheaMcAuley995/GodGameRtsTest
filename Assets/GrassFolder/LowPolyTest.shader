// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'


Shader "Custom/LowPolyTest" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

				pass
			{


			CGPROGRAM

				// Physically based Standard lighting model, and enable shadows on all light types
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma geometry geom

				// Use shader model 3.0 target, to get nicer looking lighting
				#pragma target 4.0

				sampler2D _MainTex;

				struct v2g
				{
					float4 pos : SV_POSITION;
					float3 norm : NORMAL;
					float2 uv : TEXCOORD0;
				};

				struct g2f
				{
					float4 pos : SV_POSITION;
					float3 norm : NORMAL;
					//float2 uv : TEXCOORD0;
					//float3 diffuseColor : TEXCOORD1;
					//float3 specularColor : TEXCOORD2;
				};


				half _Glossiness;
				half _Metallic;
				fixed4 _Color;

				v2g vert(appdata_full v)
				{
					float3 v0 = mul(unity_ObjectToWorld, v.vertex).xyz;
					//v.vertex.xyz = mul((float3x3)unity_WorldToObject, v0);

					v2g OUT;
					OUT.pos = v.vertex;
					OUT.norm = v.normal;
					OUT.uv = v.texcoord;
					return OUT;
				}


				[maxvertexcount(3)]
				void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
				{
					float3 v0 = IN[0].pos.xyz;
					float3 v1 = IN[1].pos.xyz;
					float3 v2 = IN[2].pos.xyz;

					float3 normal = normalize(cross(v0 - v1, v1 - v2));
					float4 worldNormal = UnityObjectToClipPos(normal);

					g2f OUT;
					OUT.pos = UnityObjectToClipPos(IN[0].pos) + worldNormal * 0.1f;
					OUT.norm = normal;
					
					triStream.Append(OUT);

					OUT.pos = UnityObjectToClipPos(IN[1].pos) + worldNormal * 0.1f;
					OUT.norm = normal;
					triStream.Append(OUT);

					OUT.pos = UnityObjectToClipPos(IN[2].pos) + worldNormal * 0.1f;
					OUT.norm = normal;
					triStream.Append(OUT);


				}

				half4 frag(g2f IN) : COLOR
				{
					return float4(1.0,1.0,1.0, 1.0);
				}
				ENDCG
			}
		}
}
