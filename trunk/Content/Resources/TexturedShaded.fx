
struct VertexToPixel
{
    float4 Position   	: POSITION;    
    float4 Color		: COLOR0;
    float LightingFactor: TEXCOORD0;
    float2 TextureCoords: TEXCOORD1;
    float calcRS        : TEXCOORD2;
    float4 clipPlanes	: TEXCOORD3;
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

//------- XNA-to-HLSL variables --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xLightDirection;
float3 xCameraPosition;
float3 xDiffuseColor;
float xDiffuseFactor;
float xOpacity;
float xTransparency;
float xAmbient;
float xSpecularFactor;
float3 xSpecularColor;
bool xEnableLighting;
bool xHasTexture;
float4 xClipPlane0;
bool xClipPlanes;

float calcRS;
//------- Texture Samplers --------

Texture xTexture;

sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: NotRlyTexturedShaded --------

VertexToPixel NotRlyTexturedShadedVS( float4 inPos : POSITION, float3 inNormal: NORMAL)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);

	if (xClipPlanes)
	{
		Output.clipPlanes.x = dot(mul(inPos,xWorld), xClipPlane0); 
		Output.clipPlanes.y = 0; 
		Output.clipPlanes.z = 0; 
		Output.clipPlanes.w = 0; 
	}
    
	Output.Position = mul(inPos, preWorldViewProjection);	
		
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
		
	float4 V = normalize(float4(xCameraPosition.x,xCameraPosition.y,xCameraPosition.z,0)-mul(xView,mul(xWorld,inPos)));
	float4 R = normalize(reflect(float4(xLightDirection,0),normalize(mul(xView,mul(xWorld,float4(inNormal.x,inNormal.y,inNormal.z,0))))));
	
	float RV = saturate(dot(R,V));
	
	Output.calcRS=pow(RV,1)*xSpecularFactor;
	Output.LightingFactor = 1;
	
	
	if(!xHasTexture)
	Output.Color=float4(xDiffuseColor,0)*xDiffuseFactor;
	else
	Output.Color=float4(1,1,1,0);
	if (xEnableLighting)
		Output.LightingFactor=saturate(dot(Normal, -xLightDirection))+xAmbient;
		//Output.LightingFactor = saturate(dot(Normal, -xLightDirection));
		
	
	
    
	return Output;    
}

PixelToFrame NotRlyTexturedShadedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;
    
	if (xClipPlanes)
		clip(PSIn.clipPlanes); 

	//Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
	
	Output.Color = 1;

	Output.Color.rgb*=PSIn.Color;
		
	Output.Color +=float4(PSIn.calcRS*xSpecularColor,0);
	Output.Color.rgb *= saturate(PSIn.LightingFactor);

	Output.Color.a=xTransparency;

	return Output;
}

technique NotRlyTexturedShaded_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 NotRlyTexturedShadedVS();
		PixelShader  = compile ps_2_0 NotRlyTexturedShadedPS();
	}
}

technique NotRlyTexturedShaded
{
	pass Pass0
	{   
		//VertexShader = compile vs_1_1 TexturedShadedVS();
		//PixelShader  = compile ps_1_1 TexturedShadedPS();
		//CONV
		VertexShader = compile vs_2_0 NotRlyTexturedShadedVS();
		PixelShader  = compile ps_2_0 NotRlyTexturedShadedPS();
	}
}

//------- Technique: TexturedShaded --------

VertexToPixel TexturedShadedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);

	if (xClipPlanes)
	{
		Output.clipPlanes.x = dot(mul(inPos,xWorld), xClipPlane0); 
		Output.clipPlanes.y = 0; 
		Output.clipPlanes.z = 0; 
		Output.clipPlanes.w = 0; 
	}
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	if(xHasTexture)
		Output.TextureCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
		
	float4 V = normalize(float4(xCameraPosition.x,xCameraPosition.y,xCameraPosition.z,0)-mul(xView,mul(xWorld,inPos)));
	float4 R = normalize(reflect(float4(xLightDirection,0),normalize(mul(xView,mul(xWorld,float4(inNormal.x,inNormal.y,inNormal.z,0))))));
	
	float RV = saturate(dot(R,V));
	
	Output.calcRS=pow(RV,1)*xSpecularFactor;
	Output.LightingFactor = 1;
	
	
	if(!xHasTexture)
	Output.Color=float4(xDiffuseColor,0)*xDiffuseFactor;
	else
	Output.Color=float4(1,1,1,0);
	if (xEnableLighting)
		Output.LightingFactor=saturate(dot(Normal, -xLightDirection))+xAmbient;
		//Output.LightingFactor = saturate(dot(Normal, -xLightDirection));
		
	
	
    
	return Output;    
}

PixelToFrame TexturedShadedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		

	if (xClipPlanes)
		clip(PSIn.clipPlanes); 

    if(xHasTexture)
		Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
	else
		Output.Color = 1;
	Output.Color.rgb*=PSIn.Color;
		
	Output.Color +=float4(PSIn.calcRS*xSpecularColor,0);
	Output.Color.rgb *= saturate(PSIn.LightingFactor);

	Output.Color.a=xTransparency;

	return Output;
}

technique TexturedShaded_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 TexturedShadedVS();
		PixelShader  = compile ps_2_0 TexturedShadedPS();
	}
}

technique TexturedShaded
{
	pass Pass0
	{   
		//VertexShader = compile vs_1_1 TexturedShadedVS();
		//PixelShader  = compile ps_1_1 TexturedShadedPS();
		//CONV
		VertexShader = compile vs_2_0 TexturedShadedVS();
		PixelShader  = compile ps_2_0 TexturedShadedPS();
	}
}

//------- Technique: BlueHologram --------

VertexToPixel BlueHologramVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	Output.TextureCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
		
	float4 V = normalize(float4(xCameraPosition.x,xCameraPosition.y,xCameraPosition.z,0)-mul(xView,mul(xWorld,inPos)));
	float4 R = normalize(reflect(float4(xLightDirection,0),normalize(mul(xView,mul(xWorld,float4(inNormal.x,inNormal.y,inNormal.z,0))))));
	
	float RV = saturate(dot(R,V));
	
	
	Output.LightingFactor = 1;
	
	

	Output.Color=float4(0,0,1,0);
	
	Output.LightingFactor = saturate(dot(Normal, -xLightDirection) + xAmbient);
		
	
	
    
	return Output;    
}

PixelToFrame BlueHologramPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		

    Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
	Output.Color.rgb *= PSIn.Color;
	Output.Color.rgb *= PSIn.LightingFactor;
	
	Output.Color.a=xTransparency-0.1f;

	return Output;
}



technique BlueHologram
{
	pass Pass0
	{   
		VertexShader = compile vs_1_1 BlueHologramVS();
		PixelShader  = compile ps_2_0 BlueHologramPS();
	}
}



VertexToPixel NotRlyBlueHologramVS( float4 inPos : POSITION, float3 inNormal: NORMAL)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
		
	float4 V = normalize(float4(xCameraPosition.x,xCameraPosition.y,xCameraPosition.z,0)-mul(xView,mul(xWorld,inPos)));
	float4 R = normalize(reflect(float4(xLightDirection,0),normalize(mul(xView,mul(xWorld,float4(inNormal.x,inNormal.y,inNormal.z,0))))));
	
	float RV = saturate(dot(R,V));
	
	
	Output.LightingFactor = 1;
	
	

	Output.Color=float4(0,0,1,0);
	
	Output.LightingFactor = saturate(dot(Normal, -xLightDirection) + xAmbient);
		
	
	
    
	return Output;    
}



PixelToFrame NotRlyBlueHologramPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		

    
	Output.Color.rgb = PSIn.Color;
	Output.Color.rgb *= PSIn.LightingFactor;
	
	Output.Color.a=xTransparency-0.1f;

	return Output;
}



technique NotRlyBlueHologram
{
	pass Pass0
	{   
		VertexShader = compile vs_1_1 NotRlyBlueHologramVS();
		PixelShader  = compile ps_2_0 NotRlyBlueHologramPS();
	}
}