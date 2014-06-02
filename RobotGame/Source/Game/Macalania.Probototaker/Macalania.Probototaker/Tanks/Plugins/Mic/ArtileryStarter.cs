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

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    class ArtileryStarter : Plugin
    {
        ArtileryProjectile[] _rockets;
        bool _firstUpdate = true;

        public ArtileryStarter()
        {
            Size = 1;
            _rockets = new ArtileryProjectile[6];
            OriginOfset = new Vector2(0, 30);
        }
        public override void Load(ContentManager content)
        {
             Sprite = new Sprite(content.Load<Texture2D>("Textures/Tanks/Misc/artilery"));
             Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }

        public override void Update(double dt)
        {
            if (_firstUpdate)
            {
                ReloadRockets();
                _firstUpdate = false;
            }
            base.Update(dt);

            for (int i = 0; i < _rockets.Length; i++)
            {
                if (_rockets[i] != null)
                {
                    _rockets[i].Position = Tank.Position;
                    _rockets[i].Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
                }

            }
        }

        private void ReloadRockets()
        {
            for (int i = 0; i < _rockets.Length; i++)
            {
                ArtileryProjectile rocket2 = new ArtileryProjectile(Tank, new Vector2(0, 0), Tank.GetTurretDirection(), 0.0f);
                YunaGameEngine.Instance.GetActiveRoom().AddGameObjectWhileRunning(rocket2);

                Vector2 org2 = new Vector2((Sprite.Texture.Width / 2) - 2 - 10 * i, Sprite.Origin.Y);
                rocket2.Sprite.Origin = org2;

                _rockets[i] = rocket2;
            }
        }
    }
}
