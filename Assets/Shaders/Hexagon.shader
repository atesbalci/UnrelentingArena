Shader "Hexagon"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white"
		_HexPosition("HexPosition", Vector) = (0, 0, 0, 0)
	}
	SubShader
	{
		Pass
		{
			// indicate that our pass is the "base" pass in forward
			// rendering pipeline. It gets ambient and main directional
			// light data set up; light direction in _WorldSpaceLightPos0
			// and color in _LightColor0
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" // for UnityObjectToWorldNormal
			#include "UnityLightingCommon.cginc" // for _LightColor0

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 diff : COLOR0; // diffuse lighting color
				float4 vertex : SV_POSITION;
			};

			float4 _HexPosition;
			uniform float Radius = 10;
			uniform float CloseHeight = 10;
			uniform float FarHeight = 1;

			uniform float4 RiserPosition0 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition1 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition2 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition3 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition4 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition5 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition6 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition7 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition8 = float4(0, 0, 0, -1000000);
			uniform float4 RiserPosition9 = float4(0, 0, 0, -1000000);

			v2f vert(appdata_base v)
			{
				v2f o;
				float height = FarHeight;
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition0 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition1 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition2 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition3 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition4 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition5 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition6 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition7 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition8 - _HexPosition) / Radius)));
				height = max(height, lerp(CloseHeight, FarHeight, min(1, length(RiserPosition9 - _HexPosition) / Radius)));
				float4 vertexNew = float4(v.vertex.x, v.vertex.y * height, v.vertex.z, v.vertex.w);
				o.vertex = mul(UNITY_MATRIX_MVP, vertexNew);
				o.uv = v.texcoord;
				// get vertex normal in world space
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				// dot product between normal and light direction for
				// standard diffuse (Lambert) lighting
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				// factor in the light color
				o.diff = nl * _LightColor0;
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				// sample texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// multiply by lighting
				col *= i.diff;
				return col;
			}
			ENDCG
		}
	}
}