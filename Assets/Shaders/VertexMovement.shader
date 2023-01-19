Shader "Unlit/VertexMovement"
{
    Properties
    {
        _HorizontalOffset("Horizontal Offset", Float) = 0
        _Amplitude("Amplitude", Float) = 1
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("Texture2D", 2D) = "white" {}
        _SecTex("Secondary Texture2D", 2D) = "white" {}
        _Blend("Blend", Range(0,1)) = 0
        _BlendFrequency("Blend frequency", Float) = 1
        _BounceFrequency("Baunce frequency", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
            };

            float _HorizontalOffset;
            float _Amplitude;
            half4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _SecTex;
            float4 _SecTex_ST;
            float _Blend;
            float _BlendFrequency;
            float _BounceFrequency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.x += _HorizontalOffset;
                o.vertex.y += _Amplitude * 0.1 * sin(_Time.y * _BounceFrequency);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = lerp(tex2D(_MainTex,i.uv), tex2D(_SecTex,i.uv), clamp(((sin(_Time.y * _BlendFrequency) * 0.5) + 0.5), 0, 1));
                col *= _Color;
                return col;
            }
            ENDCG
        }
    }
}
