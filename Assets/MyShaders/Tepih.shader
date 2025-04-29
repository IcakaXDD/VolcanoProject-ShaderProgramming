Shader "Custom/CenteredSquareShader" {
    Properties {
        _BackgroundColor ("Background Color", Color) = (0, 0, 0, 1)

        _Square1Color ("Square Color 1", Color) = (1, 1, 1, 1)
        _Square2Color ("Square Color 2", Color) = (1, 1, 1, 1)
        _Square3Color ("Square Color 3", Color) = (1, 1, 1, 1)
        _Tint ("LightTint", Color) = (1,1,1,1)

        _Size ("Square Size 1", Range(0, 1)) = 0.5
        _Size2("Square Size 2", Range(0,1)) = 0.3
        _Size3("Square Size 3", Range(0,1)) = 0.2

        _CutOff("Cutoff",Range(0.0,1.0)) = 0
        
        _SecondaryTex("Secondary Texture",2D) = "white"{}

        _Feather("Feather", Range(0.0,0.1)) = 0

        _BurnColor("Burn Color", Color) =(1,1,1,1)
    }

    SubShader {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;             
            };

            fixed4 _BackgroundColor, _Square1Color, _Square2Color,_Square3Color;
            float _Size, _EdgeWidth,_Size2,_Size3, _Feather;
            float4 _Tint;

            sampler2D _SecondaryTex;
            float4 _SecondaryTex_ST;

            float _CutOff;
            float4 _BurnColor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv2 = TRANSFORM_TEX(v.uv, _SecondaryTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {

                fixed4 secondaryTex = tex2D(_SecondaryTex,i.uv2.xy);

                float2 center = float2(0.5, 0.5); 
                float halfSize1 = _Size * 0.5;
                float halfSize2 = _Size2*0.5;
                float halfSize3 = _Size3*0.5;

                float minX1 = center.x - halfSize1;
                float maxX1 = center.x + halfSize1;
                float minY1 = center.y - halfSize1;
                float maxY1 = center.y + halfSize1;

                float minX2 = center.x - halfSize2;
                float maxX2 = center.x + halfSize2;
                float minY2 = center.y - halfSize2;
                float maxY2 = center.y + halfSize2;

                float minX3 = center.x - halfSize3;
                float maxX3 = center.x + halfSize3;
                float minY3 = center.y - halfSize3;
                float maxY3 = center.y + halfSize3;

                fixed3 burnArea = step(secondaryTex.r-_Feather, _CutOff);
                
                if(i.uv.x > minX3 && i.uv.x < maxX3 && 
                    i.uv.y > minY3 && i.uv.y < maxY3){
                        fixed alpha = _Square3Color.a*step(secondaryTex.r,_CutOff);
                        fixed3 col =lerp(_Square3Color,_BurnColor,burnArea);
                        return fixed4(col,1) * _Tint;
                        }
                if(i.uv.x > minX2 && i.uv.x < maxX2 && 
                    i.uv.y > minY2 && i.uv.y < maxY2){
                        fixed alpha = _Square2Color.a*step(secondaryTex.r,_CutOff);
                        return fixed4(_Square2Color.rgb,alpha) * _Tint;
                        }
                if (i.uv.x > minX1 && i.uv.x < maxX1 && 
                    i.uv.y > minY1 && i.uv.y < maxY1) {
                    fixed alpha = _Square1Color.a*step(secondaryTex.r,_CutOff);
                    return fixed4(_Square1Color.rgb,alpha) * _Tint;
                    
                }
                fixed alpha = _BackgroundColor.a*step(secondaryTex.r,_CutOff);
                return fixed4(_BackgroundColor.rgb,alpha) * _Tint;
            }
            ENDCG
        }
    }
}