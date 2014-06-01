using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    class ShieldPlugin : Plugin
    {
        private PluginDirection _dir;

        public ShieldPlugin(PluginDirection dir)
        {
            _dir = dir;
            Size = 1;
        }
        public override void Load(ContentManager content)
        {
            if (_dir == PluginDirection.Left)
                Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/shieldLeft"));
            base.Load(content);
        }
    }
}
