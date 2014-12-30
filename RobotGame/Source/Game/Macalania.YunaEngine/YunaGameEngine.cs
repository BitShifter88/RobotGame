using Macalania.YunaEngine.Gui;
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
        public static YunaGameEngine Instance { get; set; }

        public delegate void EngineStartedEventHandler();
        public event EngineStartedEventHandler EngineStarted;

        bool _stepByStepExecution = false;

        private Room _activeRoom;

        public YunaGameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
            Resolution.Init(ref graphics);
            graphics.PreferMultiSampling = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            this.IsFixedTimeStep = true;
            Content.RootDirectory = "Content";
            Instance = this;

            Resolution.SetVirtualResolution(1920, 1080);


            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;

            graphics.PreferredBackBufferWidth = 1680;
            graphics.PreferredBackBufferHeight = 1050;

            Resolution.SetResolution(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false);
            
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.PreferredBackBufferWidth = 1920;
            graphics.ToggleFullScreen();
            this.IsMouseVisible = true;
        }

  

        public void SetActiveRoom(Room room, bool load)
        {
            if (_activeRoom != null)
                _activeRoom.Unload();

            _activeRoom = room;
            if (load)
            {
                room.Inizialize();
                room.Load(Services);
            }
        }

        public Room GetActiveRoom()
        {
            return _activeRoom;
        }

        public IServiceProvider GetServices()
        {
            return Services;
        }

        protected override void Initialize()
        {
            Globals.Device = graphics.GraphicsDevice;
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
            GraphicsDevice.Clear(Color.Black);

            if (_activeRoom != null)
                _activeRoom.Draw(_render);
            base.Draw(gameTime);
        }
    }
}
