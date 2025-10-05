Shader "Hidden/GrayscaleEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Desaturation ("Desaturation", Range(0,1)) = 1
		_Contrast ("Contrast", Range(0.5,2)) = 1
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
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
			float _Desaturation;
			float _Contrast;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// Desaturate towards luminance (luminosity method)
				float gray = dot(col.rgb, float3(0.299, 0.587, 0.114));
				float3 desat = lerp(col.rgb, gray.xxx, saturate(_Desaturation));
				// Apply contrast around midpoint 0.5
				float contrast = _Contrast;
				desat = (desat - 0.5) * contrast + 0.5;
				desat = saturate(desat);
				col.rgb = desat;
				return col;
			}
			ENDCG
		}
	}
	Fallback Off
}


