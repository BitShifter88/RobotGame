using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    public class BatteryPlugin : Plugin
    {
        private PluginDirection _dir;

        public BatteryPlugin(PluginDirection dir)
        {
            _dir = dir;
            Size = 1;

            StoredPower = 500;
            ComponentMaxHp = 100;
        }
        public override void Load(ContentManager content)
        {
            if (_dir == PluginDirection.Left)
                Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/battery"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }

    }
}
