using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Rooms
{
    public class Room
    {
        public List<GameObject> GameObjects { get; set; }
        public List<GameObject> ToBeAdded { get; set; }
        public ContentManager Content { get; set; }
        public Camera Camera { get; set; }
        public YunaGameEngine Engine { get; set; }

        public Room(YunaGameEngine engine)
        {
            Engine = engine;
            GameObjects = new List<GameObject>();
            ToBeAdded = new List<GameObject>();
            Camera = new Camera();
        }

        public virtual void Inizialize()
        {
        }

        public virtual void AddGameObject(GameObject obj)
        {
            obj.Inizialize();
            GameObjects.Add(obj);
        }

        protected virtual void RemoveGameObject(GameObject obj)
        {
            obj.Unload();
            GameObjects.Remove(obj);
        }

        public virtual void AddGameObjectWhileRunning(GameObject obj)
        {
            obj.Inizialize();
            obj.Load(Content);
            ToBeAdded.Add(obj);
        }

        public virtual void Load(IServiceProvider serviceProvider)
        {
            Content = new ContentManager(serviceProvider);
            Content.RootDirectory = "Content";

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

        public virtual void Update(double dt)
        {
            DestroyGameObjects();
            UpdateGameObjects(dt);
            AddGameObjects();
            DestroyGameObjects();
        }

        private void UpdateGameObjects(double dt)
        {
            foreach (GameObject obj in GameObjects)
            {
                obj.Update(dt);
            }
        }

        private void AddGameObjects()
        {
            foreach (GameObject obj in ToBeAdded)
            {
                GameObjects.Add(obj);
            }
            ToBeAdded.Clear();
        }

        private void DestroyGameObjects()
        {
            List<GameObject> objToDestroy = new List<GameObject>();

            foreach (GameObject obj in GameObjects)
            {
                if (obj.Destroy)
                    objToDestroy.Add(obj);
            }
            foreach (GameObject obj in objToDestroy)
            {
                RemoveGameObject(obj);
            }
        }

        public virtual void Draw(IRender render)
        {
            render.Begin();
            foreach (GameObject obj in GameObjects)
            {
                obj.Draw(render, Camera);
            }
            render.End();
        }
    }
}
