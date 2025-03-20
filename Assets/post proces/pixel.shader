Shader "Snapshot/Pixel"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_PixelSize("Pixelation", Range(0.001, 0.1)) = 0.001
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			Pass
			{
				CGPROGRAM
				#pragma vertex vert_img
				#pragma fragment frag

				#include "UnityCG.cginc"

				sampler2D _MainTex;
				float _PixelSize;

				fixed4 frag(v2f_img i) : SV_Target
				{
					float3 col;
					float ratioX = (int)(i.uv.x / _PixelSize) * _PixelSize;
					float ratioY = (int)(i.uv.y / _PixelSize) * _PixelSize;
					col = tex2D(_MainTex, float2(ratioX, ratioY));

					return float4(col, 1.0);
				}
				
				ENDCG
			}
		}

}

