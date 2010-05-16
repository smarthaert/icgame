using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ICGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public enum GameState
    {
        Initialize,MainMenu,Campaign,Mission,Pause,Exit
    }

    public class Game : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        private Effect effect;
      

        UserInterfaceController userInterfaceController;
        

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Campaign = new Campaign();
            Camera=new Camera();
            
            Campaign.GameState = GameState.Initialize;

        }

        #region Subclasses

        public UserInterfaceController UserInterfaceController
        {
            get
            {
                return userInterfaceController;
            }
        }

        public Campaign Campaign
        {
            get;
            set;
        }

        public Display Display
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public AI AI
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public GameEventController GameEventController
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #endregion

        public GameInfo GameInfo
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            userInterfaceController = new UserInterfaceController(Camera,Campaign);



            base.Initialize();

            Campaign.GameState = GameState.MainMenu;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = Content.Load<Effect>("effects");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

            UserInterfaceController.UpdateInput();

            switch (Campaign.GameState)
            {
                case GameState.Initialize:
                    break;
                case GameState.MainMenu:
                    break;
                case GameState.Campaign:
                    break;
                case GameState.Mission:
                    break;
                case GameState.Pause:
                    break;
                case GameState.Exit:
                    this.Exit();
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //TYMCZASOWE
            VertexPositionColor [] vertices = new VertexPositionColor[3];
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f,300.0f);
            Matrix view = Camera.CameraMatrix;
            Matrix worldMatrix = Matrix.Identity;

            effect.Parameters["xView"].SetValue(view);
            effect.Parameters["xProjection"].SetValue(projection);
            effect.Parameters["xWorld"].SetValue(worldMatrix);
 
            vertices[0].Position = new Vector3(-0.5f, -0.5f, 0f);
            vertices[0].Color = Color.Red;
            vertices[1].Position = new Vector3(0, 0.5f, 0f);
            vertices[1].Color = Color.Green;
            vertices[2].Position = new Vector3(0.5f, -0.5f, 0f);
            vertices[2].Color = Color.Yellow;

            VertexDeclaration myVertexDeclaration = new VertexDeclaration(graphics.GraphicsDevice, VertexPositionColor.VertexElements);


            effect.CurrentTechnique = effect.Techniques["Colored"];
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();

                graphics.GraphicsDevice.VertexDeclaration = myVertexDeclaration;
                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1);

                pass.End();
            }
            effect.End();
            //TYMCZASOWE

            base.Draw(gameTime);
        }

        public Camera Camera
        {
            get;
            set;
        }

        public UserInterface UserInterface
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
