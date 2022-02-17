using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Match3Test
{
    // this class handles the grid of marbles and implements function for core gameplay (swap, finding and destroying matches, dropping and creating marbles)
    class Board
    {
        private Random random;
        private Dictionary<Textures, Texture2D> textures = new Dictionary<Textures, Texture2D>();
        private SpriteBatch spriteBatch;
        private Cell selectedCell;

        private MouseState lastMouseState;
        private MouseState currentMouseState;

        // ued to store marbles that were swapped to unswap them, if no match or to create bonuses in their places if conditions are met
        private Cell wasSpapped1;
        private Cell wasSpapped2;

        public Cell[,] cells;
        public List<Destroyer> destroyers = new List<Destroyer>();
        public bool isAnimating;
        public int score { get; private set; }

        public Board(Dictionary<Textures, Texture2D> textures, SpriteBatch spriteBatch)
        {
            random = new Random();
            this.textures = textures;
            this.spriteBatch = spriteBatch;
            cells = new Cell[8, 8];
            isAnimating = false;
            this.GenerateBoard();
            score = 0;
        }

        // Generates new board and fills it with random elements in each cell
        public void GenerateBoard()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    MarbleColor color = (MarbleColor)random.Next(0, 5);
                    Cell cell = new Cell(spriteBatch, color, y, x, textures);
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
                // make sure that game field is clicked
                if (currentMouseState.Y > Constants.cellSize * 8)
                {
                    return false;
                }
                // get the coordinates of call that was clilcked in the array of cells
                mouseY = currentMouseState.Y / Constants.cellSize;
                mouseX = currentMouseState.X / Constants.cellSize;

                
                // 1. if no cell is selected - select cell
                if (selectedCell == null)
                {
                    selectedCell = cells[mouseY, mouseX];
                    selectedCell.SelectCell();
                    lastMouseState = currentMouseState;
                    return false;
                }
                // 2. if cell is already selected, check whether new cell is a valid option for swap (is neighbour)
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

            SwapCells(wasSpapped1, wasSpapped2);
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
                    if (cells[y, x] != null)
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
                    if (cells[y, x] == null)
                    {
                        continue;
                    }
                    // Update method of cell returns true if animation is running
                    if (cells[y, x].Update())
                    {
                        isAnimating = true;
                    }
                }
            }

        }

        // this method finds matches and destroys matches that are currently on the board
        public bool FindMatches()
        {
            // here we store all the matches that were found
            List<List<Cell>> lines = new List<List<Cell>>();
            // Find Horizontal matches
            for (int y = 0; y < 8; y++)
            {
                /* iterate through row from the second marble and add previous to the list
                if colors are the same, repeat. If colors differ, check whether line 
                is a match3 or longer and start new list*/

                List<Cell> line = new List<Cell>();
                for (int x = 1; x < 8; x++)
                {
                    line.Add(cells[y, x - 1]);
                    if (cells[y, x].MarbleColor == cells[y, x - 1].MarbleColor)
                    {
                        if (x==7) line.Add(cells[y, x]);
                        continue;
                    }
                    if (line.Count() >= 3) lines.Add(line);
                    line = new List<Cell>();

                }
                if (line.Count() >= 3) lines.Add(line);
                
            }

            // Find vertical lines
            for (int x = 0; x < 8; x++)
            {

                List<Cell> line = new List<Cell>();
                for (int y = 1; y < 8; y++)
                {
                    line.Add(cells[y - 1, x]);
                    if (cells[y, x].MarbleColor == cells[y - 1, x].MarbleColor)
                    {
                        if (y == 7) line.Add(cells[y, x]);
                        continue;
                    }
                    if (line.Count() >= 3) lines.Add(line);
                    line = new List<Cell>();
                }
                if (line.Count() >= 3) lines.Add(line);

            }

            // if no mathec return false
            if (lines.Count == 0)
            {
                return false;
            }

            // Generate bonuses and destroy matches
            
            // list to store lines that should be destroyed after check
            List<List<Cell>> toDestroy = new List<List<Cell>>();
            // Find intersections between matches
            foreach (List<Cell> line in lines)
            {
                foreach (List<Cell> otherLine in lines)
                {
                    // if the same line go on
                    if (line == otherLine) continue;

                    // get the intersection cell, delete it from lines, mark lines for destruction and go on
                    var tmp = line.Intersect(otherLine);
                    if (tmp.Count() > 0)
                    {
                        Cell intersection = tmp.First();
                        line.Remove(intersection);
                        otherLine.Remove(intersection);
                        toDestroy.Add(line);
                        toDestroy.Add(otherLine);
                        intersection.PlaceBonus(Bonus.Bomb);
                    }

                }
            }
            // destroy all intersecting matches and remove them from the list of matches
            foreach (List<Cell> line in toDestroy)
            {
                foreach (Cell cell in line)
                {
                    AddDestroyer(cell);
                    score += cell.Destroy();
                }
                // remove this lines from list of initial mathes not to check them again
                lines.Remove(line);
            }

            // Check for matches that generate bonuses marbles
            foreach (List<Cell> line in lines)
            {
                // define cell for bonus. If match contains marble that was swapped (moved last)
                // put bonus there, if not, put it in the default position

                Cell placeForBonus;
                if (line.Contains(wasSpapped1)) placeForBonus = wasSpapped1;
                else if (line.Contains(wasSpapped2)) placeForBonus = wasSpapped2;
                else placeForBonus = line[0];
                AddDestroyer(placeForBonus);

                // Add bonus LINE
                if (line.Count() == 4)
                {
                    // if horizontal line
                    if (line[0].Row == line[1].Row)
                    {
                        placeForBonus.PlaceBonus(Bonus.LineHor);
                        line.Remove(placeForBonus);
                        continue;

                    }
                    // for vertical line
                    placeForBonus.PlaceBonus(Bonus.LineVer);
                    line.Remove(placeForBonus);
                    continue;
                }
                // Add bonus BOMB
                else if (line.Count >= 5)
                {
                    placeForBonus.PlaceBonus(Bonus.Bomb);
                    line.Remove(placeForBonus);
                    continue;
                }

            }

            wasSpapped1 = null;
            wasSpapped2 = null;

            // Destroy matches that are left
            foreach (List<Cell> line in lines)
            {
                foreach (Cell cell in line)
                {
                    AddDestroyer(cell);
                    score += cell.Destroy();
                }
            }
            // when there are matches return true
            return true;
        }

        // Iterates through the board, finds empty spaces and drops the marbles from above to that places
        public void DropMarbles()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 7; y > 0; y--)
                {
                    
                    // if cell with marble continue loop
                    if (!cells[y, x].isEmpty())
                    {
                        continue;
                    }

                    // if cell is empty find closest cell in the top and drop it to this empty cell
                    for (int i = y - 1; i >= 0; i--)
                    {
 
                        // if next cell on the top go higher
                        if (cells[i, x].isEmpty())
                        {
                            continue;
                        }

                        Cell tmpCell = cells[y, x];
                        cells[i, x].DropTo(y);
                        cells[y, x] = cells[i, x];
                        cells[i, x] = tmpCell;
                        break;
 
                    }

                }
            }
            
        }

        // iterate over every cell in the array and spawn marbles when the cells are empty
        public void SpawnMarbles()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (cells[y, x].isEmpty())
                    {
                        MarbleColor color = (MarbleColor)random.Next(0, 5);
                        Cell cell = new Cell(spriteBatch, color, y, x, textures);
                        cells[y, x] = cell;
                    }
                }
            }
        }

        // Adds destroyers to the board
        public void AddDestroyer(Cell cell)
        {
            // check what type of destroyers we are adding and add

            switch (cell.bonus)
            {
                
                case (Bonus.LineHor):
                    // one flies to the left and other to the right
                    destroyers.Add(new Destroyer(spriteBatch, cell.Row, cell.Column, textures, new Vector2(0, cell.Row), DestroyerType.Fireball));
                    destroyers.Add(new Destroyer(spriteBatch, cell.Row, cell.Column, textures, new Vector2(7, cell.Row), DestroyerType.Fireball));
                    break;
                case (Bonus.LineVer):
                    destroyers.Add(new Destroyer(spriteBatch, cell.Row, cell.Column, textures, new Vector2(cell.Column, 0), DestroyerType.Fireball));
                    destroyers.Add(new Destroyer(spriteBatch, cell.Row, cell.Column, textures, new Vector2(cell.Column, 7), DestroyerType.Fireball));

                    break;
                case (Bonus.Bomb):
                    destroyers.Add(new Destroyer(spriteBatch, cell.Row, cell.Column, textures, new Vector2(cell.Column, 0), DestroyerType.Bomb));
                    break;
            }
            
        }

        public void UpdateDestroyers(GameTime gameTime)
        {
            List<Cell> addDestroyersHere = new List<Cell>();
            foreach (Destroyer destroyer in destroyers)
            {
                // destroy cells as destroyer reaches new positions
                if (destroyer.Update(gameTime))
                {
                    switch(destroyer.destroyerType)
                    {
                        case DestroyerType.Fireball:
                            //cells[destroyer.Row, destroyer.Column].Destroy();
                            addDestroyersHere.Add(cells[destroyer.Row, destroyer.Column]);

                            break;
                        case DestroyerType.Bomb:
                            foreach (Point point in destroyer.toDestroyByBomb)
                            {
                                //cells[point.Y, point.X].Destroy();
                                addDestroyersHere.Add(cells[point.Y, point.X]);
                            }
                            break;
                    }
                    
                }
                isAnimating = true;
            }

            // Remove all destroyers that reached destination
            destroyers.RemoveAll(destroyer => destroyer.IsFinished);

            // Add new destroyers if needed (some cells were destroyed and they had bonus in them)
            foreach (Cell cell in addDestroyersHere)
            {
                AddDestroyer(cell);
                score += cell.Destroy();
            }
        }

        public void DrawDestroyers()
        {
            foreach (Destroyer destroyer in destroyers)
            {
                destroyer.Draw();
            }

        }
        
    }
}
