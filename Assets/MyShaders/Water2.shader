Shader "Unlit/Water2"
{
    Properties
    {
        _Color ("Water Color", Color) = (0.2,0.5,0.7,1)
        _NoiceTex ("Noise Texture", 2D) = "white" {}
        _NormalMap1 ("Normal Map 1", 2D) = "bump" {}
        _NormalMap2 ("Normal Map 2", 2D) = "bump" {}

        _Scale ("Noise Scale", Range(0.01, 10)) = 0.03
        _Amplitude ("Amplitude", Range(0.01, 10)) = 0.015
        _Speed ("Wave Speed", Range(0.01, 5)) = 0.15
        _NormalSpeed ("Normal Scroll Speed", Range(0.01, 5)) = 0.5
        _NormalStrength ("Normal Strength", Range(0, 2)) = 1
        _LightIntensity ("Light Intensity", Range(0, 10)) = 2.5
        _LightBias ("Light Bias", Range(0, 1)) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

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
                float2 uvNoise : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _NoiceTex;
            float4 _NoiceTex_ST;
            sampler2D _NormalMap1;
            sampler2D _NormalMap2;

            float _Scale;
            float _Amplitude;
            float _Speed;
            float _NormalSpeed;
            float _NormalStrength;
            float _LightIntensity;
            float _LightBias;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                float2 waveUV = v.uv + _Time.y * _Speed;
                float noise = tex2Dlod(_NoiceTex, float4(waveUV * _Scale, 0, 0)).r;
                float wave = noise * _Amplitude;
                float3 displaced = v.vertex.xyz + float3(0, wave, 0);
                o.vertex = UnityObjectToClipPos(float4(displaced, 1));
                o.uvNoise = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv1 = i.uvNoise + _Time.y * _NormalSpeed;
                float2 uv2 = i.uvNoise - _Time.y * _NormalSpeed;

                float3 n1 = UnpackNormal(tex2D(_NormalMap1, uv1));
                float3 n2 = UnpackNormal(tex2D(_NormalMap2, uv2));
                float3 blendedNormal = normalize((n1 + n2) * 0.5);
                blendedNormal.xy *= _NormalStrength;

                float3 lightDir = normalize(float3(0.0, 1.0, 0.0));
                float lighting = saturate(dot(blendedNormal, lightDir) + _LightBias) * _LightIntensity;

                fixed4 col = _Color;
                col.rgb *= lighting;
                return col;
            }
            ENDCG
        }
    }
}
