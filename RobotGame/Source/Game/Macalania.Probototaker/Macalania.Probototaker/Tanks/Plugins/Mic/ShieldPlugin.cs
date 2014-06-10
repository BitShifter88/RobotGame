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

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    public class ShieldPlugin : Plugin
    {
        private PluginDirection _dir;

        public ShieldPlugin(PluginDirection dir)
        {
            _dir = dir;
            Size = 1;
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

        public override bool Activate(Vector2 point, Tank target)
        {
            bool success = base.Activate(point, target);

            if (success)
            {
                Shield s = new Shield(RoomManager.Instance.GetActiveRoom(), Tank, 6000, 200);
                RoomManager.Instance.GetActiveRoom().AddGameObjectWhileRunning(s);
            }

            return success;
        }
    }
}
