// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
//-----------------------------------------------------------------------------
// Clouds.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


// the relative position for the texture coordinates, inducing a slight parallax
float2 Position;
float2 Stretch;


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
    texCoord.x *= Stretch.x; //0.00025f;
    texCoord.y *= Stretch.y; //0.00025f;
    //texCoord *= 0.5f;
    float4 results = float4(1,1,1,1.0) * tex2D(TextureSampler, texCoord);
        
    return results;
}


technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
