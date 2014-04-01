// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

// the relative position for the texture coordinates, inducing a slight parallax
float2 Position;

// vertex shader input, http://forums.create.msdn.com/forums/p/71866/438467.aspx
float4x4 MatrixTransform : register(vs, c0);

// modify the sampler state on the zero texture sampler, used by SpriteBatch
sampler TextureSampler : register(s0) = 
sampler_state
{
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    // sample from the cloud texture for the blue component
    texCoord.x += Position.x * 0.0002f; //0.00025f;
    texCoord.y += Position.y * 0.0002f; //0.00025f;
    texCoord *= 0.5f;
    float4 results = float4(0,0,1,0.25) * tex2D(TextureSampler, texCoord);

    // sample from the cloud texture for the green component
    texCoord.x += Position.x * 0.00025f + 0.25f;
    texCoord.y += Position.y * 0.00025f - 0.15;
    texCoord *= 0.4f;
    results += float4(0,1,0,0.15) * tex2D(TextureSampler, texCoord);
        
    return results;
}

// see http://forums.create.msdn.com/forums/p/71866/438467.aspx
void SpriteVertexShader(inout float4 color    : COLOR0, 
                        inout float2 texCoord : TEXCOORD0, 
                        inout float4 position : SV_Position) 
{ 
    position = mul(position, MatrixTransform); 
} 


technique Technique1
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 SpriteVertexShader(); 
        PixelShader  = compile ps_3_0 PixelShaderFunction();
    }
}