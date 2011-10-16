// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt


float2 Position;
float Velocity;
float Time;
float NoiseLevel;
float Alpha;

// modify the sampler state on the zero texture sampler, used by SpriteBatch
sampler TextureSampler : register(s0) = 
sampler_state
{
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{    
	float4 res = float4(0,0,0,0);
	float2 vDif = texCoord - Position ;
	float2 vDifNorm = normalize(vDif);
	float lDif = length(vDif);

	float lWarped = lDif * lDif;

	float2 vTexSample = Position + lWarped * vDifNorm; // see notes
	// motion
	//float t = fmod(Time,sqrt(lDif));
	float t = -Time;
	vTexSample += (Velocity * t * vDifNorm );
	float2 vNoise = float2( NoiseLevel * noise(Time) , NoiseLevel * noise(Time)  ) ;
	vTexSample += vNoise;
	//float2 vVelocity = Velocity * ( (vDif / length(vDif)) );
	//float2 vTexSample = texCoord - fmod(vVelocity * Time, 1.0f) ;
	float2 vTexSample2 = Position + lWarped * vDifNorm + (Velocity * t * 0.8334 * vDifNorm);


	res += float4(0.8,lDif,0.4,0.9)  * tex2D(TextureSampler, vTexSample ) ;		  
	res += float4(lWarped,0,0.2,lDif/2.0f)  * tex2D(TextureSampler, vTexSample2 ) ;		  
	    
    return Alpha * res;
}

technique Technique2
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
		VertexShader = null;
    }
}
