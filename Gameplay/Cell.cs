using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Match3Test
{
    // this class handles basic logic and animation of Marble and stores data about current marble
    class Cell: IEquatable<Cell>
    {
        private Vector2 position;
        private Vector2 destination;
        private Vector2 movementVector;
        private AnimatedSprite explosion;
        private float movementSpeed;

        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Dictionary<Textures, Texture2D> textures;
        private CellState state;
        private float rotationAngle = 0f;
        private float opacity = 0.01f;
        private int scoreFromMarble = 1;

        public int Row { get; set; }
        public int Column { get; set; }
        public Bonus bonus { get; set; }

        public MarbleColor MarbleColor { get; set; }

        public Cell(SpriteBatch spriteBatch, MarbleColor color, int row, int column, Dictionary<Textures, Texture2D> textures)
        {
            this.spriteBatch = spriteBatch;
            MarbleColor = color;
            Row = row;
            Column = column;
            position = new Vector2(column * Constants.cellSize, row * Constants.cellSize);
            movementSpeed = Constants.MarbleMovementSpeed;
            state = CellState.FadeIn;
            bonus = Bonus.None;
            this.textures = textures;
            texture = textures[(Textures)color];
            explosion = new AnimatedSprite(textures[Textures.Explosion], 2, 4);
        }

        // returns true if animation is running and false otherwise (rotation is not an animation here)
        public bool Update()
        {
            if (state == CellState.Neutral)
            {
                scoreFromMarble = 1;
                return false;
            }
            switch(state)
            {
                case CellState.Selected:
                    rotationAngle += 0.1f;
                    return false;
                case CellState.Moving:
                    MoveToAnimation();
                    break;
                case CellState.Exploding:
                    // update returns true when animation of explosion is finished
                    if (explosion.Update())
                    {
                        state = CellState.Empty;
                        MarbleColor = MarbleColor.Empty;
                    }
                    break;
                case CellState.Empty:
                    return false;
                case CellState.FadeIn:
                    opacity += 0.08f;
                    if (opacity >= 1f)
                    {
                        state = CellState.Neutral;
                        break;
                    }
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
                    spriteBatch.Draw(texture, position + new Vector2(texture.Width/2, texture.Height/2), sourceRectangle, Color.White, rotationAngle, origin, 1.0f, SpriteEffects.None, 1);
                    DrawBonus();
                    break;
                case CellState.Moving:
                    spriteBatch.Draw(texture, position, Color.White);
                    DrawBonus();
                    break;
                case CellState.Exploding:
                    // play explosion animation
                    explosion.Draw(spriteBatch, position);
                    break;
                case CellState.Empty:
                    break;
                case CellState.FadeIn:
                    spriteBatch.Draw(texture, position, Color.White * opacity);
                    if (opacity >= 1)  opacity = 1f; 
                    break;
                default:
                    spriteBatch.Draw(texture, position, Color.White);
                    DrawBonus();
                    break;
            }
            
        }

        private void DrawBonus()
        {
            switch(bonus)
            {
                case (Bonus.None):
                    break;
                case (Bonus.Bomb):
                    spriteBatch.Draw(textures[Textures.Bomb], position, Color.White);
                    break;
                case (Bonus.LineHor):
                    spriteBatch.Draw(textures[Textures.LineHor], position, Color.White);
                    break;
                case (Bonus.LineVer):
                    spriteBatch.Draw(textures[Textures.LineVer], position, Color.White);
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

        // returns true if other cell is next to this cell horizontally or vertically
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

        public void MakeEmpty()
        {
            state = CellState.Empty;
        }

        // moving the cell to new given position
        public bool MoveTo(int destinationRow, int destinationColumn)
        {
            //1. calculate and save destination for movement
            destination = new Vector2(destinationColumn, destinationRow);
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
            /* move by modifying position using movement vector, when destination reached
             change state for neutral*/
            position += movementVector * Constants.MarbleMovementSpeed;
            
            if ((int)(position.X + Constants.cellSize / 2) / Constants.cellSize == destination.X 
                && (int)(position.Y + Constants.cellSize / 2)  / Constants.cellSize == destination.Y)
 
            {
                position.X = destination.X * Constants.cellSize;
                position.Y = destination.Y * Constants.cellSize;
                state = CellState.Neutral;
                return;
            }
        }

        // drops the cell into given location
        public void DropTo(int row)
        {
            //1. calculate and save destination for movement
            destination = new Vector2(Column, row);
            //2. calculate movement vector
            movementVector = new Vector2(0, row - Row);
            //3. change state for movement
            state = CellState.Moving;
            //4. change data about row and column
            Row = row;

        }

        // destroys the cell and returns its score
        public int Destroy()
        {
            // change state for destruction
            bonus = Bonus.None;
            int score = scoreFromMarble;
            scoreFromMarble = 0;
            state = CellState.Exploding;
            return score;

        }

        public void PlaceBonus(Bonus bonus)
        {
            this.bonus = bonus;
        }

        // methods to compare cells to be able to use intersect on lists of cells
        public bool Equals(Cell other)
        {
            if (other is null)
                return false;

            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj) => Equals(obj as Cell);
        public override int GetHashCode() => (Row, Column).GetHashCode();

    }
}
