Shader "Unlit/Wood"
{
    Properties
	{
		_Rings("Rings Frequency",Float) =10
		_RingSharpness("Ring Sharpness",Range(0,1)) = 0.0
		_FogRange("Fog Range",Range(0,0.2)) = 0.02
		_Ambient("Ambient Light", Range(0,1)) = 0

	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION; // in object space
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 object : TEXCOORD0;		  // object space position
				float3 worldPos : TEXCOORD1;     // world space position
				float3 camPos : TEXCOORD2;      // camera/view space position

			};

			float _Rings, _RingSharpness, _FogRange, _Ambient; 

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.object = v.vertex;

				float3 world = mul(unity_ObjectToWorld, v.vertex);
				o.worldPos = UnityObjectToWorldNormal(v.normal);

				float4 cam = mul(UNITY_MATRIX_MV, v.vertex);
				o.camPos = cam;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				//Wood Rings
				//float dist = length(i.object.xy);
				float woodPattern = fmod(i.object.y*_Rings, 1);
	
				float4 col = float4(0.5, 0.3, 0.1, 1.0) * woodPattern;

				//Height-based tint
				float heightFactor = saturate(i.worldPos.y * _RingSharpness); // the higher the y in world space the brighter
				col *= lerp(0.8, 1.2, heightFactor); // blend the bridhtness of the y color 

				//Fog
				float camDist = length(i.camPos);
				float fogAmount = saturate(1.0 - camDist * _FogRange); 
				float4 fogColor = float4(0.5, 0.5, 0.5, 1); // gray color
				col = lerp(fogColor, col, fogAmount); // blend and mixing color with fog

				float3 N = normalize(i.worldPos.xyz);
			    float3 worldLigthDir = normalize(_WorldSpaceLightPos0.xyz);
				float diff = saturate(dot(N,worldLigthDir)); //0 = edge, 1= light hit

				float3 backCol = col.rgb*_Ambient;
				col.rgb = lerp(backCol,col.rgb,diff);

				return col;
			}
			ENDCG
		}
	}
}
