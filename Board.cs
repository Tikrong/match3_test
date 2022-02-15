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
        private Dictionary<Textures, Texture2D> textures = new Dictionary<Textures, Texture2D>();
        private SpriteBatch spriteBatch;
        private Cell selectedCell;

        
        public Cell[,] cells;
        public bool isAnimating;

        // It maybe transferred to controll class later
        private MouseState lastMouseState;
        private MouseState currentMouseState;

        private Cell wasSpapped1;
        private Cell wasSpapped2;

        public Board(Dictionary<Textures, Texture2D> textures, SpriteBatch spriteBatch)
        {
            random = new Random();
            this.textures = textures;
            this.spriteBatch = spriteBatch;
            cells = new Cell[8, 8];
            isAnimating = false;
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
                    Cell cell = new Cell(spriteBatch, color, y, x, textures[(Textures)color], textures[Textures.Explosion]);
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
            // remember the cells that were swapped
            wasSpapped1 = cell1;
            wasSpapped2 = cell2;

            // change the position of cells in the array
            cells[cell1.Row, cell1.Column] = cell2;
            cells[cell2.Row, cell2.Column] = cell1;

            // Move cell to new position
            Point tmpCell2Pos = new Point(cell2.Row, cell2.Column);
            cell2.MoveTo(cell1.Row, cell1.Column);
            cell1.MoveTo(tmpCell2Pos.X, tmpCell2Pos.Y);

        }

        // this method is called after unsuccessfull swap to return cells to their positions
        public void SwapCellsBack()
        {
            Cell cell1 = wasSpapped1;
            Cell cell2 = wasSpapped2;
            // change the position of cells in the array
            cells[cell1.Row, cell1.Column] = cell2;
            cells[cell2.Row, cell2.Column] = cell1;

            // Move cell to new position
            Point tmpCell2Pos = new Point(cell2.Row, cell2.Column);
            cell2.MoveTo(cell1.Row, cell1.Column);
            cell1.MoveTo(tmpCell2Pos.X, tmpCell2Pos.Y);

            wasSpapped1 = null;
            wasSpapped2 = null;
        }

        // Draws each cell
        public void Draw()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (cells[y,x] != null)
                    {
                        cells[y, x].Draw();
                    }
                    
                }
            }
        }

        // Updates each cell
        public void Update()
        {
            isAnimating = false;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (cells[y,x] == null)
                    {
                        continue;
                    }
                    if (cells[y, x].Update())
                    {
                        isAnimating = true;
                    }
                }
            }
        }

        public bool FindMatches()
        {
            List<List<Cell>> lines = new List<List<Cell>>();
            // Find Horizontal lines
            for (int y = 0; y < 8; y++)
            {
                
                List<Cell> line = new List<Cell>();
                for (int x = 1; x < 8; x++)
                {
                    if (cells[y,x].MarbleColor == cells[y,x-1].MarbleColor)
                    {
                        line.Add(cells[y, x - 1]);
                        if (x == 7)
                        {
                            line.Add(cells[y, x]);
                            if (line.Count >= 3)
                            {
                                lines.Add(line);
                                
                            }
                        }
                        
                    }
                    else
                    {
                        line.Add(cells[y, x - 1]);
                        if (line.Count >= 3)
                        {
                            lines.Add(line);
                            
                        }
                        line = new List<Cell>();
                    }
                }
            }
            
            // Find vertical lines
            for (int x = 0; x < 8; x++)
            {

                List<Cell> line = new List<Cell>();
                for (int y = 1; y < 8; y++)
                {
                    if (cells[y, x].MarbleColor == cells[y - 1, x].MarbleColor)
                    {
                        line.Add(cells[y - 1, x]);
                        if (y == 7)
                        {
                            line.Add(cells[y, x]);
                            if (line.Count >= 3)
                            {
                                lines.Add(line);

                            }
                        }

                    }
                    else
                    {
                        line.Add(cells[y - 1, x]);
                        if (line.Count >= 3)
                        {
                            lines.Add(line);

                        }
                        line = new List<Cell>();
                    }
                }
            }

            // if no mathec return false
            if (lines.Count == 0)
            {
                return false;
            }

            // check that it's working
            // Destroy matches
            foreach (List<Cell> line in lines)
            {
                foreach (Cell cell in line)
                {
                    cell.Destroy();
                }
            }
            // when there are matches return true
            return true;

            // Find intersections
            // Generate bonuses
            // Destroy lines

            return false;
        }

        // Iterates through the board, finds empty spaces and drops the marbles from above to that places
        public bool DropMarbles()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 7; y > 0; y--)
                {
                    if (cells[y, x] == null)
                    {
                        // if cell is empty find closest cell in the top and drop it to this empty cell
                        for (int i = y - 1; i >= 0; i--)
                        {
                            if (cells[i, x] == null)
                            {
                                continue;
                            }

                            // if next cell on the top go higher
                            if (cells[i, x].isEmpty())
                            {
                                continue;
                            }
                            cells[i, x].DropTo(y);
                            //Cell tmpCell = cells[y, x];
                            cells[y, x] = cells[i, x];
                            cells[i, x] = null;
                            break;

                        }
                    }
                    // if cell with marble continue loop
                    else if (!cells[y, x].isEmpty())
                    {
                        continue;
                    }

                    else
                    {
                        // if cell is empty find closest cell in the top and drop it to this empty cell
                        for (int i = y - 1; i >= 0; i--)
                        {
                            if (cells[i, x] == null)
                            {
                                continue;
                            }

                            // if next cell on the top go higher
                            if (cells[i, x].isEmpty())
                            {
                                continue;
                            }
                            cells[i, x].DropTo(y);
                            //Cell tmpCell = cells[y, x];
                            cells[y, x] = cells[i, x];
                            cells[i, x] = null;
                            break;

                        }
                    }

                }
            }
            return false;
        }

        // iterate over every cell in the array and spawn marbles when the cells are empty
        public void SpawnMarbles()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (cells[y,x] == null)
                    {
                        MarbleColor color = (MarbleColor)random.Next(0, 5);
                        Cell cell = new Cell(spriteBatch, color, y, x, textures[(Textures)color], textures[Textures.Explosion]);
                        cells[y, x] = cell;
                        continue;
                    }
                    if (cells[y,x].isEmpty())
                    {
                        MarbleColor color = (MarbleColor)random.Next(0, 5);
                        Cell cell = new Cell(spriteBatch, color, y, x, textures[(Textures)color], textures[Textures.Explosion]);
                        cells[y, x] = cell;
                        
                    }
                }
            }
        }






    }
}
