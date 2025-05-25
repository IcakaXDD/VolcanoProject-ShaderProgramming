Shader "Custom/Target" {
    Properties {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (1, 1, 1, 1)
        _BackgroundColor ("Background", Color) = (0, 0, 0, 0)
        _Radius1 ("Outer Radius", Range(0,0.5)) = 0.5
        _Radius2 ("Mid Radius", Range(0,0.5)) = 0.3
        _Radius3 ("Inner Radius", Range(0,0.5)) = 0.1
    }

    SubShader {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color1, _Color2, _BackgroundColor;
            float _Radius1, _Radius2, _Radius3;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                if (dist < _Radius3)
                    return _Color1;
                if (dist < _Radius2)
                    return _Color2;
                if (dist < _Radius1)
                    return _Color1;

                return  fixed4(_BackgroundColor.rgb,0);
            }
            ENDCG
        }
    }
}