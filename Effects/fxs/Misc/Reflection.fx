sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);

float2 uReflectorScreenPosition;
float2 uReflectorScreenSize;
float2 uPlayerScreenPosition;
float2 uSheetSize;
bool uFlipVertical = false;
float Brightness = -0.3f;
float uStrength = 100.0f;

float4 Reflect(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 reflector = tex2D(uImage0, coords);
    float4 normalMap = tex2D(uImage2, coords);
    
    float2 reflectedCoords = uReflectorScreenPosition + coords * uReflectorScreenSize;
    
    if(uFlipVertical)
    {
        reflectedCoords.y = 2 * uReflectorScreenPosition.y - reflectedCoords.y;
    }
    
    reflectedCoords = (reflectedCoords - uPlayerScreenPosition) / uSheetSize;
    
    float2 normalMapOffset = normalMap.rg;
    normalMapOffset = normalMapOffset * 2 - 1;
    normalMapOffset = normalMapOffset * uStrength / uSheetSize;
    
    reflectedCoords.xy += normalMapOffset.rg;
    
    if ((reflectedCoords.x > 1 || reflectedCoords.x < 0) || (reflectedCoords.y > 1 || reflectedCoords.y < 0))
    {
        return (reflector + Brightness) * reflector.a;
    }

    
    float4 reflected = tex2D(uImage1, reflectedCoords);
    
    
    return (reflector + reflected + Brightness) * reflector.a;
}

technique Technique1
{
    pass Reflection
    {
        PixelShader = compile ps_3_0 Reflect();
    }
}
