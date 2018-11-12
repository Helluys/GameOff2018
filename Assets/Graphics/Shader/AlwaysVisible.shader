Shader "ShaderDeQualitay/AlwaysVisible" {
	Properties {
		[Header(Unity default shader stuff)]
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		[Header(Always Visible stuff)]
		[IntRange]_Clip ("Pixel cliping",Range(0,10)) = 3
		_SeeThroughColor ("See through color",Color) = (1,1,1,1)
	}

	SubShader {

			Pass{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		ZTest Greater
			//Pass 1

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};
				int _Clip;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _SeeThroughColor;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					clip(1 - (i.vertex.x % _Clip));
				clip(1 - (i.vertex.y % _Clip));
				fixed4 col = _SeeThroughColor;
				return col;
				}
				ENDCG
					}
		

		ZTest Less
		
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			struct Input {
				float2 uv_MainTex;
			};

			sampler2D _MainTex;
			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
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
