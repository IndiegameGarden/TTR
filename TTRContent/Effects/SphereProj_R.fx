// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
/**
 * A texture depth/stretch effect. It can be set to different depths along the x and/or y
 * axes. Zero depth on an axis means no effect is applied.
 */

// vertex shader input, http://forums.create.msdn.com/forums/p/71866/438467.aspx
float4x4 MatrixTransform : register(vs, c0);

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
// http://en.wikipedia.org/wiki/Orthographic_projection_%28cartography%29
    float R = 5; //Depth.x;
	float phi0 = 0.0;
	float lam0 = 0.0;
	float2 x = float2(0,0);
	float rho = length(texCoord);
	float c = asin( rho );
	float sinc=sin(c);
	float cosc=cos(c);
	float sinphi0 = sin(phi0);
	float cosphi0 = cos(phi0);
	float2 xin = float2(texCoord.y - 0.5,texCoord.x-0.5) ;
	x.x = asin( cosc * sinphi0 + (xin.y * sinc * cosphi0) / rho ) ;
	x.y = lam0 + atan( (xin.x * sinc) / (rho * cosphi0  * cosc  - xin.y * sinphi0* sinc ) );
	// sample texture
	return tex2D(TextureSampler, x); 
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
		PixelShader = compile ps_3_0 PS_Draw();
    }
}
