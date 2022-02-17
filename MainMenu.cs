using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3Test
{

    // This class renders main menu and switches Game State for game loop if button is clicked
    class MainMenu
    {
        private MouseState lastMouseState;
        private MouseState currentMouseState;

        private SpriteBatch spriteBatch;

        private Texture2D background;
        private Texture2D button;
        private Texture2D buttonHover;
        private Rectangle buttonRect;

        private MainGame game;

        bool isHover = false;

        public MainMenu(SpriteBatch spriteBatch, Dictionary<Textures, Texture2D> textures, MainGame game)
        {
            this.spriteBatch = spriteBatch;
            background = textures[Textures.BackgroundMain];
            button = textures[Textures.ButtonPlay];
            buttonHover = textures[Textures.ButtonPlayHover];
            buttonRect = new Rectangle((Constants.screenWidth - button.Width) / 2, (Constants.screenHeight - button.Height) / 2, button.Width, button.Height);
            this.game = game;
        }

        public void Update()
        {
            isHover = false;
            currentMouseState = Mouse.GetState();
            if (buttonRect.Contains(new Point(currentMouseState.X, currentMouseState.Y)))
            {
                isHover = true;
            }

            // if button is clicked
            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                if (buttonRect.Contains(new Point(currentMouseState.X, currentMouseState.Y)))
                {
                    game.ChangeState(GameState.GameLoop);
                }
            }
        }

        public void Draw()
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            if (isHover)
            {
                spriteBatch.Draw(buttonHover, buttonRect, Color.White);

            }
            else
            {
                spriteBatch.Draw(button, buttonRect, Color.White);
            }
        }


    }
}
