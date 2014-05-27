using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.MainGuns
{
    class StarterMainGun : MainGun
    {
        public StarterMainGun()
        {
            Size = 2;
        }
        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/MainGuns/mainGunStarter"));
            base.Load(content);
        }
    }
}
