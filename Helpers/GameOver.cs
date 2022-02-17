using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3Test
{
    // Draws GameOver message on the screen with button that leads to MainMenu
    class GameOver
    {
        private MouseState lastMouseState;
        private MouseState currentMouseState;

        private SpriteBatch spriteBatch;
        private SpriteFont font;
        

        private Texture2D gameOver;
        private Rectangle gameOverRect;

        private Texture2D button;
        private Texture2D buttonHover;
        private Rectangle buttonRect;

        bool isHover = false;

       

        public GameOver(SpriteBatch spriteBatch, SpriteFont font, Dictionary<Textures, Texture2D> textures)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            gameOver = textures[Textures.GameOver];
            gameOverRect = new Rectangle((Constants.screenWidth - gameOver.Width) / 2, (Constants.screenHeight - gameOver.Height) / 2, gameOver.Width, gameOver.Height);
            button = textures[Textures.ButtonQuit];
            buttonHover = textures[Textures.ButtonQuitHover];
            buttonRect = new Rectangle((Constants.screenWidth - button.Width)/2, gameOverRect.Bottom + 30, button.Width, button.Height);

        }

        // returns true if button quit is clicked
        public bool Update()
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
                    return true;
                }
            }

            return false;
        }

        public void Draw(int score)
        {

            spriteBatch.Draw(gameOver, gameOverRect, Color.White);
            if (isHover)
            {
                spriteBatch.Draw(buttonHover, buttonRect, Color.White);
                
            }
            else
            {
                spriteBatch.Draw(button, buttonRect, Color.White);
            }
            
            Vector2 scoreSize = font.MeasureString("YOUR SCORE: " + score);
            spriteBatch.DrawString(font, "YOUR SCORE: " + score, new Vector2((Constants.screenWidth - scoreSize.X)/2, buttonRect.Bottom + 50), Color.White);
            
            
        }
    }
}
