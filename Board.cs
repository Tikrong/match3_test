using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3
{
    class Board
    {
        Cell[,] boardArray;
        private Random random;

        private int cellSize = 140;

        private Cell selectedCell;

        public Board(Game1 game)
        {
            boardArray = new Cell[8, 8];
            random = new Random();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    boardArray[y,x] = new Cell((Type)random.Next(0, 5), game, new Vector2(x * cellSize, y * cellSize));
                }
            }
        }

        public void DrawBoard(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    boardArray[y,x].DrawCell(spriteBatch, new Vector2(x * cellSize, y * cellSize));
                }
            }
        }

        public void MouseClick(MouseState state)
        {
            int y = state.Y / this.cellSize;
            int x = state.X / this.cellSize;

            if (selectedCell == null)
            {
                selectedCell = boardArray[y, x];
                selectedCell.state = Cell.State.Selected;
            }
            else
            {
                //selectedCell.state = Cell.State.Normal;
                //selectedCell = boardArray[y, x];
                //selectedCell.state = Cell.State.Selected;

                //Vector2 destination = new Vector2(x * cellSize, y * cellSize);
                //selectedCell.destination = destination;
                //selectedCell.state = Cell.State.Moving;
                Cell newCell = boardArray[y, x];
                boardArray[y, x] = boardArray[(int)selectedCell.position.Y / cellSize, (int)selectedCell.position.X / cellSize];
                boardArray[(int)selectedCell.position.Y / cellSize, (int)selectedCell.position.X / cellSize] = newCell;
                SwapCells(selectedCell, newCell);



                selectedCell = null;
            }

         
        }

        public void SwapCells(Cell cell1, Cell cell2)
        {
            cell1.destination = cell2.position;
            cell2.destination = cell1.position;
            cell1.state = Cell.State.Moving;
            cell2.state = Cell.State.Moving;
        }

    }
}
