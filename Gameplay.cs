using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Match3Test
{

    // This class will handle gameplay screen and will manage the board states, UI, timer
    class Gameplay
    {
        private Board board;
        private SpriteBatch spriteBatch;
        Dictionary<Textures, Texture2D> textures;
        private GameState state;
        private Score score;
        


        public Gameplay(SpriteBatch spriteBatch, Dictionary<Textures, Texture2D> textures, SpriteFont font)
        {
            this.spriteBatch = spriteBatch;
            this.textures = textures;
 
            board = new Board(this.textures, this.spriteBatch);
            state = GameState.CheckLines;
            score = new Score(spriteBatch, font);
            
        }

        public void Update(GameTime gameTime)
        {
            if (!board.isAnimating)
            {
                switch(state)
                {
                    case GameState.Input:
                        if (board.UserInput())
                        {
                            state = GameState.CheckAfterSwap;
                            break;
                        }
                        break;
                    case GameState.CheckAfterSwap:
                        if (board.FindMatches())
                        {
                            state = GameState.DropCells;
                            break;
                        }
                        board.SwapCellsBack();
                        state = GameState.Input;
                        break;
                    case GameState.CheckLines:
                        
                        if (board.FindMatches())
                        {
                            state = GameState.DropCells;
                            break;
                        }
                        state = GameState.Input;
                
                        break;
                    case GameState.DropCells:
                        board.DropMarbles();
                        state = GameState.SpawnCells;
                        break;
                    case GameState.SpawnCells:
                        board.SpawnMarbles();
                        state = GameState.CheckLines;
                        break;

                }
                
            }
            board.Update();
            board.UpdateDestroyers(gameTime);
            

        }

        public void Draw(GameTime gameTime)
        {
            board.Draw();
            board.DrawDestroyers();
            score.Draw(gameTime);
        }


    }
}
