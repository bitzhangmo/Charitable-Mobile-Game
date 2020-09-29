using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Bloom : PostEffectsBase
{
 
    public Shader BloomShader;
    private Material _bloomMaterial = null;
 
    public Material Material
    {
        get
        {
            _bloomMaterial = CheckShaderAndCreateMaterial(BloomShader, _bloomMaterial);
            return _bloomMaterial;
        }
    }
 
    //Bloom是建立在高斯模糊基础上的，所以参数基本与高斯模糊一样
    //模糊迭代次数
    [Range(0,4)] public int Iterations = 3;
    //模糊范围
    [Range(0.2f, 3.0f)] public float BlurSperad = 0.6f;
    //缩放系数
    [Range(1, 8)] public int DownSample = 2;
    //亮度阈值，控制提取较亮区域时使用的阈值大小
    //一般情况下图像的亮度值不会超过1，但是如果开启了HDR，硬件会允许颜色值被存储在一个更高精度范围的缓冲中，此时可能会超过1，所以范围定在0-4
    [Range(0.0f, 4.0f)] public float LuminanceThreshold = 0.6f;
 
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Material != null)
        {
            Material.SetFloat("_LuminanceThreshold", LuminanceThreshold);
 
            int rtW = src.width / DownSample;
            int rtH = src.height / DownSample;
 
            RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
            buffer0.filterMode = FilterMode.Bilinear;
 
            //使用Shader中的第一个Pass提取图像中较亮的区域
            Graphics.Blit(src, buffer0, Material, 0);
 
            //高斯模糊迭代处理
            for (int i = 0; i < Iterations; i++)
            {
                Material.SetFloat("_BlurSize", 1.0f + i * BlurSperad);
                RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                //使用第二个Pass,渲染竖直滤波
                Graphics.Blit(buffer0, buffer1,Material,1);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
                buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
                //使用第三个Pass，渲染水平滤波
                Graphics.Blit(buffer0, buffer1, Material, 2);
                RenderTexture.ReleaseTemporary(buffer0);
                buffer0 = buffer1;
            }
 
            //把材质传递给材质中的Bloom纹理属性
            Material.SetTexture("_Bloom", buffer0);
            //使用第四个Pass，将原图与材质进行混合,并输出最终结果
            Graphics.Blit(src, dest, Material, 3);
            RenderTexture.ReleaseTemporary(buffer0);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
 
}