Shader "Custom/CloudShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader 
	{
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert_img
			#pragma fragment frag

			float rand(float2 n) 
			{ 
				return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
			}

			float noise(float2 n) 
			{
				const float2 d = float2(0.0, 1.0);
				float2 b = floor(n), f = smoothstep(float2(0.0,0.0), float2(1.0,1.0), frac(n));
				return lerp(lerp(rand(b), rand(b + d.yx), f.x), lerp(rand(b + d.xy), rand(b + d.yy), f.x), f.y);
			}

			float fbm(float2 n) 
			{
				float2 dirs[4];
				dirs[0] = float2(0.0, 0.3);
				dirs[1] = float2(0.5, 0.6);
				dirs[2] = float2(-0.1, 0.9);
				dirs[3] = float2(0.7, 1.2);
    
				float total = 0.0;
				float amplitude = 0.5;

				for (int i = 0; i <4; i++) {
					total += noise(n +_Time.w*dirs[i]) * amplitude;
					n += n;
					amplitude *= 0.8;
				}
				return total;
			}

			float clouds(float2 n) 
			{
				float ampl = noise(n + float2(_Time.w, _Time.w*0.1)*0.1);
  
				float strips= sin(_Time.w*1.0 + n.y*15.0 + n.x*4.0 + 8.0*noise(float2(0.0, n.y*1.0)));
				float c = ampl *0.2*strips + 1.0;
				return c;
			}			

			float4 frag(v2f_img xo) : COLOR
			{
				float2 uv = xo.uv;

				float3 r=float3(uv.x * 1.50 - 0.5, uv.y, 1.0);
				r *= 2.0/ uv.y;

				float fog = clamp(0.0, 1.0, exp(-(r.z-8.0)*0.2));

				float4 fogColor = float4(0.8, 0.9, 1.0, 1.0);
				float c = fbm(r.xz)*1.2;
				c *= clouds(r.xz);
				c = lerp(0.2, c, fog);
				float4 res = lerp(float4(0.8,0.8,1.0,0.0), float4(0.2,0.2,0.1,0.0), c);
				return float4(res);
			}

			ENDCG
		}//END of Pass
	}//END of SubShader
}//END of Shader
