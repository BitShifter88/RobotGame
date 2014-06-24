using Macalania.Probototaker.Projectiles;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.MainGuns
{
    public class MiniMainGun : MainGun
    {
        public MiniMainGun()
            : base(PluginType.MiniMainGun)
        {
            Size = 1;
            ProjectileStartPosition = new Vector2(0, 0);
            RateOfFire = 150;
            CoolDownRate = 0.03f;
        }
        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/MainGuns/MiniMainGun"));
            Sprite.DepthLayer = 0.24f;
            base.Load(content);
        }

        public override void Fire(Vector2 position, Vector2 direction)
        {
            base.Fire(position, direction);

            if (TimeSinceLastFire > RateOfFire)
            {
                ShellStarter ss = new ShellStarter(RoomManager.Instance.GetActiveRoom(), Tank, position, direction);

                RoomManager.Instance.GetActiveRoom().AddGameObjectWhileRunning(ss);
                
                ShotFired();
            }
        }
    }
}
