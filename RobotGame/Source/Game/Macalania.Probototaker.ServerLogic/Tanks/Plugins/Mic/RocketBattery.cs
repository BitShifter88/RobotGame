using Macalania.Probototaker.Network;
using Macalania.Probototaker.Projectiles;
using Macalania.Probototaker.Tanks.Turrets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Plugins.Mic
{
    public class RocketBattery : TurretModule
    {
        protected Rocket[] _rockets;
        bool _firstUpdate = true;
        protected Vector2 _targetPosition;
        protected Tank _targetTank;
        protected float _fireInterval = 0;
        protected float _currentFire = 0;
        bool _fireringRockets = false;

        public RocketBattery(PluginType type)
            : base(type)
        {

        }

        public override void Update(double dt)
        {
            if (_firstUpdate)
            {
                OnFirstUpdate();
            }

            UpdateRocketPositionRotation();

            CheckFireringRockets(dt);

            base.Update(dt);
        }

        private void CheckFireringRockets(double dt)
        {
            if (_fireringRockets && _currentFire <= 0)
            {
                // Find the next rocket to fire
                int rocketToFire = GameRandom.GetRandomInt(0, _rockets.Length - 1);
                while (_rockets[rocketToFire] == null)
                {
                    rocketToFire = GameRandom.GetRandomInt(0, _rockets.Length - 1);
                }

                FireRocket(rocketToFire);
                _currentFire = _fireInterval;

                if (IsAllRocketsFired())
                {
                    _fireringRockets = false;
                    OnFieringEnd();
                }
            }
            _currentFire -= (float)dt;
        }

        protected virtual void OnFieringEnd()
        {

        }

        public override bool Activate(Vector2 point, Tank target)
        {
            if (base.Activate(point, target))
            {
                _targetPosition = point;
                _targetTank = target;
                OnFireRockets();

                return true;
            }
            return false;
        }

        public override void OnTankDestroy()
        {
            for (int i = 0; i < _rockets.Length; i++)
            {
                if (_rockets[i] != null)
                {
                    Rocket r = _rockets[i];
                    FireRocket(i);
                    r.Explode();
                }
            }
            base.OnTankDestroy();
        }

        public override void OnReady()
        {
            base.OnReady();

            ReloadRockets();
        }

        protected virtual void OnFireRockets()
        {
            _fireringRockets = true;
        }

        protected bool IsAllRocketsFired()
        {
            for (int i = 0; i < _rockets.Length; i++)
            {
                if (_rockets[i] != null)
                    return false;
            }
            return true;
        }

        protected virtual void FireRocket(int index)
        {
        }


        protected virtual void OnFirstUpdate()
        {
            ReloadRockets();
            _firstUpdate = false;
        }

        protected virtual void ReloadRockets()
        {
            
        }

        private void UpdateRocketPositionRotation()
        {
            for (int i = 0; i < _rockets.Length; i++)
            {
                if (_rockets[i] != null)
                {
                    _rockets[i].SetPosition(Tank.Position);
                    _rockets[i].Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
                }
            }
        }
    }
}
