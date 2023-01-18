Shader "Unlit/Geometry"
{
    Properties
    {
        _HorizontalOffset("Horizontal Offset", Float) = 0
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("Texture2D", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _HorizontalOffset;
            half4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2g vert (appdata v)
            {
                v2g o;
                o.uv = v.uv; // Przekazujemy wartoœæ z appdata do obiektu o
                o.vertex = v.vertex; // j.w.
                o.vertex.x += _HorizontalOffset;
                return o;
            }


            [maxvertexcount(3)]
            void geom(triangle v2g input[3], inout TriangleStream<g2f> triStream)
            {
                g2f o;
                float3 pos = input[0].vertex.xyz;
                o.uv = input[0].uv;

                o.vertex = UnityObjectToClipPos(pos + float3(0.5, 0, 0));
                triStream.Append(o);
                o.vertex = UnityObjectToClipPos(pos + float3(-0.5, 0, 0));
                triStream.Append(o);
                o.vertex = UnityObjectToClipPos(pos + float3(0, 1, 0));
                triStream.Append(o);
                triStream.RestartStrip();
            }

            fixed4 frag (g2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;
                return col;
            }
            ENDCG
        }
    }
}
