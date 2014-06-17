using Macalania.Probototaker.Items;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    class MineLayerPlugin : Plugin
    {
        public MineLayerPlugin()
        {
            Size = 3;
            OriginOfset = new Vector2(0, 30);
            MaxCooldown = 5000;
            ComponentMaxHp = 100;
        }

        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/artilery"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }

        protected virtual void LayMine()
        {
            StarterMine mine = new StarterMine(RoomManager.Instance.GetActiveRoom());
            RoomManager.Instance.GetActiveRoom().AddGameObjectWhileRunning(mine);
            Vector2 minePos = Tank.Position + Tank.GetTurretBodyDirection() * ((float)Tank.Hull.Sprite.Texture.Height/2f + (float)mine.Sprite.Texture.Height/2f + 10f);

            mine.SetPosition(minePos);
        }

        public override bool Activate(Vector2 point, Tank target)
        {
            if (base.Activate(point, target) && Tank.IsStandingStill())
            {
                LayMine();
                return true;
            }

            return false;
        }
    }
}
