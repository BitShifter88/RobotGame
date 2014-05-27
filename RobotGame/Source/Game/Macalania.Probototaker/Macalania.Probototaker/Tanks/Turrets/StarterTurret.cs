using Macalania.Probototaker.Tanks.Plugins;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    class StarterTurret : Turret
    {
        public StarterTurret()
        {
            Top = new Plugin[2];
            Buttom = new Plugin[2];
            Left = new Plugin[3];
            Right = new Plugin[3];
        }

        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Turrets/turretStarter"));
            base.Load(content);
        }
    }
}
