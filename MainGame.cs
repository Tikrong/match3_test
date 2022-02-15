﻿using Microsoft.Xna.Framework;
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
        public Dictionary<Textures, Texture2D> textures = new Dictionary<Textures, Texture2D>();

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
            textures[Textures.Pink] = Content.Load<Texture2D>("pink");
            textures[Textures.Green] = Content.Load<Texture2D>("green");
            textures[Textures.Blue] = Content.Load<Texture2D>("blue");
            textures[Textures.Grey] = Content.Load<Texture2D>("grey");
            textures[Textures.Orange] = Content.Load<Texture2D>("orange");
            textures[Textures.Explosion] = Content.Load<Texture2D>("explosion");



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
