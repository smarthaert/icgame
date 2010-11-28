using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ICGame
{
    public class EffectFactory
    {
        private static Game MainGame;

        public EffectFactory(Game game)
        {
            MainGame = game;
        }

        public static IObjectEffect GetEffectByName(string name)        //TODO: ENUM/plik
        {
            if(MainGame == null)
            {
                throw new NullReferenceException("EffectFactory was not initialized");
            }

            IObjectEffect gameEffect;
            if (name == "water")
                gameEffect = new WaterEffect(MainGame);
            else if (name == "fire")
            {
                gameEffect = new FireEffect(MainGame);
                gameEffect.IsActive = true;
            }
            else
            {
                gameEffect = new FireSmokeEffect(MainGame);
                gameEffect.IsActive = true;
            }

            return gameEffect;
        }
    }
}
