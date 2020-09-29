Shader "Unity Shaders Book/Chapter 12/Bloom"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	    //高斯模糊的较亮区域
	    _Bloom ("Bloom (RGB)", 2D) = "black"{}
		//提取较亮区域的阈值
		_LuminanceThreshold("Luminance Threshold", Float) = 0.5
		//控制不同迭代之间高斯模糊的模糊区域范围
		_BlurSize("Blur Size", Float) = 1.0
 
	}
	SubShader
	{
		CGINCLUDE
		#include "UnityCG.cginc"
 
		sampler2D _MainTex;
		half4 _MainTex_TexelSize;
		sampler2D _Bloom;
		float _LuminanceThreshold;
		float _BlurSize;
 
		//定义提取较亮区域需要使用的顶点着色器和片元着色器
		struct v2f {
			float4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
		};
 
		//顶点着色器与之前相同
		v2f vertExtractBright(appdata_img v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;
			return o;
		}
 
		//亮度值采样
		fixed luminance(fixed4 color) {
			return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
		}
 
		//片元着色器，得到亮部区域
		fixed4 fragExtractBright(v2f i) : SV_Target
		{
			fixed4 c = tex2D(_MainTex, i.uv);
			//将亮度值减去阈值并截取到0-1范围
			fixed val = clamp(luminance(c) - _LuminanceThreshold, 0.0, 1.0);
			//将该值与原像素值相乘，得到亮部区域
			return c * val;
		}
 
		//定义混合亮部区域与原图像时使用的顶点着色器和片元着色器
		struct v2fBloom {
			float4 pos : SV_POSITION;
			//使用half4同时存储_MainTex与_Bloom的坐标
			half4 uv : TEXCOORD0;
		};
 
		v2fBloom vertBloom(appdata_img v) {
			v2fBloom o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv.xy = v.texcoord;
			o.uv.zw = v.texcoord;
			
			//平台差异化处理
			//判断是否时DirectX平台(uv从顶部开始)
			#if UNITY_UV_STARTS_AT_TOP
			if (_MainTex_TexelSize.y < 0.0)
				o.uv.w = 1.0 - o.uv.w;
			#endif
 
			return o;
		}
 
		fixed4 fragBloom(v2fBloom i) : SV_Target
		{
			//把两张纹理的采样结果相加混合
			return tex2D(_MainTex, i.uv.xy) + tex2D(_Bloom,i.uv.zw);
		}
			
		ENDCG
 
		//定义Bloom效果需要的4个Pass
		
		ZTest Always Cull Off Zwrite Off
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vertExtractBright
			#pragma fragment fragExtractBright		
			ENDCG
		}
 
		//使用在高斯模糊中定义好的两个Pass
		UsePass"Gaussian Blur/GAUSSIAN_BLUR_VERTICAL"
 
		UsePass"Gaussian Blur/GAUSSIAN_BLUR_HORIZONTAL"
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vertBloom
			#pragma fragment fragBloom	
			ENDCG
		}
	}
		Fallback Off
}