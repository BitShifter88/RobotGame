﻿using Macalania.Probototaker.Tanks.Plugins;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    class BigTurret : Turret
    {
        public BigTurret()
        {
            Top = new Plugin[3];
            Buttom = new Plugin[3];
            Left = new Plugin[3];
            Right = new Plugin[3];

            ExtraPixelsButtom = 20;
            ExtraPixelsSide = 10;
            ExtraPixelsTop = 20;

            StoredPower = 500;
        }

        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Turrets/turretBig"));
            Sprite.DepthLayer = 0.2f;
            base.Load(content);
        }
    }
}
