sampler BaseTextureSampler : register(s0);
sampler TextureSampler1 : register(s1);

struct VertexShaderOutput
{
    float4 Position : POSITION;
    float2 TextureCoords: TEXCOORD;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 baseCol = tex2D(BaseTextureSampler, input.TextureCoords);
	float4 col1 = tex2D(TextureSampler1, input.TextureCoords);
	
	return lerp(baseCol, float4(col1.xyz,1.0f), 1-col1.a);
    //return float4(baseCol.xyz*(1-col1.a) + col1.xyz, 1);
}

technique TextureMerger
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
