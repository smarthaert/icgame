using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ICGame
{
    public class EffectController
    {
        private Game MainGame;

        public EffectController(Game mainGame)
        {
            MainGame = mainGame;
        }

        public void Update(GameTime gameTime)
        {
            foreach (GameObject missionObject in MainGame.MissionController.GetMissionObjects())
            {
                foreach(IObjectEffect objectEffect in missionObject.EffectList)
                {
                 //   if(objectEffect.IsActive)
                     objectEffect.Update(gameTime);
                }
            }
        }

        //Dev
        public void SetEffects(bool state)
        {
            foreach (GameObject missionObject in MainGame.MissionController.GetMissionObjects())
            {
                foreach (IObjectEffect objectEffect in missionObject.EffectList)
                {
                    objectEffect.IsActive = state;
                }
            }
        }

        public void AddEffectToAnObject(GameObject gameObject, string name) //TODO: ENUM!!!!!!!! (albo XML)
        {
            IObjectEffect gameEffect;
            //if (name == "water")
                gameEffect = new WaterEffect(gameObject,MainGame);
            gameEffect.IsActive = true;
            
            gameObject.EffectList.Add(gameEffect);
            
        }

    }

}
