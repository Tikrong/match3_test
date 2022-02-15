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
        private Vector2 destination;
        private Vector2 movementVector;
        private AnimatedSprite explosion;

        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private CellState state;
        private float rotationAngle = 0f;

        public int Row { get; set; }
        public int Column { get; set; }

        public MarbleColor MarbleColor { get; set; }

        public Cell(SpriteBatch spriteBatch, MarbleColor color, int row, int column, Texture2D texture, Texture2D explosion)
        {
            this.spriteBatch = spriteBatch;
            MarbleColor = color;
            Row = row;
            Column = column;
            this.texture = texture;
            this.explosion = new AnimatedSprite(explosion, 2, 4);
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
                case CellState.Selected:
                    rotationAngle += 0.1f;
                    return false;
                    //break;
                case CellState.Moving:
                    MoveToAnimation();
                    break;
                case CellState.Exploding:
                    if (explosion.Update())
                    {
                        state = CellState.Empty;
                        MarbleColor = MarbleColor.Empty;
                    }
                    break;
                case CellState.Empty:
                    return false;


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
                case CellState.Moving:
                    spriteBatch.Draw(texture, position, Color.White);
                    break;
                case CellState.Exploding:
                    // play explosion animation
                    explosion.Draw(spriteBatch, position);
                    break;
                case CellState.Empty:
                    break;
                default:
                    spriteBatch.Draw(texture, position, Color.White);
                    break;
            }
            
        }

        public void SelectCell()
        {
            state = CellState.Selected;
        }

        public void UnselectCell()
        {
            state = CellState.Neutral;
        }

        public bool isNeigbour(Cell otherCell)
        {
            if (otherCell.Row == Row && Math.Abs(otherCell.Column - Column) == 1)
            {
                return true;
            }
            else if (Math.Abs(otherCell.Row - Row) == 1 && otherCell.Column == Column)
            {
                return true; 
            }
            return false;
        }

        public bool isEmpty()
        {
            return state == CellState.Empty;
        }

        // moving the cell to new given position
        public bool MoveTo(int destinationRow, int destinationColumn)
        {
            //1. calculate and save destination for movement
            destination = new Vector2(destinationColumn * Constants.cellSize, destinationRow * Constants.cellSize);
            //2. calculate movement vector
            movementVector = new Vector2(destinationColumn - Column, destinationRow - Row);
            //3. change state for movement
            state = CellState.Moving;
            //4. change data about row and column
            Row = destinationRow;
            Column = destinationColumn;
            
            return false;
        }

        private void MoveToAnimation()
        {
            // Move horizontal
            if (movementVector.Y == 0)
            {
                // Move right
                if (movementVector.X > 0)
                {
                    position = new Vector2(position.X + 0.5f, position.Y);
                    // if destination reached
                    if (position.X >= destination.X)
                    {
                        position.X = destination.X;
                        state = CellState.Neutral;
                        return;
                    }
                }
                // Move left
                else if (movementVector.X < 0)
                {
                    position = new Vector2(position.X - 0.5f, position.Y);
                    // if destination reached
                    if (position.X <= destination.X)
                    {
                        position.X = destination.X;
                        state = CellState.Neutral;
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
                    position = new Vector2(position.X, position.Y - 0.5f);
                    // if destination reached
                    if (position.Y <= destination.Y)
                    {
                        position.Y = destination.Y;
                        state = CellState.Neutral;
                        return;
                    }
                }

                // move down
                else if (movementVector.Y > 0)
                {
                    position = new Vector2(position.X, position.Y + 0.5f);
                    // if destination reached
                    if (position.Y >= destination.Y)
                    {
                        position.Y = destination.Y;
                        state = CellState.Neutral;
                        return;
                    }
                }
            }

        }

        // drops the cell into given location
        public void DropTo(int row)
        {
            //1. calculate and save destination for movement
            destination = new Vector2(Column * Constants.cellSize, row * Constants.cellSize);
            //2. calculate movement vector
            movementVector = new Vector2(0, row - Row);
            //3. change state for movement
            state = CellState.Moving;
            //4. change data about row and column
            Row = row;

        }

        public void Destroy()
        {
            // change state for destruction
            state = CellState.Exploding;

        }

       




    }
}
