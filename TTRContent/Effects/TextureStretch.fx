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
    AddressU = Wrap; // can use Clamp, or Wrap for different fx
    AddressV = Wrap;
};

// pixshader function
float4 PS_Draw(float2 texCoord : TEXCOORD0) : COLOR0
{
	// calc per-pixel viewdepth 'z' depending on position on screen
	float2 x_center = float2(0.5,0.5);
	float z;
	z = Depth.x * cos( 3.141592 * (texCoord.x - Center.x ) ) + Depth.y * cos( 3.141592 * (texCoord.y - Center.y) );

	// calc texture sampling vector based on 'z' depth
	// see paper equation x = t + beta(t-0.5)   
	float2 x = texCoord + z * (texCoord - x_center);

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


technique Draw
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 PS_Draw();
    }
}
