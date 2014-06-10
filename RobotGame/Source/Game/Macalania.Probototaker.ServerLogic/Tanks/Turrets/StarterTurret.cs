using Macalania.Probototaker.Tanks.Plugins;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    public class StarterTurret : Turret
    {
        public StarterTurret()
        {
            Top = new Plugin[2];
            Buttom = new Plugin[2];
            Left = new Plugin[3];
            Right = new Plugin[3];

            ExtraPixelsButtom = 20;
            ExtraPixelsSide = 10;
            ExtraPixelsTop = 20;
            StoredPower = 500;
            StoredHp = 500;
        }

        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Turrets/turretStarter"));
            Sprite.DepthLayer = 0.2f;
            base.Load(content);
        }
    }
}
