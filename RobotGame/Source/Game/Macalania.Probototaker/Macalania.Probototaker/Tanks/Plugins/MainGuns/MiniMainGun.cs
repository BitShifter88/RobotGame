using Macalania.Probototaker.Projectiles;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.MainGuns
{
    class MiniMainGun : MainGun
    {
        public MiniMainGun()
        {
            Size = 1;
            ProjectileStartPosition = new Vector2(0, 0);
        }
        public override void Load(ContentManager content)
        {
            Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/MainGuns/MiniMainGun"));
            base.Load(content);
        }

        public override void SpawnProjectile(Vector2 position, Vector2 direction)
        {
            base.SpawnProjectile(position, direction);

            ShellStarter ss = new ShellStarter(Tank, position, direction, 0.5f);

            YunaGameEngine.Instance.GetActiveRoom().AddGameObjectWhileRunning(ss);
        }
    }
}
