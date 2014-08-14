using Macalania.Probototaker.Tanks;
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

namespace Macalania.Probototaker.Projectiles
{
    public class ShellStarter : Shell
    {
        public ShellStarter(Room room, Tank tankSource, Vector2 position, Vector2 direction)
            : base(room, tankSource, position, direction, 0.5f, 2000, ProjectileType.ShellStarter)
        {
            Damage = new Damage() { TankDamage = 10, AmorPenetration = 10, ComponentDamage = 2 };
        }

        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Projectiles/shell"));
            Sprite.SetOriginCenter();
            Sprite.DepthLayer = 0.7f;
            base.Load(content);
        }

        
    }
}
