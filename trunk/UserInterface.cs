using System;
using System.Collections.Generic;
using System.Text;

namespace ICGame
{
    public class UserInterface
    {
        public UserInterface(Campaign campaign)
        {
            Campaign = campaign;
        }

        public Campaign Campaign
        {
            get; set;
        }
    }
}
