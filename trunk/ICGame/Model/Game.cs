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
        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        SpriteBatch spriteBatch; 
        public Effect effect;   //nie patrzeæ

        public Game()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";

            Campaign = new Campaign();

            
            UserInterface = new UserInterface();

            MissionController = new MissionController(this);
            CampaignController = new CampaignController(this, MissionController);

            Camera = new Camera(new Vector3(244, 0, 154),MissionController);

            UserInterfaceController = new UserInterfaceController(Camera, CampaignController,UserInterface);

        
        
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

        public MissionController MissionController
        {
            get; set;
        }

        public CampaignController CampaignController
        {
            get; set;
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            CampaignController.StartCampaign();
            CampaignController.CampaignState = GameState.MainMenu;

            Display = new Display(GraphicsDeviceManager, UserInterface, Camera, CampaignController, effect);
            
            

            IsMouseVisible = true;

            UserInterfaceController.InitializeUserInterface(this);

            //Alpha blending
          //  GraphicsDevice.RenderState.AlphaBlendEnable = true;
          //  GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
          //GraphicsDevice.RenderState.DepthBufferEnable = false;
          //  GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            
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

         
            UserInterfaceController.UpdateUserInterfaceState(gameTime);
            MissionController.UpdateMission(gameTime);
            
            switch (CampaignController.CampaignState)
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
