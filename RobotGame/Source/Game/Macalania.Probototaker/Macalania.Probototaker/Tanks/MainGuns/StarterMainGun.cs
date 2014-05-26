using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.MainGuns
{
    class StarterMainGun : MainGun
    {
        public void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Texture/Tanks/MainGuns/mainGunStarter"));
        }
    }
}
