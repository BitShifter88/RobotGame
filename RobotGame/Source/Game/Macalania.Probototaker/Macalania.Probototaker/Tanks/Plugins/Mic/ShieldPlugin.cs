using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
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
            MaxCooldown = 500;
            PowerUsage = 400;
        }
        public override void Load(ContentManager content)
        {
            if (_dir == PluginDirection.Left)
                Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/shieldLeft"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }

        public override bool Activate(Vector2 point, Tank target)
        {
            bool success = base.Activate(point, target);

            if (success)
            {
                Shield s = new Shield(Tank, 3000);
                YunaGameEngine.Instance.GetActiveRoom().AddGameObjectWhileRunning(s);
            }

            return success;
        }
    }
}
