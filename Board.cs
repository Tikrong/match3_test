using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3Test
{
    class Board
    {
        private Random random;
        private Dictionary<MarbleColor, Texture2D> textures = new Dictionary<MarbleColor, Texture2D>();
        private SpriteBatch spriteBatch;

        
        public Cell[,] cells;

        public Board(Dictionary<MarbleColor, Texture2D> textures, SpriteBatch spriteBatch)
        {
            random = new Random();
            this.textures = textures;
            this.spriteBatch = spriteBatch;
            cells = new Cell[8, 8];
            this.GenerateBoard();
        }

        public void GenerateBoard()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    MarbleColor color = (MarbleColor)random.Next(0, 5);
                    Cell cell = new Cell(spriteBatch, color, y, x, textures[color]);
                    cells[y, x] = cell;
                }
            }
        }

        public void Draw()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    cells[y, x].Draw();
                }
            }
        }




    }
}
