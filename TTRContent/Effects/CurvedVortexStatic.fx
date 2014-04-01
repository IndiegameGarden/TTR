// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

/*
 * a texture that zooms in from a point somewhere on screen
 */

float2 Position;
float Velocity;
float Time;
float NoiseLevel;
float Alpha;

// modify the sampler state on the zero texture sampler, used by SpriteBatch
sampler TextureSampler : register(s0) = 
sampler_state
{
    AddressU = Clamp;
    AddressV = Clamp;
};


float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{    
	float4 res = float4(0,0,0,0);
	float2 vDif = texCoord - Position ;
	float2 vDifNorm = normalize(vDif);
	float lDif = length(vDif);
	lDif += NoiseLevel * noise(Time);
	float lWarped = (1-Velocity)*lDif + Velocity * lDif * lDif;
	float2 vTexSample = Position + (lWarped * vDifNorm); 
	res = tex2D(TextureSampler, vTexSample ) ;		  
	    
    return res;
}


technique Technique2
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
