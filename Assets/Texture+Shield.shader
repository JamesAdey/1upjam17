Shader "Custom/Texture+Shield" {
Properties
 {
 	_MainTexColor("Main Colour", Color) = (1,1,1,1)
    _ShieldColor("Shield Colour", Color) = (0,1,0,1)
    _Inside("_Inside", Range(0,0.2) ) = 0
    _Rim("_Rim", Range(1,2) ) = 2
    _MainTexture("Main Texture", 2D) = "black" {}
    _ShieldTexture("Shield Texture", 2D) = "white" {}
    _Speed("_Speed", Range(0.5,5) ) = 0.5
    _Tile("_Tile", Range(1,10) ) = 5
    _Strength("_Strength", Range(0,5) ) = 1.5
 }
    
 SubShader
 {
     Tags
     {
     
         "Queue"="Transparent"
         "RenderType"="Transparent"
 
    }
 
      
	ZWrite On
	ZTest LEqual
 
		
		CGPROGRAM
		#pragma surface surf BlinnPhong

		fixed4 _MainTexColor;
		fixed4 _ShieldColor;
		fixed _Inside;
		fixed _Rim;
		sampler2D _MainTexture;
		sampler2D _ShieldTexture;
		fixed _Speed;
		fixed _Tile;
		fixed _Strength;

		struct Input {
			float4 screenPos;
			float3 viewDir;
			float2 uv_ShieldTexture;
			float2 uv_MainTexture;
			
		};

		inline fixed Fresnel(float3 viewDir)
		{
			float3 up = float3(0.0f,0.0f,1.0f);
			
			float theta = dot( normalize(viewDir), up);
			
			return fixed(1.0f - theta);
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			// calculate Fresnel
			fixed fresnel = Fresnel( IN.viewDir );
			
			// fresnel related effects
			fixed stepfresnel = step( fresnel , fixed(1.0f));
			fixed insideContrib = clamp( stepfresnel , _Inside , fixed(1.0f));
			fixed rimContrib = pow( fresnel , _Rim );
			
			// calculate texture coords
			half timeOffset = half(_Time.x) * _Speed;
			half2 shieldTexCoords = float2( IN.uv_ShieldTexture.x , IN.uv_ShieldTexture.y + timeOffset );
			half2 mainTexCoords = float2( IN.uv_MainTexture.x, IN.uv_MainTexture.y);
			shieldTexCoords *= _Tile.xx;
			
			// and get the texture contribution
			fixed3 mainTexContrib = tex2D (_MainTexture, mainTexCoords);
			fixed shieldContrib = tex2D (_ShieldTexture, shieldTexCoords) * _Strength;
			
			// set the colours
			o.Albedo = mainTexContrib * _MainTexColor.rgb;
			o.Emission = rimContrib * insideContrib * shieldContrib * _ShieldColor.rgb * _ShieldColor.a;
			o.Normal = fixed3(0.0,0.0,1.0);
		}
		ENDCG
	} 
	 Fallback "Diffuse"
}

