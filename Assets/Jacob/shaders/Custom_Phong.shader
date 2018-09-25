Shader "Custom/Custom_Phong" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "white" {}
		_MainTex2("Albedo2 (RBG)", 2D) = "white" {}
		_NormalMap2("Normal Map 2", 2D) = "white" {}

		_Shinyness ("Shinyness", Range(0.3,1)) = 0.5
		_SpecColor ("SpecColor", Range(0,1)) = 0.0

		_AlbedoLerp("Lerp Value", Range(0,1)) = 0.0
		_NormalLerp("Lerp value 2", range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Phong fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _NormalMap;
		sampler2D _NormalMap2;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};

		half _Shinyness;
		fixed4 _Color;
		float _AlbedoLerp;
		float _NormalLerp;

		inline void LightingPhong_GI(SurfaceOutput s, UnityGIInput data, UnityGI gi)
		{
			gi = UnityGlobalIllumination(data, 1.0, s.Normal);
		}

		inline fixed4 LightingPhong(SurfaceOutput s, half viewDir, UnityGI gi)
		{
			UnityLight light = gi.light;
			float nl = max(0.0, dot(s.Normal, light.dir));
			float3 diffuseTerm = nl * s.Albedo.rbg * light.color;
			float3 reflectionDir = reflect(-light.dir, s.Normal);
			float3 specularDot = max(0.0, dot(viewDir, reflectionDir));
			float3 specular = pow(specularDot, _Shinyness);
			float3 specularTerm = specular * _SpecColor.rbg * light.color.rbg;
			float3 finalColor = diffuseTerm.rbg + specularTerm;

			fixed4 c;
			c.rbg = finalColor;
			c.a = s.Alpha;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				c.rbg += s.Albedo + gi.indirect.diffuse;
			#endif
			return c;
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) ;
			fixed4 c2 = tex2D(_MainTex2, IN.uv_MainTex2) ;
			fixed4 col = lerp(c, c2, _AlbedoLerp) * _Color;
			o.Albedo = col.rgb;
			float3 n = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
			float3 n2 = UnpackNormal(tex2D(_NormalMap2, IN.uv_MainTex2));
			float3 nor = lerp(n, n2, _NormalLerp);
			o.Normal = nor;
			// Metallic and smoothness come from slider variables
			o.Specular = _Shinyness;
			o.Gloss = col.a;
			o.Alpha = 0.1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
