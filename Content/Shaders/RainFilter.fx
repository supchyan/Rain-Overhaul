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

float4 RainFilter(float4 sampleColor : COLOR0, float2 uv : TEXCOORD0) : COLOR0 {
    float velocity = 0.3f;
    float forceX = 5.0f; // changed it to make more rainworldy
    float forceY = 0.01f;

    float4 distort = tex2D(uImage1, float2(
        uv.x * forceX + uv.y * sin(uTime)/4.0f * uProgress,
        uv.y * forceY + uTime * velocity
    ));
    float2 rainVec = (uv.xy - distort.xy*uOpacity);

    float4 rain = tex2D(uImage0, rainVec);

    rain.rgb *= float3(1 + 0.01f*uIntensity, 1, 1 + 0.2f*uIntensity);

    float saturation = rain.a / (1 + uIntensity);

    float3 weights = dot(rain.rgb, float3(0.299,0.587,0.114));
    rain.rgb = lerp(weights, rain.rgb, saturation);

    return rain;
}
technique Technique1 {
    pass RainFilter {
        PixelShader = compile ps_2_0 RainFilter();
    }
}