Shader "Sprites/Bumped Diffuse with Shadows"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
				_Cutoff ("Alpha Cutoff", Range (0,1)) = 0.5
		speed ("Speed", Range(0, 100)) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"			
		}


		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf NoLighting vertex:vert addshadow nofog keepalpha alphatest:_Cutoff
		#pragma multi_compile DUMMY PIXELSNAP_ON 

		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _Color;
		float speed;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpMap;
			fixed4 color;
		};

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			return float4(0.0,0.0,0.0,s.Alpha);
		}
		
		void vert (inout appdata_full v, out Input o)
		{
			#if defined(PIXELSNAP_ON) && !defined(SHADER_API_FLASH)
			v.vertex = UnityPixelSnap (v.vertex);
			#endif
			
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color * _Color;
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
			o.Albedo = c.rgb * c.a * (float3(1.0,1.0,1.0) + (speed * float3(0.0, -sin(_Time.w * speed), -cos(_Time.w * speed))));
			o.Alpha = c.a;
			o.Emission = o.Albedo;
		}


		ENDCG
	}

Fallback "Transparent/Cutout/Diffuse"
}
