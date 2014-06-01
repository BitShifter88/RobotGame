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
    class RocketStarterPlugin : Plugin
    {
        private PluginDirection _dir;

        public RocketStarterPlugin(PluginDirection dir)
        {
            _dir = dir;
            Size = 3;
        }
        public override void Load(ContentManager content)
        {
            if (_dir == PluginDirection.Right)
                Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/rocketStarterRight"));
            base.Load(content);
        }

        public override void Update(double dt)
        {
            base.Update(dt);

            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
        }
    }
}
