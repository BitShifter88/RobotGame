using Macalania.Probototaker.Network;
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

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    public class SuperArtillery : Plugin
    {
        ArtileryProjectile[] _rockets;
        bool _firstUpdate = true;
        bool _fireringRockets = false;
        float _fireInterval = 400;
        float _currentFire = 0;
        Vector2 _target;

        public SuperArtillery()
        {
            Size = 1;
            _rockets = new ArtileryProjectile[6];
            OriginOfset = new Vector2(0, 30);
            MaxCooldown = 5000;
            ComponentMaxHp = 100;
        }
        public override void Load(ResourceManager content)
        {
             Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/superArtillery"));
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
            

            for (int i = 0; i < _rockets.Length; i++)
            {
                if (_rockets[i] != null)
                {
                    _rockets[i].SetPosition(Tank.Position);
                    _rockets[i].Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
                }
            }

            if (_fireringRockets && _currentFire <= 0)
            {
                int rocketToFire = GameRandom.GetRandomInt(0, 5);

                while (_rockets[rocketToFire] == null)
                {
                    rocketToFire = GameRandom.GetRandomInt(0, 5);
                }

                FireRocket(rocketToFire);
                _currentFire = _fireInterval;

                if (IsAllRocketsFired())
                    _fireringRockets = false;
            }
            _currentFire -= (float)dt;

            base.Update(dt);
        }

        private bool IsAllRocketsFired()
        {
            for (int i= 0; i < _rockets.Length; i++)
            {
                if (_rockets[i] != null)
                    return false;
            }
            return true;
        }

        public override bool Activate(Vector2 point, Tank target)
        {
            if (base.Activate(point, target))
            {
                _target = point;
                FireRockets();
               
                return true;
            }
            return false;
        }

        public override void OnReady()
        {
            base.OnReady();

            ReloadRockets();
        }

        private void FireRockets()
        {
            _fireringRockets = true;
        }

        private void FireRocket(int index)
        {
            _rockets[index].Ignite(Tank.Position, Vector2.Distance(Tank.Position, _target) + GameRandom.GetRandomInt(-200, 200));
            _rockets[index].Sprite.SetOriginCenter();
            _rockets[index].Sprite.Rotation = Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180);
            Vector2 p = new Vector2(-((Sprite.Texture.Width / 2) - 2 - 3 - 10 * index), -Sprite.Origin.Y + _rockets[index].Sprite.Texture.Height/2);
            p = YunaMath.RotateVector2(p, Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180));
            _rockets[index].SetPosition(p + Tank.Position);

            _rockets[index].Direction = -Tank.GetTurretBodyDirection();
            _rockets[index] = null;
        }

        private void ReloadRockets()
        {
            for (int i = 0; i < _rockets.Length; i++)
            {
                ArtileryProjectile rocket2 = new ArtileryProjectile(RoomManager.Instance.GetActiveRoom(), Tank, new Vector2(0, 0), Tank.GetTurretBodyDirection(), 0.0f);
                RoomManager.Instance.GetActiveRoom().AddGameObjectWhileRunning(rocket2);

                Vector2 org2 = new Vector2((Sprite.Texture.Width / 2) - 2 - 10 * i, Sprite.Origin.Y);
                rocket2.Sprite.Origin = org2;

                _rockets[i] = rocket2;
            }
        }
    }
}
