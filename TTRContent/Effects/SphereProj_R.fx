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
    AddressU = Wrap; // can use Clamp, or Wrap for different fx
    AddressV = Wrap;
};

// pixshader function
float4 PS_Draw(float2 texCoord : TEXCOORD0) : COLOR0
{
// http://en.wikipedia.org/wiki/Orthographic_projection_%28cartography%29
    float R = Depth.x;
	//float phi0 = 0.4;
	float lam0 = 0.6;
	float2 x = 0;
	float rho = length(texCoord);
	float c = asin( rho / R);
	float sinc=sin(c);
	float cosc=cos(c);
	float sinphi0 = 0.3;
	float cosphi0 = 0.7;
	//x.x = asin( cosc * sinphi0 + (texCoord.y * sinc * cosphi0) / rho ) ;
	//x.y = lam0 + atan ( x * sinc / (rho * cosphi0  * cosc  - texCoord.y * sinphi0* sinc ) );
	x.x = asin( (texCoord.y * sinc * cosphi0) / rho ) ;
	x.y = lam0 + x * sinc / (rho * cosphi0  * cosc  - texCoord.y * sinphi0* sinc ) ;
	// sample texture
	return tex2D(TextureSampler, x); 
}

technique Draw
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PS_Draw();
    }
}
