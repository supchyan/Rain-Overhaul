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

float4 RainBubble(float4 sampleColor : COLOR0, float2 uv : TEXCOORD0) : COLOR0 {
    float4 rain = tex2D(uImage0, uv);
    rain.rgb = float3(0.0, 0.0, 0.0);

    float2 muck = (uv-rain.xy);
    float3 raintex = tex2D(uImage0,muck).rgb/2.0;
    float3 tex = tex2D(uImage0, uv).rgb/2.0;
    
    float xMUL = (lerp(1.0, 1.05, sin(2.0*uTime)));
    float yMUL = (lerp(1.0, 1.05, cos(2.0*uTime)));

    xMUL *= (8.0 - uOpacity);
    yMUL *= (8.0 - uOpacity);
    
    float2 fragCoord = uv * uScreenResolution;

    float2 newuv = (
    float2(fragCoord.x*xMUL,fragCoord.y*yMUL)*2.0 -
    float2(uScreenResolution.x*xMUL,uScreenResolution.y*yMUL))
    /uScreenResolution.y;

    float bubble = length(newuv/2.0);
    
    return float4(tex,1.0)+float4(raintex*smoothstep(1.0,0.99,bubble),1.0)+float4(tex*smoothstep(0.85,1.0,bubble),1.0);
}
technique Technique3 {
    pass RainBubble {
        PixelShader = compile ps_2_0 RainBubble();
    }
}