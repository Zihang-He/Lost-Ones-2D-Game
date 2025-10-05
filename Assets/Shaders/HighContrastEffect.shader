Shader "Hidden/HighContrastEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Contrast ("Contrast", Range(0.5,3)) = 1
		_Brightness ("Brightness", Range(-1,1)) = 0
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
			float _Contrast;
			float _Brightness;

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
				// Contrast around midpoint 0.5 and optional brightness offset
				float3 c = col.rgb;
				c = (c - 0.5) * _Contrast + 0.5;
				c += _Brightness;
				c = saturate(c);
				col.rgb = c;
				return col;
			}
			ENDCG
		}
	}
	Fallback Off
}


