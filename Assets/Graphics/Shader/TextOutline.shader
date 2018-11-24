Shader "Mlk/TextOutline"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_Tint("Tint",Color) = (1,1,1,1)
		_ShadowColor("ShadowColor",Color) = (1,1,1,1)
		_ShadowOffset("ShadowOffset",Vector) = (0,0,0,0)
	}
		SubShader
	{
		CGINCLUDE
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	fixed4 _Tint;
	float4 _ShadowOffset;
	fixed4 _ShadowColor;
	ENDCG
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
		v2f vert(appdata v)
	{
		v2f o;
		float4 newPos;
		newPos = v.vertex + _ShadowOffset;
		o.vertex = UnityObjectToClipPos(newPos);
		o.uv = v.uv;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{

		fixed4 col = tex2D(_MainTex, i.uv);
	col.rgb = _ShadowColor.rgb;
	return col;
	}
		ENDCG
	}

		Pass
	{
		CGPROGRAM
		v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{

		fixed4 col = tex2D(_MainTex, i.uv);
	col.rgb = _Tint.rgb;
	return col;
	}
		ENDCG
	}
	}
}
