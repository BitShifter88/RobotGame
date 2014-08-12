using Macalania.Probototaker.Tanks.Turrets;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    public class SunPannelNew : TurretModule
    {
        private PluginDirection _dir;

        public SunPannelNew(PluginDirection dir, Room room)
            : base(PluginType.SunPannel, room)
        {
            _dir = dir;
            PowerRegen = 1;
            ComponentMaxHp = 100;
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        public override void Load(ResourceManager content)
        {
            if (_dir == PluginDirection.Left)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/sunPannel"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }
    }
}
