using Macalania.YunaEngine.Input;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine
{
    public class YunaGameEngine: Game
    {
        GraphicsDeviceManager graphics;
        IRender _render;
        KeyboardInput _keyboardInput = new KeyboardInput(false);
        MouseInput _mouseInput = new MouseInput();
        Room _activeRoom;
        public static YunaGameEngine Instance { get; set; }

        public delegate void EngineStartedEventHandler();
        public event EngineStartedEventHandler EngineStarted;

        bool _stepByStepExecution = false;

        public YunaGameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";
            Instance = this;

            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 1300;

            this.IsMouseVisible = true;
        }

        public IServiceProvider GetServices()
        {
            return Services;
        }

        public Room GetActiveRoom()
        {
            return _activeRoom;
        }

        public void SetActiveRoom(Room room, bool load)
        {
            if (_activeRoom != null)
                _activeRoom.Unload();
            if (load)
            {
                room.Inizialize();
                room.Load(Services);
            }
            _activeRoom = room;
        }

        protected override void Initialize()
        {
            EngineStarted();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _render = new SimpleRender(graphics.GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _mouseInput.EngineUpdate(gameTime);
            _keyboardInput.EngineUpdate(gameTime);

            if (KeyboardInput.IsKeyClicked(Keys.F1))
                _stepByStepExecution = !_stepByStepExecution;

            if (_stepByStepExecution == false)
            {
                if (_activeRoom != null)
                    _activeRoom.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            else if (_stepByStepExecution == true && KeyboardInput.IsKeyClicked(Keys.F2))
            {
                if (_activeRoom != null)
                    _activeRoom.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (_activeRoom != null)
                _activeRoom.Draw(_render);
            base.Draw(gameTime);
        }
    }
}
