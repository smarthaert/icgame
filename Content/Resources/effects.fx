
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
bool xShowNormals;
bool xHasTexture;
float xOvercast;
float xTime;
float4 xClipPlane0;
bool xClipPlanes;

float calcRS;
//------- Texture Samplers --------

Texture xTexture;

sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture xTexture0;

sampler TextureSampler0 = sampler_state { texture = <xTexture0> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture1;

sampler TextureSampler1 = sampler_state { texture = <xTexture1> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = wrap; AddressV = wrap;};

Texture xTexture2;

sampler TextureSampler2 = sampler_state { texture = <xTexture2> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture xTexture3;

sampler TextureSampler3 = sampler_state { texture = <xTexture3> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

float xWaveLength;
float xWaveHeight;
float3 xWindDirection;
float xWindForce;

//--------Water Reflections-----------------

float4x4 xReflectionView;
Texture xReflectionMap;

sampler ReflectionSampler = sampler_state { texture = <xReflectionMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};
Texture xRefractionMap;

sampler RefractionSampler = sampler_state { texture = <xRefractionMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture xWaterBumpMap;

sampler WaterBumpMapSampler = sampler_state { texture = <xWaterBumpMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

//------- Technique: Pretransformed --------

VertexToPixel PretransformedVS( float4 inPos : POSITION, float4 inColor: COLOR)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);
	
	//Output.Position = inPos;
	Output.Color = inColor;
    
	return Output;    
}

PixelToFrame PretransformedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = PSIn.Color;

	return Output;
}

technique Pretransformed_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 PretransformedVS();
		PixelShader  = compile ps_2_0 PretransformedPS();
	}
}

technique Pretransformed
{
	pass Pass0
	{   
		//VertexShader = compile vs_1_1 PretransformedVS();
		//PixelShader  = compile ps_1_1 PretransformedPS();
		//CONV
		VertexShader = compile vs_2_0 PretransformedVS();
		PixelShader  = compile ps_2_0 PretransformedPS();
	}
}

//------- Technique: Colored --------

VertexToPixel ColoredVS( float4 inPos : POSITION, float4 inColor: COLOR, float3 inNormal: NORMAL)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);
	Output.Color = inColor;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = saturate(dot(Normal, -xLightDirection));
    
	return Output;    
}

PixelToFrame ColoredPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
    
	Output.Color = PSIn.Color;
	Output.Color.rgb *= saturate(PSIn.LightingFactor + xAmbient);
	
	return Output;
}

technique Colored_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 ColoredVS();
		PixelShader  = compile ps_2_0 ColoredPS();
	}
}

technique Colored
{
	pass Pass0
	{   
		//VertexShader = compile vs_1_1 ColoredVS();
		//PixelShader  = compile ps_1_1 ColoredPS();
		//CONV
		VertexShader = compile vs_2_0 ColoredVS();
		PixelShader  = compile ps_2_0 ColoredPS();
	}
}

//------- Technique: Textured --------

VertexToPixel TexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	Output.TextureCoords = inTexCoords;
	
	float3 Normal = normalize(mul(normalize(inNormal), xWorld));	
	Output.LightingFactor = 1;
	if (xEnableLighting)
		Output.LightingFactor = saturate(dot(Normal, -xLightDirection));
    
	return Output;    
}

PixelToFrame TexturedPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
	Output.Color.rgb *= saturate(PSIn.LightingFactor + xAmbient);

	return Output;
}

technique Textured_2_0
{
	pass Pass0
	{   
		VertexShader = compile vs_2_0 TexturedVS();
		PixelShader  = compile ps_2_0 TexturedPS();
	}
}

technique Textured
{
	pass Pass0
	{   
		//VertexShader = compile vs_1_1 TexturedVS();
		//PixelShader  = compile ps_1_1 TexturedPS();
		//CONV
		VertexShader = compile vs_1_1 TexturedVS();
		PixelShader  = compile ps_2_0 TexturedPS();
	}
}

//------- Technique: PointSprites --------

struct SpritesVertexOut
{
    float4 Position		: POSITION0;
    float1 Size 		: PSIZE;
};

struct SpritesPixelIn
{
    #ifdef XBOX
		float4 TexCoord : SPRITETEXCOORD;
	#else
		float2 TexCoord : TEXCOORD0;
	#endif
};

SpritesVertexOut PointSpritesVS (float4 Position : POSITION, float4 Color : COLOR0, float1 Size : PSIZE)
{
    SpritesVertexOut Output = (SpritesVertexOut)0;
     
    float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection); 
    Output.Position = mul(Position, preWorldViewProjection);    
    Output.Size = 1/(pow(Output.Position.z,2)+1	) * Size;
    
    return Output;    
}

PixelToFrame PointSpritesPS(SpritesPixelIn PSIn)
{ 
    PixelToFrame Output = (PixelToFrame)0;    

    #ifdef XBOX
		float2 texCoord = abs(PSIn.TexCoord.zw);
    #else
		float2 texCoord = PSIn.TexCoord.xy;
    #endif

    Output.Color = tex2D(TextureSampler, texCoord);
    
    return Output;
}

technique PointSprites_2_0
{
	pass Pass0
	{   
		//PointSpriteEnable = true;
		//CONV
		VertexShader = compile vs_2_0 PointSpritesVS();
		PixelShader  = compile ps_2_0 PointSpritesPS();
	}
}

technique PointSprites
{
	pass Pass0
	{   
		//PointSpriteEnable = true;
		//CONV
		//VertexShader = compile vs_1_1 PointSpritesVS();
		//PixelShader  = compile ps_1_1 PointSpritesPS();
		//CONV
		VertexShader = compile vs_1_1 PointSpritesVS();
		PixelShader  = compile ps_2_0 PointSpritesPS();
	}
}

//------- Technique: PerlinNoise --------
 struct PNVertexToPixel
 {    
     float4 Position         : POSITION;
     float2 TextureCoords    : TEXCOORD0;
 };
 
 struct PNPixelToFrame
 {
     float4 Color : COLOR0;
 };
 
 PNVertexToPixel PerlinVS(float4 inPos : POSITION, float2 inTexCoords: TEXCOORD)
 {    
     PNVertexToPixel Output = (PNVertexToPixel)0;
     
     Output.Position = inPos;
     Output.TextureCoords = inTexCoords;
     
     return Output;    
 }
 
 PNPixelToFrame PerlinPS(PNVertexToPixel PSIn)
 {
     PNPixelToFrame Output = (PNPixelToFrame)0;    
     
     float2 move = float2(0,1);
     float4 perlin = tex2D(TextureSampler, (PSIn.TextureCoords)+xTime*move)/2;
     perlin += tex2D(TextureSampler, (PSIn.TextureCoords)*2+xTime*move)/4;
     perlin += tex2D(TextureSampler, (PSIn.TextureCoords)*4+xTime*move)/8;
     perlin += tex2D(TextureSampler, (PSIn.TextureCoords)*8+xTime*move)/16;
     perlin += tex2D(TextureSampler, (PSIn.TextureCoords)*16+xTime*move)/32;
     perlin += tex2D(TextureSampler, (PSIn.TextureCoords)*32+xTime*move)/32;    
     
     Output.Color.rgb = 1.0f-pow(abs(perlin.r), xOvercast)*2.0f;
     Output.Color.a =1;
 
     return Output;
 }
 
 technique PerlinNoise
 {
     pass Pass0
     {
         VertexShader = compile vs_1_1 PerlinVS();
         PixelShader = compile ps_2_0 PerlinPS();
     }
 }


 //------- Technique: SkyDome --------
 struct SDVertexToPixel
 {    
     float4 Position         : POSITION;
     float2 TextureCoords    : TEXCOORD0;
     float4 ObjectPosition    : TEXCOORD1;
 };
 
 struct SDPixelToFrame
 {
     float4 Color : COLOR0;
 };
 
 SDVertexToPixel SkyDomeVS( float4 inPos : POSITION, float2 inTexCoords: TEXCOORD0)
 {    
     SDVertexToPixel Output = (SDVertexToPixel)0;
     float4x4 preViewProjection = mul (xView, xProjection);
     float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
     
     Output.Position = mul(inPos, preWorldViewProjection);
     Output.TextureCoords = inTexCoords;
     Output.ObjectPosition = inPos;
     
     return Output;    
 }
 
 SDPixelToFrame SkyDomePS(SDVertexToPixel PSIn)
 {
     SDPixelToFrame Output = (SDPixelToFrame)0;        
 
     float4 topColor = float4(0.3f, 0.3f, 0.8f, 1);    
     float4 bottomColor = 1;    
     
     float4 baseColor = lerp(bottomColor, topColor, saturate((PSIn.ObjectPosition.y)/0.4f));
     float4 cloudValue = tex2D(TextureSampler, PSIn.TextureCoords).r;
     
     Output.Color = lerp(baseColor,1, cloudValue);        
 
     return Output;
 }
 
 technique SkyDome
 {
     pass Pass0
     {
         VertexShader = compile vs_1_1 SkyDomeVS();
         PixelShader = compile ps_2_0 SkyDomePS();
     }
 }


//------- Technique: Multitextured --------
struct MTVertexToPixel
{
    float4 Position         : POSITION;    
    float4 Color            : COLOR0;
    float3 Normal            : TEXCOORD0;
    float2 TextureCoords    : TEXCOORD1;
    float4 LightDirection    : TEXCOORD2;
    float4 TextureWeights    : TEXCOORD3;
    float4 clipPlanes		: TEXCOORD4;
};

struct MTPixelToFrame
{
    float4 Color : COLOR0;
};

MTVertexToPixel MultiTexturedVS( float4 inPos : POSITION, float3 inNormal: NORMAL, float2 inTexCoords: TEXCOORD0, float4 inTexWeights: TEXCOORD1)
{    
    MTVertexToPixel Output = (MTVertexToPixel)0;
	
    float4x4 preViewProjection = mul (xView, xProjection);
    float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	if (xClipPlanes)
	{
		Output.clipPlanes.x = dot(inPos, xClipPlane0); 
		Output.clipPlanes.y = 0; 
		Output.clipPlanes.z = 0; 
		Output.clipPlanes.w = 0; 
	}
    Output.Position = mul(inPos, preWorldViewProjection);
    Output.Normal = mul(normalize(inNormal), xWorld);
    Output.TextureCoords = inTexCoords;
    Output.LightDirection.xyz = -xLightDirection;
    Output.LightDirection.w = 1;    
    Output.TextureWeights = inTexWeights;
    
    return Output;    
}

MTPixelToFrame MultiTexturedPS(MTVertexToPixel PSIn)
{
    MTPixelToFrame Output = (MTPixelToFrame)0;
    
	if (xClipPlanes)
		clip(PSIn.clipPlanes); 

    float lightingFactor = 1;
    if (xEnableLighting)
        lightingFactor = saturate(saturate(dot(PSIn.Normal, PSIn.LightDirection)) + xAmbient);
        
    Output.Color = tex2D(TextureSampler0, PSIn.TextureCoords)*PSIn.TextureWeights.x;
    Output.Color += tex2D(TextureSampler1, PSIn.TextureCoords)*PSIn.TextureWeights.y;
    Output.Color += tex2D(TextureSampler2, PSIn.TextureCoords)*PSIn.TextureWeights.z;
    Output.Color += tex2D(TextureSampler3, PSIn.TextureCoords)*PSIn.TextureWeights.w;    
        
   // Output.Color *= lightingFactor;
    
    return Output;
}

technique MultiTextured
{
    pass Pass0
    {
        VertexShader = compile vs_1_1 MultiTexturedVS();
        PixelShader = compile ps_2_0 MultiTexturedPS();
    }
}



//------- Technique: Water --------

struct WVertexToPixel
{
    float4 Position                 : POSITION;
    float4 ReflectionMapSamplingPos    : TEXCOORD1;
    float2 BumpMapSamplingPos        : TEXCOORD2;
    float4 RefractionMapSamplingPos : TEXCOORD3;
    float4 Position3D                : TEXCOORD4;
};

struct WPixelToFrame
{
    float4 Color : COLOR0;
};

WVertexToPixel WaterVS(float4 inPos : POSITION, float2 inTex: TEXCOORD)
{    
    WVertexToPixel Output = (WVertexToPixel)0;

    float4x4 preViewProjection = mul (xView, xProjection);
    float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    float4x4 preReflectionViewProjection = mul (xReflectionView, xProjection);
    float4x4 preWorldReflectionViewProjection = mul (xWorld, preReflectionViewProjection);

    Output.Position = mul(inPos, preWorldViewProjection);
    Output.ReflectionMapSamplingPos = mul(inPos, preWorldReflectionViewProjection);

	float3 windDir = normalize(xWindDirection);    
	float3 perpDir = cross(xWindDirection, float3(0,1,0));

	float ydot = dot(inTex, xWindDirection.xz);
	float xdot = dot(inTex, perpDir.xz);
	float2 moveVector = float2(xdot, ydot);

	moveVector += float2(0, xTime*xWindForce);

	Output.BumpMapSamplingPos = moveVector/xWaveLength;
	Output.RefractionMapSamplingPos = mul(inPos, preWorldViewProjection);
	Output.Position3D = mul(inPos, xWorld);

    return Output;
}

WPixelToFrame WaterPS(WVertexToPixel PSIn)
{
    WPixelToFrame Output = (WPixelToFrame)0;      
	       
    float2 ProjectedTexCoords;
    ProjectedTexCoords.x = PSIn.ReflectionMapSamplingPos.x/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;
    ProjectedTexCoords.y = -PSIn.ReflectionMapSamplingPos.y/PSIn.ReflectionMapSamplingPos.w/2.0f + 0.5f;

	float4 bumpColor = tex2D(WaterBumpMapSampler, PSIn.BumpMapSamplingPos);
	float2 perturbation = xWaveHeight*(bumpColor.rg - 0.5f)*2.0f;
	float2 perturbatedTexCoords = ProjectedTexCoords + perturbation;    

	float4 reflectiveColor = tex2D(ReflectionSampler, perturbatedTexCoords);

	float2 ProjectedRefrTexCoords;
	ProjectedRefrTexCoords.x = PSIn.RefractionMapSamplingPos.x/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;
	ProjectedRefrTexCoords.y = -PSIn.RefractionMapSamplingPos.y/PSIn.RefractionMapSamplingPos.w/2.0f + 0.5f;    
	float2 perturbatedRefrTexCoords = ProjectedRefrTexCoords + perturbation;    
	float4 refractiveColor = tex2D(RefractionSampler, perturbatedRefrTexCoords);

	float3 eyeVector = normalize(xCameraPosition - PSIn.Position3D);

	float3 normalVector = float3(0,1,0);

	float fresnelTerm = saturate(dot(eyeVector, normalVector));

	  
     float4 combinedColor = lerp(reflectiveColor, refractiveColor, fresnelTerm);
     
     float4 dullColor = float4(0.3f, 0.3f, 0.5f, 1.0f);
     
     Output.Color = lerp(combinedColor, dullColor, 0.2f);

    //Output.Color = tex2D(ReflectionSampler, ProjectedTexCoords);    
    
    return Output;
}

technique Water
{
    pass Pass0
    {
        VertexShader = compile vs_1_1 WaterVS();
        PixelShader = compile ps_2_0 WaterPS();
    }
}