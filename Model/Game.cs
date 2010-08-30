using System;
using System.Collections.Generic;
using System.IO;
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
        public Effect effect;   //nie patrzeæ

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";

            Campaign = new Campaign();
            Camera=new Camera();
            UserInterface = new UserInterface(Campaign);
            UserInterfaceController = new UserInterfaceController(Camera, Campaign,UserInterface);
            
            Campaign.GameState = GameState.Initialize;

        }

        #region Subclasses

        public UserInterfaceController UserInterfaceController
        {
            get; set;
        }

        public Campaign Campaign
        {
            get; set;
        }

        public Display Display
        {
            get; set;
        }

        public AI AI
        { 
            get; set; 
        }

        public GameEventController GameEventController
        {
            get; set;
        }

        public GameInfo GameInfo
        {
            get; set;
        }

        public Camera Camera
        {
            get;
            set;
        }

        public UserInterface UserInterface
        {
            get;
            set;
        }
        #endregion



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Display = new Display(graphics, UserInterface, Camera, Campaign, effect);
            Campaign.GameState = GameState.MainMenu;
            this.IsMouseVisible = true;
            this.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            this.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            //this.GraphicsDevice.RenderState.DepthBufferEnable = false;
            this.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = Content.Load<Effect>("Resources/effects");
            UserInterface.LoadGraphics(this);
            Campaign.GameObjectFactory.LoadModels(this);
            Campaign.BuyUnit(GameObjectID.FireTruck);
            //damn...
            //Campaign.Mission.ObjectContainer.GameObjects.Add(
                //Campaign.GameObjectFactory.CreateGameObject(GameObjectID.SelectionRing));
            Campaign.SendToMission(Campaign.UnitContainer.Units[0]);

            Texture2D heightMap = Content.Load<Texture2D>("Resources/heightmap");
            Campaign.Mission.Board.LoadHeightData(heightMap);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
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
            UserInterfaceController.UpdateUserInterfaceState(gameTime);
            Campaign.Mission.ObjectContainer.UpdateGameObjects(); //temp

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
           
           Display.Draw(gameTime);
           base.Draw(gameTime);
        }
    }
}
