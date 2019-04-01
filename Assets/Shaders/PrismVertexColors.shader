Shader "Custom/NewSurfaceShader" {
	
	SubShader {
		
		
		CGPROGRAM
		
		#pragma surface surf Lambert 
		
		struct Input
		{
		    float4 verColors : COLOR;
		};
		
		void surf(Input IN, inout SurfaceOutput o)
		{
		    o.Albedo = IN.verColors;
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}
