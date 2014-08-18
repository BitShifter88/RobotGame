using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace TextureGenerator
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Texture2D> Pixel16s = new List<Texture2D>();
        List<Texture2D> Pixel6s = new List<Texture2D>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadBites();
         
            // TODO: use this.Content to load your game content here
        }

        private void LoadBites()
        {
            string[] files = Directory.GetFiles("Content");

            foreach (string file in files)
            {
                if (file.Contains(".xnb"))
                    continue;
                string cutfile = file.Remove(file.Length - 4);
                cutfile = cutfile.Replace('\\', '/');
                cutfile = cutfile.Remove(0, cutfile.IndexOf("Content") + 8);

                string[] split = cutfile.Split('-');

                if (split[0].Contains("16"))
                {
                    Pixel16s.Add(Content.Load<Texture2D>(cutfile));
                }
                if (split[0].Contains("6") && !split[0].Contains("16"))
                {
                    Pixel6s.Add(Content.Load<Texture2D>(cutfile));
                }
            }

            Texture2D result = new Texture2D(GraphicsDevice, 512, 512);
            Color[] pixels = new Color[512 * 512];
            result.GetData(pixels);

            Color background = new Color(39, 88, 27);

            for (int i = 0; i < 512; i++)
            {
                for (int j = 0; j < 512; j++)
                {
                    pixels[i + j * 512] = background;
                }
            }

            result.SetData(pixels);

            result.SaveAsPng(new FileStream("output.png", FileMode.Create), 512, 512);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
