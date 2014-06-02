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

namespace Macalania.Probototaker.Tanks
{
    class Shield : GameObject
    {
        Sprite _sprite;
        Tank _tank;
        public float Duration { get; set; }

        public Shield(Room room, Tank tank, float duration)
            : base(room)
        {
            _tank = tank;
            Duration = duration;
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

        public override void Update(double dt)
        {
            _sprite.Position = _tank.Position;
            Duration -= (float)dt;

            if (Duration <= 0)
                DestroyGameObject();

            base.Update(dt);
        }

        public override void Draw(IRender render, Camera camera)
        {
            _sprite.Draw(render, camera);
        }
    }
}
