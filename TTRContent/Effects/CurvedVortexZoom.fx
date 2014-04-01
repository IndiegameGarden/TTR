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
	float2 CenterPosition = float2(0.5,0.5);
	float2 vDif = texCoord - Position ;
	float2 vDifNorm = normalize(vDif);
	float lDif = length(vDif);
	lDif += NoiseLevel * noise(Time);
	float lWarped = lDif * lDif;
	float sz = 10.0 - (Velocity * Time);
	if (sz < 0)
	   sz = 0;
	sz *= sz;
	float2 vNoise;
	vNoise.x = noise(Velocity);
	vNoise.y = noise(sz);
	float2 vTexSample = CenterPosition + sz * (vDif); // + vNoise;  // vDif == lWarped * vDifNorm

	if (sz > 0)
		res += float4(0.8,1.0,0.4,0.4)  * tex2D(TextureSampler, vTexSample ) ;		  
	    
    return res;
}


technique Technique2
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}