Shader "AlphaSelfIllum" {
	Properties {
		_Color ("Color Tint", Color) = (1,1,1,1)
		_MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white"
	}
	Category {
		SubShader {
			Tags{ "Queue" = "Overlay" }
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			Material {
				Emission [_Color]
			}
			Pass {
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag

				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;
				uniform float4 _Color;

				fixed4 frag(v2f_img i) : SV_Target{
					return tex2D(_MainTex, i.uv) * _Color;
				}
				ENDCG
			}
		}
	}
}