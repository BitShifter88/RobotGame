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
    class SmallExplosion : Explosion
    {
        Sprite _sprite;

        public SmallExplosion(Room room, Vector2 position) : base(room, position, 500, 100)
        {

        }

        public override void Load(ContentManager content)
        {
            _sprite = new Sprite(content.Load<Texture2D>("Textures/Effects/explosion"));
            _sprite.SetOriginCenter();
            _sprite.DepthLayer = 0.5f;
            base.Load(content);
        }

        public override void Update(double dt)
        {
            _sprite.Position = Position;
            _sprite.Update(dt);
            base.Update(dt);
        }

        public override void Draw(IRender render, Camera camera)
        {
            _sprite.Draw(render, camera);
            base.Draw(render, camera);
        }
    }
}
