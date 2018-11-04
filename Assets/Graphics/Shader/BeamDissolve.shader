Shader "ShaderDeQualitay/BeamDissolve"
{
	Properties
	{
		[HideInInspector] _MainTex ("Main Texture", 2D) = "white" {}
		_Color("Color",Color) = (1,1,1,1)
		_DissolveTex("Dissolve Texture",2D) = "white"{}
		_Tint("Dissolve Tint",Color) = (1,1,1,1)
		_DissolveValue("Current Dissolve", Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
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
				float3 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _DissolveTex;
			float4 _DissolveTex_ST;
			float _DissolveValue;
			float4 _Tint;
			float4 _Color;


			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 newUV = TRANSFORM_TEX(i.uv,_DissolveTex);
				fixed4 col = tex2D(_MainTex, i.uv)*_Color;
				float factor =  (saturate(_DissolveValue * (1+tex2D(_DissolveTex, newUV).r)));
				col = lerp(col, _Tint, factor);
				clip(0.9f - factor);

				return col;
			}
			ENDCG
		}
	}
}
