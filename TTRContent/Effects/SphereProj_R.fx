// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
/**
 * A texture depth/stretch effect. It can be set to different depths along the x and/or y
 * axes. Zero depth on an axis means no effect is applied.
 */

/// variables
// current (instantaneous) depth effect (around 0). >0 is ... and <0 is ....
// values for x axis and y axis. 0 is no effect.
float2 Depth = float2(0.1,0.1);
float2 Center = float2(0.5,0.5);

// tex sampler
sampler TextureSampler : register(s0) = 
sampler_state
{
    AddressU = Clamp; // can use Clamp, or Wrap for different fx
    AddressV = Clamp;
};

// pixshader function
float4 PS_Draw(float2 texCoord : TEXCOORD0) : COLOR0
{
	float R = 1;
	float2 xc = float2(0,0);

	float2 x = float2(0,0);
	float2 xxc = 2*(texCoord-xc);
	float2 term = R*R - (xxc)*(xxc);
	float4 t=float4(0,0,0,0);
	if (term.x >=0 && term.y >= 0) {
		x=sqrt( term ) + xc;
		t = tex2D(TextureSampler, x); 
	}
	return t;
}

technique Draw
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PS_Draw();
    }
}
