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
        private int score;
        private SpriteBatch spriteBatch;
        private SpriteFont scoreFont;
        private float timeLeft;

        public Score(SpriteBatch spriteBatch, SpriteFont scoreFont)
        {
            this.spriteBatch = spriteBatch;
            this.scoreFont = scoreFont;
            score = 0;
            timeLeft = 60f;
        }

        public void Draw(GameTime gameTime)
        {
            timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            int timer = (int)timeLeft;
            Vector2 sizeOfText = scoreFont.MeasureString("SCORE: 999");
            spriteBatch.DrawString(scoreFont, "SCORE: 999", new Vector2(10, 517), Color.White);
            spriteBatch.DrawString(scoreFont, timer.ToString(), new Vector2(sizeOfText.X+30, 517), Color.White);
        }
    }
}
