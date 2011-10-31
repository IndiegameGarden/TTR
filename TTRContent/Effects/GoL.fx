// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
//
// RGBA components
// R - logo as seen
// G - path where GOL can go - initially
// B - unused
// A - transparency around logo AND GOL cell values

// variables
// will be set via parameter depending on texture size such that delta corresponds to one pixel
float DeltaPixelX = 0.002;
// will be set via parameter depending on texture size
float DeltaPixelY = 0.002;
// will be set by app with the simTime
float Time = 0;
// may be changed
float PosToTimeScaling = 15;
// threshold variation tbd
float ThVar = 0;
float DoGolUpdate = 0;
// vertex shader input, http://forums.create.msdn.com/forums/p/71866/438467.aspx
float4x4 MatrixTransform : register(vs, c0);

sampler TextureSampler : register(s0) = 
sampler_state
{
    AddressU = Clamp;
    AddressV = Clamp;
};

// inits the GOL cells based on a color component in graphic
float4 PS_BufferInit(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 p = tex2D(TextureSampler, texCoord);
	float4 res = p;	
	float intens = p.g;

	// set the GoL cell possibly to '1' if local pixel is bright or dark
	if (intens >= 0.5 )
	{
		res.a = 1;
		res.b = intens;
	}
	else
	{
		res.a = 0;
		res.b = intens;
	}
	return res;
}

float4 PS_Update(float2 texCoord : TEXCOORD0) : COLOR0
{   
	// current p and new pixel value pnew
	float4 p = tex2D(TextureSampler, texCoord);
	float4 pnew = p; // assumption: copy all previous unless changes mode (eg to alpha channel)

	// direction vectors
	float2 dx = float2(DeltaPixelX,0);
	float2 dy = float2(0,DeltaPixelY);

	// sample the neighborhood pixels
    float4 l = tex2D(TextureSampler, texCoord - dx);	
	float4 r = tex2D(TextureSampler, texCoord + dx);
	float4 u = tex2D(TextureSampler, texCoord - dy);
	float4 d = tex2D(TextureSampler, texCoord + dy);
	float4 ul = tex2D(TextureSampler, texCoord - dy - dx);
	float4 ur = tex2D(TextureSampler, texCoord - dy + dx);
	float4 ll = tex2D(TextureSampler, texCoord + dy - dx);
	float4 lr = tex2D(TextureSampler, texCoord + dy + dx);
	
	// GoL stats
	// ns: neighborhood sum
	float4 ns = l + r + u + d + ul + ur + ll + lr;
	// pns: neighborhood sum with middle pixel
	float4 pns = ns + p;
	// golSum: Game of Life cells sum (from Alpha chan)
	float golSum = ns.a ; 
	float golSumLead = pns.g; // calc sum of 'leading color' pixels
	float th1 = 0; //(1-pns.r/9)/2.0;

	// apply fade rules - fading towards target
	const float db = 2*(0.00390625); // in units of 1/256 float value.

	// apply GoL rules, golSum may be 0-8
	if (DoGolUpdate==1) 
	{
		if (p.a > 0.5)   
		{	// living cell

			if (golSum > (1.5+ThVar+th1) && golSum < (3.5+ThVar-th1) )
			{
				pnew.a = 1; // cell lives on
			}else{
				pnew.a = 0 ; // cell dies
			}
		}
		else
		{   // dead cell
			if (golSum > (1.5+ThVar+th1) && golSum < (2.5+ThVar-th1) && golSumLead > 0)	// cell spawns
				pnew.a = 1;  // cell spawns
		}
	}

	float target = pnew.a * (0.3 + 0.7 * p.r);
	if (target > pnew.b )
		pnew.b += db;
	else if (target < pnew.b )
		pnew.b -= db;

	return pnew ;

}

// use the pixel Alpha channel (=GoL cell values) to influence drawing of bitmap
float4 PS_Draw(float2 texCoord : TEXCOORD0) : COLOR0
{
	// current p
	float4 p = tex2D(TextureSampler, texCoord);
	float4 pnew = p;

	// direction vectors
	float2 dx = float2(DeltaPixelX,0);
	float2 dy = float2(0,DeltaPixelY);

	// sample the neighborhood pixels
    float4 l = tex2D(TextureSampler, texCoord - dx);	
	float4 r = tex2D(TextureSampler, texCoord + dx);
	float4 u = tex2D(TextureSampler, texCoord - dy);
	float4 d = tex2D(TextureSampler, texCoord + dy);
	float4 ul = tex2D(TextureSampler, texCoord - dy - dx);
	float4 ur = tex2D(TextureSampler, texCoord - dy + dx);
	float4 ll = tex2D(TextureSampler, texCoord + dy - dx);
	float4 lr = tex2D(TextureSampler, texCoord + dy + dx);
	
	// stats
	// ns: neighborhood sum
	float4 ns = l + r + u + d + ul + ur + ll + lr;
	// pns: current-pixel and neighborhood sum
	float4 pns = ns + p;

	if (pns.r > 0 && pns.b > 0)
		pnew = float4( p.r*p.b, 0.05*pns.b, 0.01*pns.b, 0.5) ; // 0.1*pns.b*p.r+0.8*p.b 
	else
		pnew = p.b * float4( 0.6 + 0.1*pns.g, 0.2, 0.6, 0.5) ;

	pnew.a = 0;
	return pnew;
}

// see http://forums.create.msdn.com/forums/p/71866/438467.aspx
void SpriteVertexShader(inout float4 color    : COLOR0, 
                        inout float2 texCoord : TEXCOORD0, 
                        inout float4 position : SV_Position) 
{ 
    position = mul(position, MatrixTransform); 
} 

/*
 * initial filling of the GoL update buffer from a texture/bitmap
 */
technique BufferInit
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PS_BufferInit();
		VertexShader = compile vs_3_0 SpriteVertexShader();
    }
}

// this is called recursively on the GoL texture buffer
technique Update
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PS_Update();
		VertexShader = compile vs_3_0 SpriteVertexShader();
    }
}

// this is only used to draw the buffer to a visible screen, not updates
technique Draw
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PS_Draw();
		VertexShader = compile vs_3_0 SpriteVertexShader();
    }
}

