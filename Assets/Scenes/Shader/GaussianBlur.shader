﻿Shader "Gaussian Blur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlurSize("Blur Size", Float) = 1.0
		_FocusBox("Focus Box", Vector) = (0,0,0,0)
	}
	SubShader
	{
		CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half4 _MainTex_TexelSize;	// xxx_TexelSize 是 xxx 纹理对应的每个纹素的大小
		float _BlurSize;
		float4 _FocusBox;

		struct v2f
		{
			float4 pos : SV_POSITION;
			half2 uv[5] : TEXCOORD0;
		};

		// appdata_img 为 Unity 内置的结构体，在 UnityCG.cginc 中定义
		v2f vertBlurVertical(appdata_img v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			half2 uv = v.texcoord;
			o.uv[0] = uv;
			o.uv[1] = uv + float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
			o.uv[2] = uv - float2(0.0, _MainTex_TexelSize.y * 1.0) * _BlurSize;
			o.uv[3] = uv + float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;
			o.uv[4] = uv - float2(0.0, _MainTex_TexelSize.y * 2.0) * _BlurSize;

			return o;
		}

		v2f vertBlurHorizontal(appdata_img v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			half2 uv = v.texcoord;
			o.uv[0] = uv;
			o.uv[1] = uv + float2(_MainTex_TexelSize.x * 1.0, 0.0) * _BlurSize;
			o.uv[2] = uv - float2(_MainTex_TexelSize.x * 1.0, 0.0) * _BlurSize;
			o.uv[3] = uv + float2(_MainTex_TexelSize.x * 2.0, 0.0) * _BlurSize;
			o.uv[4] = uv - float2(_MainTex_TexelSize.x * 2.0, 0.0) * _BlurSize;

			return o;
		}

		fixed4 fragBlur(v2f i) : SV_Target
		{
			// 高斯权重
			float weight[3] = {0.4026, 0.2442, 0.0545};

			fixed3 sum = tex2D(_MainTex, i.uv[0]).rgb * weight[0];

			for (int it = 1; it < 3; it++)
			{
				sum += tex2D(_MainTex, i.uv[it * 2 - 1]).rgb * weight[it];
				sum += tex2D(_MainTex, i.uv[it * 2]).rgb * weight[it];
			}

			if (i.uv[0].x > _FocusBox.x && i.uv[0].x < _FocusBox.z && i.uv[0].y > _FocusBox.y && i.uv[0].y < _FocusBox.w)
				return fixed4(tex2D(_MainTex, i.uv[0]).rgb, 1);

			return fixed4(sum, 1.0);
		}

		ENDCG

		ZTest Always Cull Off ZWrite Off

		Pass 
		{
			NAME "GAUSSIAN_BLUR_VERTICAL"

			CGPROGRAM

			#pragma vertex vertBlurVertical
			#pragma fragment fragBlur

			ENDCG
		}

		Pass
		{
			NAME "GAUSSIAN_BLUR_HORIZONTAL"

			CGPROGRAM

			#pragma vertex vertBlurHorizontal
			#pragma fragment fragBlur

			ENDCG
		}
	}
	Fallback Off
}