using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Macalania.YunaEngine.Rooms;
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
        public Vector2 Position { get; private set; }
        public bool Destroy { get; set; }
        public Room Room { get; set; }

        public GameObject(Room room)
        {
            Room = room;
        }

        public virtual void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public void DestroyGameObject()
        {
            Destroy = true;
        }

        public virtual void Inizialize()
        {
        }
        public virtual void Load(ResourceManager content)
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
