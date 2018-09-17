Shader "Custom/SimpleWater" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_SpecColor("Specular Material Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Float) = 10
		_ShoreTex ("Shore Texture", 2D) = "blue"{}
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Detail ("Detail", 2D) = "grey"{}
		_WaveSpeed("Wave Speed", float) = 30
		_WaveAmp("Wave Amplitude", float) = 1
		_NoiseTex("Noise Texture", 2D) = "white"{}
	}
	SubShader {
		Tags {"QUEUE" = "Transparent" "RenderType"="Opaque" }
		LOD 500

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NoiseTex;
		sampler2D _NormalMap;
		sampler2D _Detail;
		float _WaveAmp;
		float _WaveSpeed;


		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void vert(inout appdata_full v)
		{
			float noiseSample = tex2Dlod(_NoiseTex, float4(v.texcoord.xy, 0, 0));
			v.vertex.y += sin((_Time * _WaveSpeed) * noiseSample) * _WaveAmp;
			v.normal.y += sin((_Time * _WaveSpeed) * noiseSample) * _WaveAmp;
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= float2(1, 1);

			float3 normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex) * _Time.y);
			o.Normal = normal;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Albedo *= tex2D(_Detail, screenUV).rgb * 1;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
