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

        private int cellSize = 64;

        private Cell selectedCell;

        private Game1 game;

        private List<Cell> cellsToDestroy = new List<Cell>();

        public Board(Game1 game)
        {
            boardArray = new Cell[8, 8];
            random = new Random();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    boardArray[y,x] = new Cell((Type)random.Next(0, 5), game, new Vector2(x * cellSize, y * cellSize), y, x);
                }
            }
            this.game = game;
        }

        public void DrawBoard(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    boardArray[y,x]?.DrawCell(spriteBatch, new Vector2(x * cellSize, y * cellSize));
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
                //selectedCell.state = Cell.State.Exploding;
                return;
            }
            else if(IsValidNeighbour(boardArray[y, x]))
            {
                //selectedCell.state = Cell.State.Normal;
                //selectedCell = boardArray[y, x];
                //selectedCell.state = Cell.State.Selected;

                //Vector2 destination = new Vector2(x * cellSize, y * cellSize);
                //selectedCell.destination = destination;
                //selectedCell.state = Cell.State.Moving;
                
                Cell newCell = boardArray[y, x];
                // you need to do the swap only if it is possible
                
                if (IsSwapPossible(selectedCell, newCell))
                {
                    SwapCells(selectedCell, newCell);
                    boardArray[y, x] = boardArray[(int)selectedCell.position.Y / cellSize, (int)selectedCell.position.X / cellSize];
                    boardArray[(int)selectedCell.position.Y / cellSize, (int)selectedCell.position.X / cellSize] = newCell;
                    DestroyCells();
                    selectedCell = null;
                    return;
                }

            }

            selectedCell.state = Cell.State.Normal;
            selectedCell = boardArray[y, x];
            selectedCell.state = Cell.State.Selected;



        }

        // we pass new cell here and return true if it is next to the selected cell
        private bool IsValidNeighbour(Cell newCell)
        {
            int tmpX = newCell.cellX - selectedCell.cellX;
            int tmpY = newCell.cellY - selectedCell.cellY;
            if (Math.Abs(tmpX) == 1 && Math.Abs(tmpY) == 0)
            {
                return true;
            }
            else if (Math.Abs(tmpX) == 0 && Math.Abs(tmpY) == 1)
            {
                return true;
            }
            
            return false;
        }

        public void DestroyCells()
        {
            foreach(Cell cell in cellsToDestroy)
            {
                cell.state = Cell.State.Exploding;
                boardArray[cell.cellY, cell.cellX] = null;
            }
            cellsToDestroy = new List<Cell>();
        }

        public void SwapCells(Cell cell1, Cell cell2)
        {
            cell1.destination = cell2.position;
            cell2.destination = cell1.position;
            cell1.state = Cell.State.Moving;
            cell2.state = Cell.State.Moving;
            
        }

        private bool IsSwapPossible(Cell cell1, Cell cell2)
        {
            Cell newCell1 = new Cell(cell1.Type, game, cell2.position, cell2.cellY, cell2.cellX);
            Cell newCell2 = new Cell(cell2.Type, game, cell1.position, cell1.cellY, cell1.cellX);

            boardArray[cell1.cellY, cell1.cellX] = cell2;
            boardArray[cell2.cellY, cell2.cellX] = cell1;



            List<List<Cell>> lines = FindLines(newCell1);
            lines.AddRange(FindLines(newCell2));
            if (lines.Count > 0)
            {
                foreach (List<Cell> line in lines)
                {
                    foreach(Cell cell in line)
                    {
                        cellsToDestroy.Add(cell);
                    }
                }
                return true;
            }

            boardArray[cell1.cellY, cell1.cellX] = cell1;
            boardArray[cell2.cellY, cell2.cellX] = cell2;


            return false;
        }

        private List<List<Cell>> FindLines(Cell cell)
        {
            List<List<Cell>> lines = new List<List<Cell>>();
            List<Cell> line = new List<Cell>();


            // check for horizontal lines
            // go left
            int tmpX;
            int i = 1;
            while(true)
            {
                tmpX = cell.cellX - i;
                if (tmpX < 0)
                {
                    break;
                }
                // if next marble to the left is of the same color append it to the list and go futher. If not - break the cycle
                else if (cell.Type == boardArray[cell.cellY, tmpX]?.Type)
                {
                    line.Add(boardArray[cell.cellY, tmpX]);
                    i++;
                }
                else
                {
                    break;
                }
            }

            // go right
            i = 1;
            while(true)
            {
                tmpX = cell.cellX + i;
                if (tmpX > 7)
                {
                    break;
                }
                // if next marble to the right is of the same color append it to the list and go futher. If not - break the cycle
                else if (cell.Type == boardArray[cell.cellY, tmpX]?.Type)
                {
                    line.Add(boardArray[cell.cellY, tmpX]);
                    i++;
                }
                else
                {
                    break;
                }
            }

            line.Add(cell);
            if (line.Count >= 3)
            {   
                lines.Add(line);
            }

            // check for vertical lines
            int tmpY;
            i = 1;
            line = new List<Cell>();
            while (true)
            {
                tmpY = cell.cellY - i;
                if (tmpY < 0)
                {
                    break;
                }
                // if next marble to the left is of the same color append it to the list and go futher. If not - break the cycle
                else if (cell.Type == boardArray[tmpY, cell.cellX]?.Type)
                {
                    line.Add(boardArray[tmpY, cell.cellX]);
                    i++;
                }
                else
                {
                    break;
                }
            }

            // go right
            i = 1;
            while (true)
            {
                tmpY = cell.cellY + i;
                if (tmpY > 7)
                {
                    break;
                }
                else if (cell.Type == boardArray[tmpY, cell.cellX]?.Type)
                {
                    line.Add(boardArray[tmpY, cell.cellX]);
                    i++;
                }
                else
                {
                    break;
                }
            }

            line.Add(cell);
            if (line.Count >= 3)
            {
                lines.Add(line);
            }


            return lines;
        }



    }
}
