using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3Test
{   // this class is used to play animation from sprite sheet
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
        }

        public bool Update()
        {
            currentFrame++;
            if (currentFrame == totalFrames * 3)
            {
                currentFrame = 0;
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (currentFrame / 3) / Columns;
            int column = (currentFrame / 3) % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, 64, 64);

            
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            
        }

    }
}
