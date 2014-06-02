using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Effects
{
    class Explosion : GameObject
    {
        Sprite _sprite;
        float _haveExistedIn = 0;
        float _lifeSpan = 200;

        public Explosion(Vector2 position)
        {
            Position = position;
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
            _haveExistedIn += (float)dt;
            if (_haveExistedIn >= _lifeSpan)
                DestroyGameObject();
            base.Update(dt);
        }

        public override void Draw(IRender render, Camera camera)
        {
            _sprite.Draw(render, camera);
            base.Draw(render, camera);
        }
    }
}
