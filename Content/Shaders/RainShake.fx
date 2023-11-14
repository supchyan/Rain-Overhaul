sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 RainShake(float4 sampleColor : COLOR0, float2 uv : TEXCOORD0) : COLOR0 { 
	uv.x += uOpacity*sin(uIntensity*16.0*uTime)/1000.0;
    uv.y += uOpacity*sin(uIntensity*4.0*uTime)/1000.0;
	float4 color = tex2D(uImage0, uv);
    return color;
}
technique ShakeTechnique {
    pass RainShake {
        PixelShader = compile ps_2_0 RainShake();
    }
}