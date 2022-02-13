using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3Test
{
    class Cell
    {
        private Vector2 position;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private CellState state;
        private float rotationAngle = 0f;

        public int Row { get; set; }
        public int Column { get; set; }

        public MarbleColor MarbleColor { get; set; }

        public Cell(SpriteBatch spriteBatch, MarbleColor color, int row, int column, Texture2D texture)
        {
            this.spriteBatch = spriteBatch;
            MarbleColor = color;
            Row = row;
            Column = column;
            this.texture = texture;
            position = new Vector2(column * Constants.cellSize, row * Constants.cellSize);
            state = CellState.Neutral;
        }

        // returns true if animation is running and false if animation is not running
        public bool Update()
        {
            if (state == CellState.Neutral)
            {
                return false;
            }
            switch(state)
            {
                case CellState.Moving:
                    // do later
                    break;
                case CellState.Selected:
                    rotationAngle += 0.1f;
                    break;
                        
            }
            return true;

        }

        public void Draw()
        {
            switch(state)
            {
                case CellState.Selected:
                    Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                    Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
                    // Here you should change the hardcoded 32,32
                    spriteBatch.Draw(texture, position + new Vector2(32, 32), sourceRectangle, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 1);
                    break;
                default:
                    spriteBatch.Draw(texture, position, Color.White);
                    break;
            }
            
        }




    }
}
