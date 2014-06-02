using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.GameLogic
{
    public class GameObject
    {
        public Vector2 Position { get; set; }
        public bool Destroy { get; set; }

        public void DestroyGameObject()
        {
            Destroy = true;
        }

        public virtual void Inizialize()
        {
        }
        public virtual void Load(ContentManager content)
        {
        }
        public virtual void Unload()
        {
        }
        public virtual void Update(double dt)
        {
        }
        public virtual void Draw(IRender render, Camera camera)
        {
        }
    }
}
