using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Items
{
    class StarterMine : GameObject
    {
        public Sprite Sprite { get; set; }

        public StarterMine(Room room)
            : base(room)
        {
        }

        public override void Load(ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Items/starterMine"));
            Sprite.SetOriginCenter();
            Sprite.DepthLayer = 0.9f;
            base.Load(content);
        }

        public override void SetPosition(Vector2 position)
        {
            Sprite.Position = position;
            base.SetPosition(position);
        }

        public void PlaceMine(Vector2 position)
        {
            SetPosition(position);
        }
        public override void Update(double dt)
        {
            Sprite.Update(dt);
            base.Update(dt);
        }
        public override void Draw(YunaEngine.Rendering.IRender render, Camera camera)
        {
            Sprite.Draw(render, camera);
        }
    }
}
