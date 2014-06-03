﻿using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    class AmorPlugin : Plugin
    {
        private PluginDirection _dir;

        public AmorPlugin(PluginDirection dir)
        {
            _dir = dir;
            Size = 1;
            ComponentMaxHp = 500;
            CompType = TankComponentType.Amor;
        }
        public override void Load(ContentManager content)
        {
            if (_dir == PluginDirection.Left)
                Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/amorStarterLeft"));
            if (_dir == PluginDirection.Right)
                Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/amorStarterRight"));
            if (_dir == PluginDirection.Buttom)
                Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/amorStarterBottom"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }


    }
}
