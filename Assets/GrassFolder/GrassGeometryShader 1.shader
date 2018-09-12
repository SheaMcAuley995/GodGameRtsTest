// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Custom/GrassGeometryShader" {
	Properties {
		[HDR]_BackgroundColor("Background Color", color) = (1,0,0,1)
		[HDR]_ForegroundColor("Foreground Color", color) = (0,0,1,1)
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Cutoff("Cutoff", Range(0,1)) = 0.25
		_GrassHeight("Grass Height", Float) = 0.25
		_GrassWidth("Grass Width", Float) = 0.25
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

				pass
			{

			CULL OFF

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
					float3 color : TEXCOORD1;
				};

				struct g2f
				{
					float4 pos : SV_POSITION;
					float3 norm : NORMAL;
					//float2 uv : TEXCOORD0;
					float3 diffuseColor : TEXCOORD1;
					//float3 specularColor : TEXCOORD2;
				};


				half _Glossiness;
				half _Metallic;
				fixed4 _BackgroundColor;
				fixed4 _ForegroundColor;
				half _GrassHeight;
				half _GrassWidth;
				half _Cutoff;

				v2g vert(appdata_full v)
				{
					v2g OUT;
					OUT.pos = v.vertex;
					OUT.norm = v.normal;
					OUT.uv = v.texcoord;
					OUT.color = tex2Dlod(_MainTex, v.texcoord).rgb;
					return OUT;
				}


				[maxvertexcount(4)]
				void geom(point v2g IN[1], inout TriangleStream<g2f> triStream)
				{
					float3 lightPosition = _WorldSpaceLightPos0;

					float3 perpendicularAngle = float3(1, 0, 0);
					float3 faceNormal = cross(perpendicularAngle, IN[0].norm);


					float3 v0 = IN[0].pos.xyz;
					float3 v1 = IN[0].pos.xyz + IN[0].norm * _GrassHeight;

					//float3 normal = normalize(cross(v0 - v1, v1 - v2));

					float3 color = (IN[0].color + IN[1].color + IN[2].color) / 3;

				//	float4 worldNormal = UnityObjectToClipPos(normal);

					g2f OUT;
					OUT.pos = UnityObjectToClipPos(v0 + perpendicularAngle * 0.5f * _GrassHeight);
					OUT.norm = faceNormal;
					OUT.diffuseColor = color;
					OUT.uv = float(1, 0);
					triStream.Append(OUT);

					OUT.pos = UnityObjectToClipPos(v0 - perpendicularAngle * 0.5f * _GrassHeight);
					OUT.norm = faceNormal;
					OUT.diffuseColor = color;
					OUT.uv = float(0, 0);
					triStream.Append(OUT);

					OUT.pos = UnityObjectToClipPos(v1 + perpendicularAngle * 0.5f * _GrassHeight);
					OUT.norm = faceNormal;
					OUT.diffuseColor = color;
					OUT.uv = float(1, 1);
					triStream.Append(OUT);

					OUT.pos = UnityObjectToClipPos(v1 - perpendicularAngle * 0.5f * _GrassHeight);
					OUT.norm = faceNormal;
					OUT.diffuseColor = color;
					OUT.uv = float(0, 1);
					triStream.Append(OUT);


				}

				half4 frag(g2f IN) : COLOR
				{
					fixed4 c = tex2D(_MainTex, IN.uv);
					clip(c.a - _Cutoff);
					return c;//return float4(IN.diffuseColor.rgb, 1.0);
				}
				ENDCG
			}
		}
}
