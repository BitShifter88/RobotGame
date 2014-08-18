using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Tanks.Turrets;
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
    public class RocketStarterPlugin : TurretModule
    {
        private RocketStarterProjectile _rocket;
        bool _firstUpdate = true;

        public RocketStarterPlugin(PluginDirection dir, Room room)
            : base(PluginType.RocketStarter, room)
        {
            PluginDir = dir;
            _moduleDim = new Point(3, 4);
            MaxCooldown = 3000;
            ComponentMaxHp = 100;

            for (int i = 1; i < 3; i++ )
            {
                RequiredBricks.Add(new Point(0, i));
            }
            for (int i = 1; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    RequiredFreeSpace.Add(new Point(i, j));
                }
            }

                PossibleDirections.Add(PluginDirection.Right);
            PossibleDirections.Add(PluginDirection.Left);
        }
        public override void Load(ResourceManager content)
        {
            if (PluginDir == PluginDirection.Right)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/rocketStarterRight"));
            if (PluginDir == PluginDirection.Left)
                Sprite = new Sprite(content.LoadYunaTexture("Textures/Tanks/Misc/rocketStarterLeft"));
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

        public void ReloadRocket()
        {
            _rocket = new RocketStarterProjectile(Room, Tank, new Vector2(0, 0), Tank.GetTurretBodyDirection(), 10000, 0.0f);
            Room.AddGameObjectWhileRunning(_rocket);
            if (PluginDir == PluginDirection.Right)
                _rocket.Sprite.Origin = new Vector2(Sprite.Origin.X-16, Sprite.Origin.Y -4);
            if (PluginDir == PluginDirection.Left)
                _rocket.Sprite.Origin = new Vector2(Sprite.Origin.X -8, Sprite.Origin.Y - 4);
        }

        public override bool Activate(Vector2 point, Tank target, Random activationRandom)
        {
            bool success = base.Activate(point, target, activationRandom);

            if (success)
            {
                _rocket.Ignite(Tank.Position, 1300, activationRandom);
                _rocket.Sprite.SetOriginCenter();
                _rocket.Sprite.Rotation = Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180);
                Vector2 p = Vector2.Zero;
                
                if (PluginDir == PluginDirection.Right)
                 p = new Vector2(-(Sprite.Origin.X - 16 - _rocket.Sprite.Texture.Width / 2), -Sprite.Origin.Y + 32);
                if (PluginDir == PluginDirection.Left)
                    p = new Vector2(-(Sprite.Origin.X - 8 - _rocket.Sprite.Texture.Width / 2), -Sprite.Origin.Y + 32);
                p = YunaMath.RotateVector2(p, Tank.GetTurrentBodyRotation() + MathHelper.ToRadians(180));
                _rocket.SetPosition(p + Tank.Position);

                _rocket.Direction = -Tank.GetTurretBodyDirection();
                _rocket = null;
            }

            return success;
        }
    }
}
