using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.GameLogic
{
    class GameObjectTemplate : GameObject
    {
        public GameObjectTemplate(Room room)
            : base(room)
        {
        }

        public override void Inizialize()
        {
            base.Inizialize();
        }
        public override void Load(ContentManager content)
        {
            base.Load(content);
        }
        public override void Update(double dt)
        {
            base.Update(dt);
        }
        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);
        }
    }
}
