//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Media;
//using Macalania.Probototaker.Tanks;
//using Macalania.YunaEngine.Rooms;
//using Macalania.YunaEngine.Rendering;
//using Macalania.YunaEngine.Input;

//namespace Macalania.Probototaker
//{
//    /// <summary>
//    /// This is the main type for your game
//    /// </summary>
//    public class Game1 : Microsoft.Xna.Framework.Game
//    {
//        GraphicsDeviceManager graphics;
//        Room _room;
//        IRender _render;
//        KeyboardInput _keyboardInput = new KeyboardInput(false);
//        MouseInput _mouseInput = new MouseInput();

//        public Game1()
//        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
//            this.IsMouseVisible = true;
//        }

//        protected override void Initialize()
//        {
//            _room = new Room();

//            Player player = new Player();
//            _room.AddGameObject(player);

            
//            base.Initialize();
//        }

//        protected override void LoadContent()
//        {
//            _render = new SimpleRender(graphics.GraphicsDevice);
//            _room.Load(Services);
//        }

//        protected override void UnloadContent()
//        {
//            _room.Unload();
//        }

//        protected override void Update(GameTime gameTime)
//        {
//            base.Update(gameTime);
//            _mouseInput.EngineUpdate(gameTime);
//            _keyboardInput.EngineUpdate(gameTime);

//            _room.Update(gameTime.ElapsedGameTime.TotalMilliseconds);
//        }

//        protected override void Draw(GameTime gameTime)
//        {
//            GraphicsDevice.Clear(Color.CornflowerBlue);

//            _room.Draw(_render);
//            base.Draw(gameTime);
//        }
//    }
//}
