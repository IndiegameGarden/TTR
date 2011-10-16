// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

// variables
// will be set via parameter depending on texture size
float DeltaPixelX = 0.002;
// will be set via parameter depending on texture size
float DeltaPixelY = 0.002;
// will be set by app with the simTime
float Time = 0;
// may be changed
float PosToTimeScaling = 15;
// threshold variation tbd
float ThVar = 0;

sampler TextureSampler : register(s0) = 
sampler_state
{
    AddressU = Clamp;
    AddressV = Clamp;
};

float4 PixelShaderFunction_BufferInit(float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 p = tex2D(TextureSampler, texCoord);
	float4 res = p;	
	float intens = (p.r + p.g + p.b) ;

	// set the GoL cell possibly to '1' if local pixel is bright or dark
	if (intens > 0 )
		res.a = 1;
	else
		res.a = 0;
	//res.a = 0;
	return res;
}

float4 PixelShaderFunction_Update(float2 texCoord : TEXCOORD0) : COLOR0
{   
	// current p and new pixel value pnew
	float4 p = tex2D(TextureSampler, texCoord);
	float4 pnew = p;

	// pixel time (for this pixel)
	float pTime = texCoord.x * PosToTimeScaling;

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
	// golSum: Game of Life cells sum (from Alpha chan)
	float golSum = ns.a ; 
	float pns1 = (pns.r + pns.g + pns.b)/(9*3);
	float pns1a = pns.a/(9);

	// calc closeness to render borders 
	const float BX = 0.1, BY = 0.1;
	float borderNearness = 0;
	if (texCoord.x < BX) borderNearness += (BX -texCoord.x);
	if (texCoord.y < BY) borderNearness += (BY -texCoord.y);
	if (texCoord.x > 1-BX) borderNearness += (BX -1+texCoord.x);
	if (texCoord.y > 1-BY) borderNearness += (BY -1+texCoord.y);

	if (Time >= pTime)
	{
		// apply GoL rules, golSum may be 0-8
		if (p.a > 0.5)   
		{	// living cell
			if (golSum > (1.5+ThVar) && golSum < (3.5+ThVar) && borderNearness < 0.02 )
			{
				pnew.a = 1; // cell lives on
			}else{
				pnew.a = 0 ; // cell dies
			}
		}
		else
		{   // dead cell
			if (golSum > (1.5+ThVar) && golSum < (2.5+ThVar) && pns1a > 0 && borderNearness < 0.02)	// cell spawns
				pnew.a = 1;
			else
				pnew.a = 0;

				/*
			else if (golSum > (1.5+ThVar) && golSum < (2.5+ThVar) && pns1a > 0 && borderNearness < 0.5)	// cell spawns
				pnew.a = 2*pns1a;
				*/
		}
	}else{
		// my time (based on pTime) has not yet come
		
	}

	return pnew ;

}

// use the pixel Alpha channel (=GoL cell values) to influence drawing of bitmap
float4 PixelShaderFunction_Draw(float2 texCoord : TEXCOORD0) : COLOR0
{
	// current p and new pixel value pnew
	float4 p = tex2D(TextureSampler, texCoord);
	float4 pnew = p;

	// pixel time (for this pixel)
	float pTime = texCoord.x * PosToTimeScaling;

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
	// golSum: Game of Life cells sum (from Alpha chan)
	float golSum = ns.a ; 
	float pns1 = (pns.r + pns.g + pns.b)/(9*3);

	// calculate distance from 'middle line' midDist = 0...1
	float midDistY = 2*abs(texCoord.y-0.5);
	float midDistX = 2*abs(texCoord.x-0.5);

	float tfade = 3;

	if (Time >= pTime )
	{
		pnew = float4( p.a,p.a,p.a,1);
		//pnew = p.a * p;
		//pnew = pns.a * p; 
		//pnew = pns.a * float4( pns.r, pns.g, pns.b,1);
		pnew *= 1-midDistY*midDistY;
		if(midDistX > 0.9)
			pnew *= 1-10*(midDistX-0.9); //*midDistX;
		pnew *= 1 - (pTime+tfade-Time)/tfade;
	}else{
		pnew = float4( 0,0,0,0);
	}

	// make black pixels always transparent.
	if ((pnew.r + pnew.g + pnew.b) == 0)
	   pnew.a = 0;
	return pnew;
}

/*
 * initial filling of the GoL update buffer from a texture/bitmap
 */
technique BufferInit
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction_BufferInit();
    }
}

// this is called recursively on the GoL texture buffer
technique Update
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction_Update();
    }
}

// this is only used to draw the buffer to a visible screen, not updates
technique Draw
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction_Draw();
    }
}

