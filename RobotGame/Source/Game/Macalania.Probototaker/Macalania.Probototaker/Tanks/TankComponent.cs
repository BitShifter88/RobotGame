﻿using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks
{
    public enum TankComponentType
    {
        Other,
        Amor
    }

    public class TankComponent
    {
        public Sprite Sprite { get; set; }
        public Tank Tank { get; private set; }

        public float StoredPower { get; set; }
        public float StoredHp { get; set; }
        public float ComponentMaxHp { get; set; }
        public float ComponentCurrentHp { get; set; }
        public TankComponentType CompType { get; set; }
        public bool IsDestroyed { get; set; }

        public Room Room { get; set; }

        public TankComponent(Room room)
        {
            Room = room;
        }

        public Vector2 GetDim()
        {
            return new Vector2(Sprite.Texture.Width, Sprite.Texture.Height);
        }

        public void SetTank(Tank tank)
        {
            Tank = tank;
        }

        public virtual void Damage(float amount)
        {
            ComponentCurrentHp -= amount;
            CheckDamage();
        }

        private void CheckDamage()
        {
            if (ComponentCurrentHp <= 0)
            {
                OnComponentDestroy();
            }
        }

        public virtual void OnTankDestroy()
        {
        }

        protected virtual void OnComponentDestroy()
        {
            IsDestroyed = true;
        }

        public virtual bool CheckCollision(Sprite s)
        {
            return Sprite.CheckCollision(s, Sprite);
        }

        public void SetColor(Color color)
        {
            Sprite.Color = color;
        }

        public virtual void Update(double dt)
        {
            SetPositionAfterTank();
            Sprite.Update(dt);
        }

        public void SetPositionAfterTank()
        {
            Sprite.Position = Tank.Position;
        }

        public virtual void Load(ResourceManager content)
        {
                Sprite.SetOriginCenter();

                
        }

        public void Ready()
        {
            ComponentCurrentHp = ComponentMaxHp;
        }

        public virtual void Draw(IRender render, Camera camera)
        {
            Sprite.Draw(render, camera);
        }
    }
}
