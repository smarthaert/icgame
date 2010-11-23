
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

//------- XNA-to-HLSL variables --------
float4x4 xWorld;
float4x4 xWorldViewProjection;
float4x4 xWorldReflectionViewProjection;
float3 xLightDirection;
float3 xCameraPosition;
float xOvercast;
float xTime;

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

//------- Technique: Water --------


WVertexToPixel WaterVS(float4 inPos : POSITION, float2 inTex: TEXCOORD)
{    
    WVertexToPixel Output = (WVertexToPixel)0;

    //float4x4 preViewProjection = mul (xView, xProjection);
    //float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    //float4x4 preReflectionViewProjection = mul (xReflectionView, xProjection);
    //float4x4 preWorldReflectionViewProjection = mul (xWorld, preReflectionViewProjection);

    Output.Position = mul(inPos, xWorldViewProjection);
    Output.ReflectionMapSamplingPos = mul(inPos, xWorldReflectionViewProjection);

	float3 windDir = normalize(xWindDirection);    
	float3 perpDir = cross(xWindDirection, float3(0,1,0));

	float ydot = dot(inTex, xWindDirection.xz);
	float xdot = dot(inTex, perpDir.xz);
	float2 moveVector = float2(xdot, ydot);

	moveVector += float2(0, xTime*xWindForce);

	Output.BumpMapSamplingPos = moveVector/xWaveLength;
	//Output.RefractionMapSamplingPos = mul(inPos, xWorldViewProjection);
	Output.RefractionMapSamplingPos = Output.Position;
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

	float3 normalVector = (bumpColor.rbg-0.5f)*2.0f;

	float fresnelTerm = saturate(dot(eyeVector, normalVector));

	  
     float4 combinedColor = lerp(reflectiveColor, refractiveColor, fresnelTerm);
     
     float4 dullColor = float4(0.3f, 0.3f, 0.5f, 1.0f);
     
     Output.Color = lerp(combinedColor, dullColor, 0.2f);

	 float3 reflectionVector = -reflect(xLightDirection, normalVector);
     float specular = dot(normalize(reflectionVector), normalize(eyeVector));
     specular = pow(specular, 256);        
     Output.Color.rgb += specular;

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