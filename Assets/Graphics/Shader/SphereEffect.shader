Shader "Mlk/SphereEffect" {
	Properties
	{
		[Header(Base)]
		[HideInInspector] _MainTex("Main Texture", 2D) = "white" {}
		[HideInInspector]_Radius("Sphere Radius",Float) = 1
		_Color("Color",Color) = (1,1,1,1)
		_Tex("Texture",2D) = "white"{}

		[Header(Effect)]
		_Thickness("Ground Halo Thickness",Float) = 0
		_EffectWidth("Effect Width",Range(0,1)) = 0.2
		_EffectStrength("Effect Strength",Range(1,5)) = 2
		_EffectSpeed("EffectSpeed",Range(1,10)) = 1

		[Header(Dissolve)]
		[HideInInspector]_DissolveValue("Current Dissolve", Range(0,1)) = 0
		_DissolveTex("Dissolve Texture",2D) = "white"{}
		_Tint("Dissolve Tint",Color) = (1,1,1,1)
	}

		SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		//Blend OneMinusDstColor SrcAlpha
		Cull Off
		Zwrite Off

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

		//Base
		sampler2D _MainTex;
		float4 _MainTex_ST;
		float4 _Color;
		sampler2D _Tex;
		float4 _Tex_ST;
		float _Radius;

		//Effect
		float _Thickness;
		float _EffectWidth;
		float _EffectStrength;
		float _EffectSpeed;

		//Dissolve
		sampler2D _DissolveTex;
		float4 _DissolveTex_ST;
		float _DissolveValue;
		float4 _Tint;
		

		float map(float s, float a1, float a2, float b1, float b2)
		{
			return b1 + (s - a1)*(b2 - b1) / (a2 - a1);
		}

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			if (i.worldPos.y < 0)
			clip(-1);
			// Texture
			float2 newUV = TRANSFORM_TEX(i.uv,_Tex);
			fixed4 col = tex2D(_Tex, newUV)*_Color;

			// Dissolve Stuff
			newUV = TRANSFORM_TEX(i.uv, _DissolveTex);
			float factor = (saturate(_DissolveValue * (1 + tex2D(_DissolveTex, newUV).r)));
			col = lerp(col, _Tint, factor);
			clip(0.9f - factor);

			float dist = distance(i.worldPos.y, 0);
			float distToRad = distance(i.worldPos.y, _Radius);

			// Ground Halo
			if (dist < _Thickness) {
				factor = map(dist, 0, _Thickness, 0, 1);
				col.a = lerp(1, col.a, factor);
			}

			// Shield Effect
			float t = sin(fmod(_Time.w*_EffectSpeed / 20,1.5708));
			float value = map(t, 0, 1, -_EffectWidth, 1 + _EffectWidth);
			float minEffectBound = _Radius * (value - _EffectWidth);
			float maxEffectBound = _Radius * (value + _EffectWidth);

			if (distToRad > minEffectBound  && distToRad < maxEffectBound) {
				factor = map(distToRad, minEffectBound, maxEffectBound, 0, 3.14159265);
				col.a *= lerp(1, _EffectStrength, sin(factor));
			}

			col.a = saturate(col.a);
			return col;
		}
		ENDCG
	}
	}
}
