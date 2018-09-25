Shader "Custom/SimpleWater" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
		_NoiseTex("Noise Tex", 2d) = "white" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_WaveSpeed("wave speed", float) = 30
		_WaveAmp("wave Amp", float) = 1
		_ScrollSpeed("Scroll speed", Range(0,1)) = 0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _NoiseTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _WaveSpeed;
		float _WaveAmp;
		float _ScrollSpeed;

		void vert(inout appdata_full v)
		{
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

			float noiseSample = tex2Dlod(_NoiseTex, float4(v.texcoord.xy,0,0));
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
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex + (_Time * _ScrollSpeed) / 100) * _Color;
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
