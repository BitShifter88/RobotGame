﻿using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    class SunPannelPlugin : Plugin
    {
        private PluginDirection _dir;

        public SunPannelPlugin(PluginDirection dir)
        {
            _dir = dir;
            Size = 1;
            ComponentMaxHp = 100;
        }
        public override void Update(double dt)
        {
            Tank.AddPower((float)dt * 0.01f);
            base.Update(dt);
        }
        public override void Load(ContentManager content)
        {
            if (_dir == PluginDirection.Left)
                Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/sunPannel"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }
    }
}
