Shader "Hidden/Vignette"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_VColor("Vignette Color", COLOR) = (0,0,0,0.5)
		_Brightness("Brightness", Range(0, 2)) = 1.0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed4 _VColor;
			fixed _Brightness;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 coord = i.uv;
				float2 center = 0.5;
				float d = distance(coord, center);
				d = pow(d, _VColor.a * 6);
				fixed4 col = tex2D(_MainTex, coord);
				
				col = lerp(col, _VColor, d) * _Brightness;
				return col;// *1 - (_VColor * d);
			}
			ENDCG
		}
	}
}
