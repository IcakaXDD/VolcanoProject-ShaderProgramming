Shader "Unlit/Water1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",Color) =(1,1,1,1)
        _NoiceTex ("Noice Texture",2D) = "white"{}

        _Scale("NoiceScale",Range(0.01, 10)) = 0.03
        _Amplitude("Amplitude", Range(0.01,10)) = 0.015
        _Speed("Speed", Range(0.01,5)) = 0.15
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            sampler2D _NoiceTex;
            float4 _NoiceTex_ST;

            float _Scale;
            float _Amplitude;
            float _Speed;

            v2f vert (appdata v)
            {
                v2f o;
                //float2 NoiceUV = float2(v.texcood.xy+_Time.y*_Speed)*_Scale;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float wave = sin(v.uv*_Amplitude+_Time.y*_Speed)*_Scale;
                o.vertex = UnityObjectToClipPos(v.vertex+wave);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv)*_Color;
                
                return col;
            }
            ENDCG
        }
    }
}
