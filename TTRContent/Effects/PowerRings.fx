// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

#define Nslots 8

// vertex shader input, http://forums.create.msdn.com/forums/p/71866/438467.aspx
float4x4 MatrixTransform : register(vs, c0);

float4 RingColor;
float2 aPosition[Nslots];
float aSize[Nslots];
float aWidth[Nslots];
float AspectRatio;

// modify the sampler state on the zero texture sampler, used by SpriteBatch
sampler TextureSampler : register(s0) = 
sampler_state
{
    AddressU = Wrap;
    AddressV = Wrap;
};

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
   	// correct for aspect ratio
	texCoord.x *= AspectRatio;
	float a_max = 0;
	float a;
	for(int i=0; i < Nslots; i++)
	{
		if (aSize[i] > 0.0f)
		{
			float rCurPos = length( texCoord - aPosition[i] );
			a = exp(-aWidth[i] * (abs(aSize[i]-rCurPos))) ;
			//a *= ( cos(6.28 * 5 * abs(aSize[i]-rCurPos)) ); // nice
			a *= (0.4 + cos(6.28 * 5 * abs(aSize[i]-rCurPos)) ); // great effect
			if(a>a_max)
				a_max = a;
		}
     }

	return a_max * a_max * (
			float4(0,0,1,0.5)  * tex2D(TextureSampler, texCoord ) + RingColor
			);
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
		PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}