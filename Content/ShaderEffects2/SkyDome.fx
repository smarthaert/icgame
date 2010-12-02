//------- XNA-to-HLSL variables --------
float4x4 xWorldViewProjection;
bool xEnableLighting;
float xOvercast;
float xTime;
bool xSetAlpha;
float xAlpha;

float calcRS;
//------- Texture Samplers --------

Texture xTexture;

sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

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
     
     Output.Position = mul(inPos, xWorldViewProjection);
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
 
	if(xSetAlpha)
		Output.Color.a = xAlpha;

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
