Shader "Unlit/MovingLava"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Texture", 2D) = "white" {} 
        _DistortionIntensity ("Distortion Intensity", Float) = 1
        _AnimationParams("Animation Params", vector) = (0,0,0,0)
        

        _WaveFrequency("Wave Frequency",Float)= 0.1
        _WaveSpeed("Wave Speed",Float) = 2
        _WaveAmplitude("Wave Amplitude",Float) = 0.03

        //_LavaBrigthness("Lava Brightness", Float) = 0.7 
        _LavaPulseFrequency("Color pulse Frequency",Float)= 0.15
        _LavaPulseSpeed("Color pulse speed",Float) = 0.5
       
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
            float4 _MainTex_ST, _Distortion_ST, _AnimationParams;
            float _LavaBrightness, _WaveFrequency, _DistortionIntensity, _WaveSpeed,_WaveAmplitude,_LavaPulseFrequency,_LavaPulseSpeed;
            

            v2f vert (appdata v)
            {
                
                v2f o;
                o.uv_Main = TRANSFORM_TEX(v.uv, _MainTex);
                float wave = sin(v.uv*_WaveFrequency + _Time.y*_WaveSpeed)* _WaveAmplitude;

                o.vertex = UnityObjectToClipPos(v.vertex+wave);

                o.uv_Distortion = TRANSFORM_TEX(v.uv, _Distortion);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 distortion = tex2D(_Distortion, i.uv_Distortion+_AnimationParams.xy*_Time.y);
                fixed heat = tex2D(_Distortion, i.uv_Distortion+_AnimationParams.zw*_Time.y).b;
                float2 uv = i.uv_Main + (distortion.rg * _DistortionIntensity);

                heat = sin(heat*_LavaPulseFrequency + _Time.y*_LavaPulseSpeed)+0.7;

                fixed4 col = tex2D(_MainTex, uv);
                col.r *= 2;
                col.b *= 1.5;
                col = pow(col,2)*(heat+1); //pow() - increases the brightness and contrast
                return col;
            }
            ENDCG
        }
    }
}
