using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.GameLogic
{
    class GameObject
    {
        public virtual void Inizialize()
        {
        }
        public virtual void Load(ContentManager content)
        {
        }
        public virtual void Unload()
        {
        }
        public virtual void Update()
        {
        }
        public virtual void Draw(Camera camera)
        {
        }
    }
}
