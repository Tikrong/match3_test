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
        private Cell selectedCell;

        
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
                // 1. if no cell is selected - select cell
                if (selectedCell == null)
                {
                    selectedCell = cells[mouseY, mouseX];
                    selectedCell.SelectCell();
                    lastMouseState = currentMouseState;
                    return false;
                }
                // 2. if cell is selected, check whether new cell is valid option for swap (is neighbour)
                Cell candidateCell = cells[mouseY, mouseX];
                
                if (selectedCell.isNeigbour(candidateCell))
                {
                    // 3. if valid option for swap, return true
                    selectedCell.UnselectCell();
                    SwapCells(selectedCell, candidateCell);
                    selectedCell = null;
                    lastMouseState = currentMouseState;
                    return true;
                }
                // 4. if not a valid option for swap, select other cell and return false
                else
                {
                    selectedCell.UnselectCell();
                    selectedCell = candidateCell;
                    selectedCell.SelectCell();
                    lastMouseState = currentMouseState;
                    return false;
                }
                
                

            }
            lastMouseState = currentMouseState;

            return false;
        }

        // places cell1 into position of cell2 in the array and moves them on places of each other
        public void SwapCells(Cell cell1, Cell cell2)
        {
            // change the position of cells in the array
            cells[cell1.Row, cell1.Column] = cell2;
            cells[cell2.Row, cell2.Column] = cell1;

            // Move cell to new position
            Point tmpCell2Pos = new Point(cell2.Row, cell2.Column);
            cell2.MoveTo(cell1.Row, cell1.Column);
            cell1.MoveTo(tmpCell2Pos.X, tmpCell2Pos.Y);
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
