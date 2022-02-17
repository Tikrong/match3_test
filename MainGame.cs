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
        public Dictionary<Textures, Texture2D> textures = new Dictionary<Textures, Texture2D>();
        private SpriteFont scoreFont;

        // Change later
        private Gameplay gameplay;
        private MainMenu mainMenu;

        private GameState state;

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
            textures[Textures.Bomb] = Content.Load<Texture2D>("bomb");
            textures[Textures.LineHor] = Content.Load<Texture2D>("line_hor");
            textures[Textures.LineVer] = Content.Load<Texture2D>("line_ver");
            textures[Textures.Fireball] = Content.Load<Texture2D>("destroyer");
            textures[Textures.BackgroundMain] = Content.Load<Texture2D>("background6");
            textures[Textures.BackgroundPlay] = Content.Load<Texture2D>("background5");
            textures[Textures.ButtonPlay] = Content.Load<Texture2D>("playNeutral");
            textures[Textures.ButtonPlayHover] = Content.Load<Texture2D>("playHover");
            textures[Textures.ButtonQuit] = Content.Load<Texture2D>("quitNormal");
            textures[Textures.ButtonQuitHover] = Content.Load<Texture2D>("quitHover");
            textures[Textures.GameOver] = Content.Load<Texture2D>("gameOver");


            scoreFont = Content.Load<SpriteFont>("scoreFont");



        }

        protected override void Update(GameTime gameTime)
        {
            if (mainMenu == null)
            {
                ChangeState(GameState.MainMenu);
            }

            switch(state)
            {
                case GameState.MainMenu:
                    mainMenu.Update();
                    break;
                case GameState.GameLoop:
                    gameplay.Update(gameTime);
                    break;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);

            spriteBatch.Begin();
            switch (state)
            {
                case GameState.MainMenu:
                    mainMenu.Draw();
                    break;
                case GameState.GameLoop:
                    gameplay.Draw(gameTime);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ChangeState(GameState newState)
        {
            switch(newState)
            {
                case GameState.MainMenu:
                    state = newState;
                    mainMenu = new MainMenu(spriteBatch, textures, this);
                    break;
                case GameState.GameLoop:
                    state = newState;
                    gameplay = new Gameplay(spriteBatch, textures, scoreFont, this);
                    break;
            }
        }

    }
}
