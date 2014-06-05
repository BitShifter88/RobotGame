using Macalania.Probototaker.Tanks;
using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Effects
{
    public class Shield : GameObject
    {
        Sprite _sprite;
        Tank _tank;
        public float Duration { get; set; }
        public float Radius { get; set; }

        public Shield(Room room, Tank tank, float duration, float radius)
            : base(room)
        {
            _tank = tank;
            Duration = duration;
            Radius = radius;
            _tank.SheeldEnabled = true;
        }

        public override void Load(ContentManager content)
        {
            _sprite = new Sprite(content.Load<Texture2D>("Textures/Effects/shieldsphere"));
            _sprite.Color = new Color(255, 255, 255, 100);
            _sprite.SetOriginCenter();
            _sprite.Position = _tank.Position;
            _sprite.DepthLayer = 0.9f;
            base.Load(content);
        }

        public bool CheckCollision(Sprite s)
        {
            if (Vector2.Distance(new Vector2(s.RelativeBoundingSphere.Center.X, s.RelativeBoundingSphere.Center.Y), Position) <= s.RelativeBoundingSphere.Radius + Radius)
                return true;
            return false;
        }

        private void OnShieldEnd()
        {
            DestroyGameObject();
            _tank.SheeldEnabled = false;
        }

        public override void Update(double dt)
        {
            SetPosition(_tank.Position);
            _sprite.Position = Position;
            Duration -= (float)dt;

            if (Duration <= 0)
                OnShieldEnd();

            base.Update(dt);

            _sprite.Update(dt);
        }

        public override void Draw(IRender render, Camera camera)
        {
            _sprite.Draw(render, camera);
        }
    }
}
