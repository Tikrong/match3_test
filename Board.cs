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

        // It maybe transferred to controll class later
        private MouseState lastMouseState;
        private MouseState currentMouseState;

        public Board(Dictionary<MarbleColor, Texture2D> textures, SpriteBatch spriteBatch)
        {
            random = new Random();
            this.textures = textures;
            this.spriteBatch = spriteBatch;
            cells = new Cell[8, 8];
            this.GenerateBoard();
        }

        // Generates new board and fills it with random elements in each cell
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

        // Check user input and returns true if should be swapped
        public bool UserInput()
        {
            int mouseY;
            int mouseX;

            currentMouseState = Mouse.GetState();
            // if mouse was clicked
            if (currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                // get the coordinates of call that was clilcked in the array of cells
                mouseY = currentMouseState.Y / Constants.cellSize;
                mouseX = currentMouseState.X / Constants.cellSize;

                // WORKING HERE
                

            }
            lastMouseState = currentMouseState;

            return false;
        }

        // Draws each cell
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

        // Updates each cell
        public void Update()
        {
            
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    cells[y, x].Update();
                }
            }
        }




    }
}
