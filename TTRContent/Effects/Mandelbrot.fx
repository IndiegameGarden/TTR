// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
// from http://blog.nostatic.org/2009/09/fractal-rendering-on-gpu-mandelbrot-and.html

// vertex shader input, http://forums.create.msdn.com/forums/p/71866/438467.aspx
float4x4 MatrixTransform : register(vs, c0);

sampler TextureSampler : register(s0);

// mandelbrot params
int Iterations = 128;
float2 Pan = float2(0.5, 0);
float Zoom = 3;
float Aspect = 1;
float2 JuliaSeed = float2(0.39, -0.2);
float3 ColorScale = float3(4, 5, 6);

float ComputeValue(float2 v, float2 offset)
{
float vxsquare = 0;
float vysquare = 0;

int iteration = 0;
int lastIteration = Iterations;

do
{
	vxsquare = v.x * v.x;
	vysquare = v.y * v.y;

	v = float2(vxsquare - vysquare, v.x * v.y * 2) + offset;

	iteration++;

	if ((lastIteration == Iterations) && (vxsquare + vysquare) > 4.0)
	{
		lastIteration = iteration + 1;
	}
}
while (iteration < lastIteration);

return (float(iteration) - (log(log(sqrt(vxsquare + vysquare))) / log(2.0))) / float(Iterations);
}

float4 Mandelbrot_PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 v = (texCoord - 0.5) * Zoom * float2(1, Aspect) - Pan;
	float val = ComputeValue(v, v);
	return float4(sin(val * ColorScale.x), sin(val * ColorScale.y), sin(val * ColorScale.z), 1);
}

float4 Julia_PixelShader(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 v = (texCoord - 0.5) * Zoom * float2(1, Aspect) - Pan;
	float val = ComputeValue(v, JuliaSeed);
	return float4(sin(val * ColorScale.x), sin(val * ColorScale.y), sin(val * ColorScale.z), 1);
}

// see http://forums.create.msdn.com/forums/p/71866/438467.aspx
void SpriteVertexShader(inout float4 color    : COLOR0, 
                        inout float2 texCoord : TEXCOORD0, 
                        inout float4 position : SV_Position) 
{ 
    position = mul(position, MatrixTransform); 
} 

technique Mandelbrot
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 Mandelbrot_PixelShader();
    }
}

technique Julia
{
    pass Pass1
    {
		VertexShader = compile vs_3_0 SpriteVertexShader();
        PixelShader = compile ps_3_0 Julia_PixelShader();
    }
}
