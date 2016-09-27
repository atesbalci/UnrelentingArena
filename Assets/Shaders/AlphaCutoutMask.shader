Shader "Cutoff Mask" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", COLOR) = (1,1,1,1)
		_BackTex ("Background Texture", 2D) = "white" {}
		_BackColor ("Background Color", COLOR) = (1,1,1,0)
		_Mask ("Mask", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	SubShader {
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
        Lighting Off
		Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			uniform sampler2D _MainTex;
			uniform sampler2D _BackTex;
			uniform sampler2D _Mask;
			uniform float _Cutoff;
			uniform float4 _Color;
			uniform float4 _BackColor;

			struct vin {
				float4 pos : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct vout {
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
			};

            vout vert(vin v) {
				vout o;
                o.pos = mul(UNITY_MATRIX_MVP, v.pos);
				o.tex = v.texcoord;
				return o;
            }

            float4 frag(vout v) : COLOR {
				if(tex2D(_Mask, v.tex).a < _Cutoff)
					return tex2D(_BackTex, v.tex).rgba * _BackColor;
				else
					return tex2D(_MainTex, v.tex).rgba * _Color;
            }
            ENDCG
        }
	} 
	Fallback "Diffuse"
}  