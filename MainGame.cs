using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Match3Test
{
    class MainGame: Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // content
        public Dictionary<MarbleColor, Texture2D> textures = new Dictionary<MarbleColor, Texture2D>();

        // Change later
        private Gameplay gameplay;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = Constants.screenHeight;
            graphics.PreferredBackBufferWidth = Constants.screenWidth;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load textures for marbles into a dictionary
            textures[MarbleColor.Red] = Content.Load<Texture2D>("pink");
            textures[MarbleColor.Green] = Content.Load<Texture2D>("green");
            textures[MarbleColor.Blue] = Content.Load<Texture2D>("blue");
            textures[MarbleColor.Grey] = Content.Load<Texture2D>("grey");
            textures[MarbleColor.Orange] = Content.Load<Texture2D>("orange");


        }

        protected override void Update(GameTime gameTime)
        {
            if (gameplay == null)
            {
                gameplay = new Gameplay(spriteBatch, textures);
            }

            gameplay.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            spriteBatch.Begin();
            gameplay.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
