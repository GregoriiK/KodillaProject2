Shader "Unlit/Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DisplacementTexture ("Displacement", 2D) = "bump" {}
        _DisplacementX("Displacement X", Range(0,1)) = 0
        _DisplacementY("Displacement Y", Range(0,1)) = 0
        _ColorTint ("Color", Color) = (1,1,1,1)
        _SpeedX ("Speed along X", Range(-0.25, 0.25)) = 0
        _SpeedY ("Speed along Y", Range(-0.25, 0.25)) = 0
        _Frequency ("Wave frequency", Float) = 1
        _Amplitude ("Wave hight", Float) = 0.1
        _WaveSpeed ("Wave speed", Float) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DisplacementTexture;
            float4 _DisplacementTexture_ST;
            float _DisplacementX;
            float _DisplacementY;
            fixed4 _ColorTint;
            float _SpeedX;
            float _SpeedY;
            float _Frequency;
            float _Amplitude;
            float _WaveSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.y +=  sin(o.vertex.x * _Frequency + _WaveSpeed * _Time.y) * _Amplitude * 0.1;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv2, _DisplacementTexture);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv.y = 1.0 - i.uv.y;
                i.uv2.x += _Time.y * _SpeedX;
                i.uv2.y += _Time.y * _SpeedY;
                half disVal = tex2D(_DisplacementTexture, i.uv2).g;
                i.uv.x += disVal * _DisplacementX - _DisplacementX * 0.5;
                i.uv.y += disVal * _DisplacementY - _DisplacementY * 0.5;
                fixed4 col = tex2D(_MainTex, i.uv) * _ColorTint;
                return col;
            }
            ENDCG
        }
    }
}