using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks
{
    class TankComponent
    {
        public Sprite Sprite { get; set; }
        public Tank Tank { get; private set; }

        public float StoredPower { get; set; }
        public float StoredHp { get; set; }

        public Vector2 GetDim()
        {
            return new Vector2(Sprite.Texture.Width, Sprite.Texture.Height);
        }

        public void SetTank(Tank tank)
        {
            Tank = tank;
        }

        public virtual bool CheckCollision(Sprite s)
        {
            return Sprite.CheckCollision(s, Sprite);
        }

        public virtual void Update(double dt)
        {
            Sprite.Position = Tank.Position;
            Sprite.Update(dt);
        }

        public virtual void Load(ContentManager content)
        {
            Sprite.SetOriginCenter();
        }



        public virtual void Draw(IRender render, Camera camera)
        {
            Sprite.Draw(render, camera);
        }
    }
}
