using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace Match3
{
    class Cell
    {
        public Type Type;
        private Texture2D texture;
        private Game1 game;
        private RotatingSprite rotatingTexture;
        private AnimatedSprite explodingMarble;
        public Vector2 position;
        public Vector2 destination;
        public int cellY;
        public int cellX;

        public enum State
        {
            Normal,
            Selected,
            Moving,
            Exploding
        }

        public State state;

        public Cell(Type type, Game1 game, Vector2 position, int cellY, int cellX)
        {
            Type = type;
            this.game = game;
            this.InitializeCell();
            state = State.Normal;
            rotatingTexture = new RotatingSprite(texture);
            explodingMarble = game.explosion;
            this.position = position;
            this.cellX = cellX;
            this.cellY = cellY;
            
        }

        private void InitializeCell()
        {
            switch (Type)
            {
                case Type.Orange:
                    texture = game.marbleOrange;
                    break;
                case Type.Green:
                    texture = game.marbleGreen;
                    break;
                case Type.Blue:
                    texture = game.marbleBlue;
                    break;
                case Type.Grey:
                    texture = game.marbleGrey;
                    break;
                case Type.Pink:
                    texture = game.marblePink;
                    break;
            }
        }

        public void DrawCell(SpriteBatch spriteBatch, Vector2 location)
        {
            switch(state)
            {
                case State.Normal:
                    spriteBatch.Draw(texture, position, Color.White);
                    break;
                case State.Selected:
                    rotatingTexture.Update();
                    rotatingTexture.Draw(spriteBatch, new Vector2(location.X + texture.Width/2, location.Y + texture.Height/2));
                    //spriteBatch.Draw(texture, location, Color.Blue);
                    break;
                case State.Moving:
                    Move(destination, spriteBatch);
                    break;
                case State.Exploding:
                    if (explodingMarble.Update())
                    {
                        position = new Vector2(0, 0);
                        state = State.Normal;
                        break;
                    }
                    explodingMarble.Draw(spriteBatch, position);
                    break;
            }
            
        }

        public void Move(Vector2 newPosition, SpriteBatch spriteBatch)
        {
            Vector2 movement = new Vector2(newPosition.X - position.X, newPosition.Y - position.Y);
            // move horizontally
            if (movement.Y == 0)
            {
                // move right
                if (movement.X > 0)
                {
                    position = new Vector2(position.X + 5, position.Y);
                    spriteBatch.Draw(texture, position, Color.White);
                    if (position.X >= newPosition.X)
                    {
                        position.X = newPosition.X;
                        state = State.Normal;
                        return;
                    }
                }
                // move left
                else
                {
                    position = new Vector2(position.X - 5, position.Y);
                    spriteBatch.Draw(texture, position, Color.White);
                    if (position.X <= newPosition.X)
                    {
                        position.X = newPosition.X;
                        state = State.Normal;
                        return;
                    }
                }
            }
            // move vertically
            else
            {
                // move down
                if (movement.Y > 0)
                {
                    position = new Vector2(position.X, position.Y + 5);
                    spriteBatch.Draw(texture, position, Color.White);
                    if (position.Y >= newPosition.Y)
                    {
                        position.Y = newPosition.Y;
                        state = State.Normal;
                        return;
                    }
                }
                // move UP
                else
                {
                    position = new Vector2(position.X, position.Y - 5);
                    spriteBatch.Draw(texture, position, Color.White);
                    if (position.Y <= newPosition.Y)
                    {
                        position.Y = newPosition.Y;
                        state = State.Normal;
                        return;
                    }
                    return;
                }
            }

        }

        public void Explode()
        {

        }


    }
}
