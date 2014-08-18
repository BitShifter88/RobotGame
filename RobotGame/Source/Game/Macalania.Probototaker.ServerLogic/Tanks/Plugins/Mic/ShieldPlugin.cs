using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macalania.Probototaker.Effects;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Macalania.Probototaker.Tanks.Turrets;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    public class ShieldPlugin : TurretModule
    {
        private PluginDirection _dir;

        public ShieldPlugin(PluginDirection dir, Room room)
            : base(PluginType.Shield, room)
        {
            _dir = dir;
            MaxCooldown = 500;
            PowerUsage = 400;
            ComponentMaxHp = 100;
        }
        public override void Load(ResourceManager content)
        {
            if (_dir == PluginDirection.Left)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/shieldLeft"));
            if (_dir == PluginDirection.Buttom)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/shieldBottom"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }

        public override bool Activate(Vector2 point, Tank target, Random activationRandom)
        {
            bool success = base.Activate(point, target, activationRandom);

            if (success)
            {
                Shield s = new Shield(Room, Tank, 6000, 200);
                Room.AddGameObjectWhileRunning(s);
            }

            return success;
        }
    }
}
