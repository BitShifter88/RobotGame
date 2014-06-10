﻿using Macalania.Probototaker.Projectiles;
using Macalania.YunaEngine;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
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
    public class RocketStarterPlugin : Plugin
    {
        private PluginDirection _dir;
        private RocketStarterProjectile _rocket;
        bool _firstUpdate = true;

        public RocketStarterPlugin(PluginDirection dir)
        {
            _dir = dir;
            Size = 3;
            MaxCooldown = 3000;
            ComponentMaxHp = 100;
        }
        public override void Load(ResourceManager content)
        {
            if (_dir == PluginDirection.Right)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/rocketStarterRight"));
            Sprite.DepthLayer = 0.3f;
            base.Load(content);
        }

        public override void OnTankDestroy()
        {
            if (_rocket != null)
                _rocket.Explode();
            base.OnTankDestroy();
        }

        public override void Update(double dt)
        {
            if (_firstUpdate)
            {
                ReloadRocket();
                _firstUpdate = false;
            }

            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;

            if (_rocket != null)
            {
                _rocket.SetPosition(Tank.Position);
                _rocket.Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
                
            }

            base.Update(dt);
        }

        public override void OnReady()
        {
            ReloadRocket();

            base.OnReady();
        }

        private void ReloadRocket()
        {
            _rocket = new RocketStarterProjectile(RoomManager.Instance.GetActiveRoom(), Tank, new Vector2(0, 0), Tank.GetTurretDirection(), 0.0f);
            RoomManager.Instance.GetActiveRoom().AddGameObjectWhileRunning(_rocket);
            _rocket.Sprite.Origin = Sprite.Origin;
        }

        public override bool Activate(Vector2 point, Tank target)
        {
            bool success = base.Activate(point, target);

            if (success)
            {
                _rocket.Ignite(Tank.Position, 1300);
                _rocket.Sprite.SetOriginCenter();
                _rocket.Sprite.Rotation = Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180);
                Vector2 p = new Vector2(Tank.Turret.Sprite.Texture.Width / 2 + _rocket.Sprite.Texture.Width / 2, 0);
                p = YunaMath.RotateVector2(p, Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180));
                _rocket.SetPosition(p + Tank.Position);

                _rocket.Direction = -Tank.GetTurretDirection();
                _rocket = null;
            }

            return success;
        }
    }
}
