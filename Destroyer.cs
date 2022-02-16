using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3Test
{
    class Destroyer
    {
        private Vector2 position;
        private Vector2 destination;
        private Vector2 movementVector;
        private float movementSpeed;

        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private double elapsedTime;
        //private CellState state;
        public DestroyerType destroyerType;

        public int Row { get; set; }
        public int Column { get; set; }

        public bool IsFinished;

        // coordinates of points in array to destroy by bomb
        public List<Point> toDestroyByBomb = new List<Point>();

        public Destroyer(SpriteBatch spriteBatch, int row, int column, Dictionary<Textures, Texture2D> textures, Vector2 destination, DestroyerType type)
        {
            this.spriteBatch = spriteBatch;
            Row = row;
            Column = column;
            texture = textures[Textures.Fireball];
            this.movementVector = new Vector2(destination.X - column, destination.Y - row);
            this.destination = new Vector2(destination.X * Constants.cellSize, destination.Y * Constants.cellSize);
            movementSpeed = 10f;
            position = new Vector2(Column * Constants.cellSize, Row * Constants.cellSize);
            IsFinished = false;
            destroyerType = type;
            elapsedTime = 0;
            
            
            if (destination.X == Column && destination.Y == Row && destroyerType != DestroyerType.Bomb)
            {
                IsFinished = true;
            }

        }

        // returns true when new cell is reached
        public bool Update(GameTime gameTime)
        {
            switch(destroyerType)
            {
                case (DestroyerType.Fireball):
                    int x = (int)position.X / Constants.cellSize;
                    int y = (int)position.Y / Constants.cellSize;

                    if (y == Row && x == Column)
                    {
                        return false;
                    }
                    Row = y;
                    Column = x;
                    return true;
                case (DestroyerType.Bomb):
                    elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (elapsedTime < 250)
                    {
                        return false;
                    }
                    // add all cells around the bomb to the array of points to destroy
                    for (int r = -1; r < 2; r++)
                    {
                        for (int c = -1; c < 2; c++)
                        {
                            if (r == 0 && c == 0)
                            {
                                continue;
                            }
                            Point point = new Point(Column + c, Row + r);
                            if (point.X >= 0 && point.X < 8 && point.Y >= 0 && point.Y < 8)
                            {
                                toDestroyByBomb.Add(point);
                            }    
                        }
                    }
                    IsFinished = true;
                    return true;




            }
            return false;
            
        }

        public void Draw()
        {
            if (destroyerType == DestroyerType.Bomb)
            {
                return;
            }
            MoveToAnimation();
            spriteBatch.Draw(texture, position, Color.White);
        }

        private void MoveToAnimation()
        {
            // Move horizontal
            if (movementVector.Y == 0)
            {
                // Move right
                if (movementVector.X > 0)
                {
                    position = new Vector2(position.X + movementSpeed, position.Y);
                    // if destination reached
                    if (position.X >= destination.X)
                    {
                        position.X = destination.X;
                        //state = CellState.Neutral;
                        IsFinished = true;
                        return;
                    }
                }
                // Move left
                else if (movementVector.X < 0)
                {
                    position = new Vector2(position.X - movementSpeed, position.Y);
                    // if destination reached
                    if (position.X <= destination.X)
                    {
                        position.X = destination.X;
                        //state = CellState.Neutral;
                        IsFinished = true;
                        return;
                    }
                }
            }

            // Move vertically
            else
            {
                // move up
                if (movementVector.Y < 0)
                {
                    position = new Vector2(position.X, position.Y - movementSpeed);
                    // if destination reached
                    if (position.Y <= destination.Y)
                    {
                        position.Y = destination.Y;
                        //state = CellState.Neutral;
                        IsFinished = true;
                        return;
                    }
                }

                // move down
                else if (movementVector.Y > 0)
                {
                    position = new Vector2(position.X, position.Y + movementSpeed);
                    // if destination reached
                    if (position.Y >= destination.Y)
                    {
                        position.Y = destination.Y;
                        //state = CellState.Neutral;
                        IsFinished = true;
                        return;
                    }
                }
            }

        }



    }
}
