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
    public class AmorPlugin : Plugin
    {
        private PluginDirection _dir;

        public AmorPlugin(PluginDirection dir, Room room)
            : base(PluginType.Amor, room)
        {
            _dir = dir;
            Size = 1;
            ComponentMaxHp = 500;
            AmorPoints = 5;
            CompType = TankComponentType.Amor;
        }
        public override void Load(ResourceManager content)
        {
            if (_dir == PluginDirection.Left)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/amorStarterLeft"));
            if (_dir == PluginDirection.Right)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/amorStarterRight"));
            if (_dir == PluginDirection.Buttom)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/amorStarterBottom"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }


    }
}
