﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ICGame
{
    public class MissionController
    {
        private Game MainGame;

        public MissionController(Game game)
        {
            MainGame = game;
        }

        public Mission Mission
        {
            get; set;
        }

        public void UpdateMission(GameTime gameTime)
        {
            Mission.Update(gameTime);
           
            
        }
        
        public void StartMission()
        {
            Mission = new Mission(MainGame);
            
        }

        public void LoadMissionData(GameObjectFactory gameObjectFactory)
        {
            Microsoft.Xna.Framework.Content.ContentManager contentManager = MainGame.Content;
            Texture2D heightMap = contentManager.Load<Texture2D>("Resources/heightmap");

            Mission.Board.LoadTextures(contentManager.Load<Texture2D>("Texture2D/Board/grass"),
                contentManager.Load<Texture2D>("Texture2D/Board/rock"),
                contentManager.Load<Texture2D>("Texture2D/Board/sand"),
                contentManager.Load<Texture2D>("Texture2D/Board/snow")
                );
            Mission.Board.PrepareSkyDome(MainGame.Content.Load<Model>("Resources/dome"), MainGame.effect, MainGame.GraphicsDevice);
            Mission.Board.LoadHeightData(heightMap);
            Mission.LoadObjects(gameObjectFactory);
        }

        public List<GameObject> GetMissionObjects()
        {
            return Mission.ObjectContainer.GameObjects;
        }

        public BoardDrawer GetBoardDrawer()
        {
            return Mission.Board.GetDrawer();
        }

        public GameObject GetActiveObject()
        {
            return GetMissionObjects()[0];
        }

        public bool CheckSelection(int x, int y, Camera camera, Matrix projection, GraphicsDevice gd)
        {
            bool selected = Mission.ObjectContainer.CheckSelection(x, y, camera, projection, gd);



            return selected;
        }
    }
}