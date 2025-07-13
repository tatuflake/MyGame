Shader "Custom/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,1,1)
        _OutlineThickness ("Outline Thickness", Range(0.0, 0.03)) = 0.003
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Name "BASE"
            Tags { "LightMode" = "ForwardBase" }
            Cull Front
            ZWrite On
            ZTest LEqual
            ColorMask RGB
            SetTexture[_MainTex] {combine texture}
        }

        Pass
        {
            Name "OUTLINE"
            Tags { "LightMode" = "Always" }
            Cull Front
            ZWrite On
            ZTest LEqual
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha
            SetTexture[_MainTex] {combine texture}
            Offset 0, -1

            // 書き込み: アウトラインの色
            Color[_OutlineColor]
            Offset[ _OutlineThickness, _OutlineThickness ]
            SetTexture[_MainTex] {combine primary}
        }
    }
    Fallback "Diffuse"
}
