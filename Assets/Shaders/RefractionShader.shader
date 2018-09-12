// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RefractionShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_RefractionAmount ("Refraction Amount", float) = 0.1
	}
	SubShader {
		Tags { "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		LOD 200
	
		GrabPass{"_GrabTexture"}

		ZWrite off

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _GrabTexture;

		struct Input {
			float4 grabUV;
			float4 refract;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _RefractionAmount;

		void vert(inout appdata_full i, out Input o)
		{
			float4 pos = UnityObjectToClipPos(i.vertex);
			o.grabUV = ComputeGrabScreenPos(pos);
			o.refract = float4(i.normal,0) * _RefractionAmount;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			//float4 objPos = mul(unity_WorldToObject, float4(IN.worldPos, 1));
			//float4 uv_GrabTexture = ComputerGrabScreenPos(objPos);
			//uv_GrabTexture.y *= -1;

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2Dproj (_GrabTexture, UNITY_PROJ_COORD(IN.grabUV + IN.refract)) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
