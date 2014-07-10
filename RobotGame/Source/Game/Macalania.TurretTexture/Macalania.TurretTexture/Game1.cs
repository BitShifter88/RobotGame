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

namespace Macalania.TurretTexture
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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

            CornersLeftTop();
            CornersRightTop();

            CornersLeftBottom();

            // TODO: use this.Content to load your game content here
        }

        private void CornersRightTop()
        {
            Texture2D texture = Content.Load<Texture2D>("turretBigNew");

            Color[] pixels = new Color[texture.Width * texture.Height];

            texture.GetData<Color>(pixels);

            Color replace = new Color(0, 0, 0, 0);

            for (int i = 0; i < texture.Width; i += 16)
            {
                for (int j = 0; j < texture.Height; j += 16)
                {
                    pixels[i + 0+12 + (j + 0) * texture.Width] = replace;
                    pixels[i + 1 + 12 + (j + 0) * texture.Width] = replace;
                    pixels[i + 2 + 12 + (j + 0) * texture.Width] = replace;
                    pixels[i + 3 + 12 + (j + 0) * texture.Width] = replace;

                    pixels[i + 0 + 12+1 + (j + 1) * texture.Width] = replace;
                    pixels[i + 1 + 12 +1+ (j + 1) * texture.Width] = replace;
                    pixels[i + 2 + 12+1 + (j + 1) * texture.Width] = replace;

                    pixels[i + 0 + 12 +2+ (j + 2) * texture.Width] = replace;
                    pixels[i + 1 + 12 +2+ (j + 2) * texture.Width] = replace;

                    pixels[i + 0 + 12 +3+ (j + 3) * texture.Width] = replace;
                }
            }

            Texture2D output = new Texture2D(GraphicsDevice, texture.Width, texture.Height);
            output.SetData<Color>(pixels);

            output.SaveAsPng(new FileStream("C:\\Users\\John\\Desktop\\textureOutput\\cornersRightTop.png", FileMode.Create), texture.Width, texture.Height);
        }


        private void CornersLeftBottom()
        {
            Texture2D texture = Content.Load<Texture2D>("turretBigNew");

            Color[] pixels = new Color[texture.Width * texture.Height];

            texture.GetData<Color>(pixels);

            Color replace = new Color(0, 0, 0, 0);

            for (int i = 0; i < texture.Width; i += 16)
            {
                for (int j = 0; j < texture.Height; j += 16)
                {
                    pixels[i + 0 + (j + 0 +12) * texture.Width] = replace;
                    pixels[i + 1 + (j + 0 +12) * texture.Width] = replace;
                    pixels[i + 2 + (j + 0 +12) * texture.Width] = replace;
                    pixels[i + 3 + (j + 0+12) * texture.Width] = replace;

                    pixels[i + 0 + (j + 1+12) * texture.Width] = replace;
                    pixels[i + 1 + (j + 1 + 12) * texture.Width] = replace;
                    pixels[i + 2 + (j + 1 +12) * texture.Width] = replace;

                    pixels[i + 0 + (j + 2 +12) * texture.Width] = replace;
                    pixels[i + 1 + (j + 2 +12) * texture.Width] = replace;

                    pixels[i + 0 + (j + 3 + 12) * texture.Width] = replace;
                }
            }

            Texture2D output = new Texture2D(GraphicsDevice, texture.Width, texture.Height);
            output.SetData<Color>(pixels);

            output.SaveAsPng(new FileStream("C:\\Users\\John\\Desktop\\textureOutput\\cornersLeftBottom.png", FileMode.Create), texture.Width, texture.Height);
        }

        private void CornersLeftTop()
        {
            Texture2D texture = Content.Load<Texture2D>("turretBigNew");

            Color[] pixels = new Color[texture.Width * texture.Height];

            texture.GetData<Color>(pixels);

            Color replace = new Color(0, 0, 0, 0);

            for (int i = 0; i < texture.Width; i += 16)
            {
                for (int j = 0; j < texture.Height; j += 16)
                {
                    pixels[i + 0 + (j + 0) * texture.Width] = replace;
                    pixels[i + 1 + (j + 0) * texture.Width] = replace;
                    pixels[i + 2 + (j + 0) * texture.Width] = replace;
                    pixels[i + 3 + (j + 0) * texture.Width] = replace;

                    pixels[i + 0 + (j + 1) * texture.Width] = replace;
                    pixels[i + 1 + (j + 1) * texture.Width] = replace;
                    pixels[i + 2 + (j + 1) * texture.Width] = replace;

                    pixels[i + 0 + (j + 2) * texture.Width] = replace;
                    pixels[i + 1 + (j + 2) * texture.Width] = replace;

                    pixels[i + 0 + (j + 3) * texture.Width] = replace;
                }
            }

            Texture2D output = new Texture2D(GraphicsDevice, texture.Width, texture.Height);
            output.SetData<Color>(pixels);

            output.SaveAsPng(new FileStream("C:\\Users\\John\\Desktop\\textureOutput\\cornersLeftTop.png", FileMode.Create), texture.Width, texture.Height);
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
