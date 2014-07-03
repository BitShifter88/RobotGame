using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Macalania.YunaEngine.Rooms
{
    public class Room
    {
        public List<GameObject> GameObjects { get; private set; }
        public List<GameObject> ToBeAdded { get; private set; }
        public ResourceManager Content { get; set; }
        public Camera Camera { get; set; }
        public bool IsRunning { get; set; }

        Mutex _addingMutex = new Mutex();

        public Room()
        {
            GameObjects = new List<GameObject>();
            ToBeAdded = new List<GameObject>();
            Camera = new Camera();
        }

        public virtual void Inizialize()
        {
        }

        public virtual void AddGameObject(GameObject obj)
        {
            _addingMutex.WaitOne();
            obj.Inizialize();
            GameObjects.Add(obj);
            _addingMutex.ReleaseMutex();
        }

        protected virtual void RemoveGameObject(GameObject obj)
        {
            _addingMutex.WaitOne();
            obj.Unload();
            GameObjects.Remove(obj);
            _addingMutex.ReleaseMutex();
        }

        public virtual void AddGameObjectWhileRunning(GameObject obj)
        {
            obj.Inizialize();
            obj.Load(Content);
            _addingMutex.WaitOne();
            ToBeAdded.Add(obj);
            _addingMutex.ReleaseMutex();
        }

#if !SERVER
        public virtual void Load(IServiceProvider serviceProvider)
        {
            Content = new ResourceManager(new ContentManager(serviceProvider));

            foreach (GameObject obj in GameObjects)
            {
                obj.Load(Content);
            }
        }
#endif
#if SERVER
        public virtual void Load(ResourceManager content)
        {
            Content = content;

            foreach (GameObject obj in GameObjects)
            {
                obj.Load(Content);
            }
        }
#endif

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
            IsRunning = true;
            DestroyGameObjects();
            UpdateGameObjects(dt);
            AddGameObjects();
            DestroyGameObjects();

            Camera.Update((float)dt);
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
            _addingMutex.WaitOne();
            foreach (GameObject obj in ToBeAdded)
            {
                GameObjects.Add(obj);
            }
            ToBeAdded.Clear();
            _addingMutex.ReleaseMutex();
        }

        private void DestroyGameObjects()
        {
            _addingMutex.WaitOne();
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
            _addingMutex.ReleaseMutex();
        }

        public virtual void Draw(IRender render)
        {
            render.Begin(Camera);
            foreach (GameObject obj in GameObjects)
            {
                obj.Draw(render, Camera);
            }
            render.End();
        }
    }
}
