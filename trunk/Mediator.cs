using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public static class Mediator
    {
        private static Game game;

        public static Game Game
        {
            get
            {
                return game;
            }
            set
            {
                game = value;
            }
        }

        private static Display display;
        private static ICGame.AI ai;
        private static ICGame.Board board;
        private static ICGame.Camera camera;
        private static ICGame.Campaign campaign;
        private static ICGame.GameEventController gameEventController;
        private static ICGame.GameInfo gameInfo;
        private static ICGame.GameObjectContainer gameObjectContainer;
        private static ICGame.GameObjectFactory gameObjectFactory;
        private static ICGame.Mission mission;
        private static ICGame.UnitContainer unitContainer;
        private static ICGame.UserInterface userInterface;
        private static ICGame.UserInterfaceController userInterfaceController;
    }
}
