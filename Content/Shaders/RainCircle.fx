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

float4 RainCircle(float4 sampleColor : COLOR0, float2 uv : TEXCOORD0) : COLOR0 {
    // float2 uv = coord.xy / uScreenResolution.xy;
	float3 raintex = tex2D(uImage1,float2(uv.x*2.1,uv.y*0.1+uTime*0.05)).rgb/8.0;
	float2 where = (uv.xy-raintex.xy);
	float3 texchur1 = tex2D(uImage0,float2(where.x,where.y)).rgb/2.0;
    float3 tex = tex2D(uImage0,float2(uv.x,uv.y)).rgb/2.0;

    float xMUL = (lerp(1.0, 1.1, sin(2.0*uTime)));
    float yMUL = (lerp(1.0, 1.1, cos(2.0*uTime)));

    // float2 newuv = (
    // float2(uv.x*xMUL,uv.y*yMUL)*2.0 -
    // float2(uScreenResolution.x*xMUL,uScreenResolution.y*yMUL))
    // /uScreenResolution.y;

    float targetSize = 1.0;
    float offset = 0.7;
    float circle = length(uv) + offset - targetSize;

    return float4(tex,1.0)+float4(texchur1*smoothstep(1.0,0.99,circle),1.0)+float4(tex*smoothstep(0.95,1.0,circle),1.0);
}
technique Technique1 {
    pass RainCircle {
        PixelShader = compile ps_2_0 RainCircle();
    }
}
// glsl circle
// void mainImage( out vec4 fragColor, in vec2 fragCoord )
// {
// 	vec2 uv = fragCoord.xy / iResolution.xy;
// 	float time = iTime;
// 	vec3 raintex = texture(iChannel1,vec2(uv.x*2.1,uv.y*0.1+time*0.05)).rgb/8.0;
// 	vec2 where = (uv.xy-raintex.xy);
// 	vec3 texchur1 = texture(iChannel0,vec2(where.x,where.y)).rgb/2.0;
//     vec3 tex = texture(iChannel0,vec2(uv.x,uv.y)).rgb/2.0;
    
//     float xMUL = (mix(1.0, 1.1, sin(2.0*iTime)));
//     float yMUL = (mix(1.0, 1.1, cos(2.0*iTime)));

//     vec2 newuv = (
//     vec2(fragCoord.x*xMUL,fragCoord.y*yMUL)*2.0 -
//     vec2(iResolution.x*xMUL,iResolution.y*yMUL))
//     /iResolution.y;
    
//     float targetSize = 0.0;
//     float offset = 0.7;
//     float circle = length(newuv) + offset - targetSize;

// 	fragColor = vec4(tex,1.0)+vec4(texchur1*smoothstep(1.0,0.99,circle),1.0)+vec4(tex*smoothstep(0.95,1.0,circle),1.0);
// }
//