Shader "Unlit/MovingLava"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1.0
        _Scale ("Scale", Float) = 5.0
        _DistortionIntensity ("Distortion Intensity", Float) = 1
        _AnimationParams("Animation Params", vector) = (0,0,0,0)
       
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
                float2 uv_Main : TEXCOORD0;
                float2 uv_Distortion : NORMAL;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _Distortion;
            float4 _MainTex_ST, _Distortion_ST;
            float _Speed;
            float _Scale;
            float _DistortionIntensity;
            float4 _AnimationParams;

            v2f vert (appdata v)
            {
                
                v2f o;
                o.uv_Main = TRANSFORM_TEX(v.uv, _MainTex);
                float wave = sin(v.uv*10+_Time.y*2)*0.3;
                o.vertex = UnityObjectToClipPos(v.vertex+wave);
                o.uv_Distortion = TRANSFORM_TEX(v.uv, _Distortion);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 distortion = tex2D(_Distortion, i.uv_Distortion+_AnimationParams.xy*_Time.y);
                fixed heat = tex2D(_Distortion, i.uv_Distortion+_AnimationParams.zw*_Time.y).b;
                //float uv_Main = tex2D(_MainTex,i.uv_Main+_AnimationParams.zw*_Time.x);
                float2 uv = i.uv_Main + (distortion.rg * _DistortionIntensity);
                heat = sin(heat + _Time.y *1.2)+0.8;
                fixed4 col = tex2D(_MainTex, uv);
                col = pow(col,2)*(heat+1);
                return col;
            }
            ENDCG
        }
    }
}
