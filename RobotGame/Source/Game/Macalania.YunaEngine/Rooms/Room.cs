using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Rooms
{
    class Room
    {
        public List<GameObject> GameObjects { get; set; }
        public ContentManager Content { get; set; }
        public Camera Camera { get; set; }

        public void Room()
        {
            GameObjects = new List<GameObject>();
            Camera = new Camera();
        }

        public virtual void AddGameObject(GameObject obj)
        {
            obj.Inizialize();
            GameObjects.Add(obj);
        }

        public virtual void AddGameObjectWhileRunning(GameObject obj)
        {
            obj.Inizialize();
            obj.Load(Content);
            GameObjects.Add(obj);
        }

        public virtual void Load(IServiceProvider serviceProvider)
        {
            Content = new ContentManager(serviceProvider);

            foreach (GameObject obj in GameObjects)
            {
                obj.Load(Content);
            }
        }

        public virtual void Unload()
        {
            Content.Unload();

            foreach (GameObject obj in GameObjects)
            {
                obj.Unload();
            }

            GameObjects.Clear();
        }

        public virtual void Update(float dt)
        {
            foreach (GameObject obj in GameObjects)
            {
                obj.Update();
            }
        }

        public virtual void Draw()
        {
            foreach (GameObject obj in GameObjects)
            {
                obj.Draw(Camera);
            }
        }
    }
}
