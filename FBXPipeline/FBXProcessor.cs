using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;


// TODO: replace these with the processor input and output types.


namespace FBXPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "FBXPipeline.FBX")]
    class FBXProcessor : ModelProcessor
    {
        public override ModelContent Process(
        NodeContent input, ContentProcessorContext context)
        {
            
            
                //foreach (NodeContent child in input.Children)
                //{
                //    MeshContent msh = child as MeshContent;
                //    foreach (GeometryContent content in msh.Geometry)
                //    {
                            
                        
                //    }
                        
                    
                    
                //}
                
            
            ModelContent mc = base.Process(input, context);
            foreach (ModelMeshContent mesh in mc.Meshes)
            {
                foreach (ModelMeshPartContent part in mesh.MeshParts)
                {
                    mesh.Tag = part.Material.Name;
                    
                }
            }
            return mc;
        }

        
      protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context) 
        { 
            
            EffectMaterialContent myMaterial = new EffectMaterialContent();
           // material.Name = "gownooooooooo";
            material.OpaqueData.Add("duupsko",15);
          Log(context,"KEYSY: "+material.OpaqueData.Keys.ToString());
            if (material is EffectMaterialContent) 
            { 
                EffectMaterialContent effectMaterialContent = (EffectMaterialContent)material; 
                
                
                Log(context, "Material is EffectMaterial! Using shader");
                 
                Log(context, effectMaterialContent.Effect.Filename.ToString()); 
 
                // remap effect 
                myMaterial.Effect = new ExternalReference<EffectContent>(effectMaterialContent.Effect.Filename); 
 
                // textures 
                foreach (KeyValuePair<string, ExternalReference<TextureContent>> pair in effectMaterialContent.Textures) 
                { 
 
                    string textureKey = pair.Key; 
                    ExternalReference<TextureContent> textureContent = pair.Value; 
 
                    if (!string.IsNullOrEmpty(textureContent.Filename)) 
                    { 
                        myMaterial.Textures.Add(textureKey, material.Textures[textureKey]); 
                        Log(context, "Set texture ‘{0}’ = {1}", textureKey, textureContent.Filename); 
                    } 
                    else 
                    { 
                        Log(context, "Failed! ‘{0}’ = {1}", textureKey, textureContent.Filename); 
                    } 
                }
                MaterialContent cm = base.ConvertMaterial(myMaterial, context);
                cm.OpaqueData.Add("ble","");
                
                return cm; 
            } 
            else if (material is BasicMaterialContent) 
            { 
                // create a BasicMaterialContent and use that to convert instead 
                BasicMaterialContent basicMaterial = (BasicMaterialContent)material;
  
                Log(context, basicMaterial.OpaqueData.ToString() + "Licznosc:" + basicMaterial.OpaqueData.Count.ToString() + " nazwa materialu " + basicMaterial.Name);
                foreach (KeyValuePair<string, object> data in basicMaterial.OpaqueData)
                {
                   Log(context,"DUPSKO: "+data.Key.ToString()+" __ " );
                }
              //  Log(context,basicMaterial.);
               // You can set textures for the effect to use 
                if (basicMaterial.Textures.Count > 0) 
                { 
                    Log(context, "Basic Material has a texture: {0}, name: {1}", basicMaterial.Name, basicMaterial.Texture.Name); 
                    
                    myMaterial.Textures.Add("DiffuseTexture", basicMaterial.Texture); 
                } 
                else 
                { 
                    Log(context, "No textures on {0}", basicMaterial.Name); 
                } 
                basicMaterial.OpaqueData.Add("dupsko",15);

                MaterialContent cm = base.ConvertMaterial(basicMaterial, context);
               
                return cm;
            } 
 
            else 
                throw new Exception("huh? this is very odd"); 
        } 
 
 
 

        private void Log(ContentProcessorContext context, string format, params object[] args) 
        { 
            string message = string.Format("CustomEffectModelProcessor: " + format, args); 
            context.Logger.LogImportantMessage(message); 
        } 
    } 

}