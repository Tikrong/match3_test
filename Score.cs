using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3Test
{
    class Score
    {
        private SpriteBatch spriteBatch;
        private SpriteFont scoreFont;

        public Score(SpriteBatch spriteBatch, SpriteFont scoreFont)
        {
            this.spriteBatch = spriteBatch;
            this.scoreFont = scoreFont;
        }

        public void Draw(GameTime gameTime, int currentScore, int timeLeft)
        {
            Vector2 sizeOfText = scoreFont.MeasureString("SCORE: 999");
            spriteBatch.DrawString(scoreFont, "SCORE: " + currentScore, new Vector2(10, 517), Color.White);
            spriteBatch.DrawString(scoreFont, "TIME  " + timeLeft.ToString(), new Vector2(sizeOfText.X+70, 517), Color.White);
        }
    }
}
