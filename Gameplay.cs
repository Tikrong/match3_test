﻿using System;
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
        private GameOver gameOver;
        private float timeLeft;
        private MainGame game;



        public Gameplay(SpriteBatch spriteBatch, Dictionary<Textures, Texture2D> textures, SpriteFont font, MainGame game)
        {
            this.spriteBatch = spriteBatch;
            this.textures = textures;
 
            board = new Board(this.textures, this.spriteBatch);
            state = GameState.CheckLines;
            score = new Score(spriteBatch, font);
            gameOver = new GameOver(spriteBatch, font, textures);
            timeLeft = 61f;
            this.game = game;
            
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
                    case GameState.GameOver:
                        if (gameOver.Update())
                        {
                            game.ChangeState(GameState.MainMenu);
                        }
                        break;

                }
                
            }
            board.Update();
            board.UpdateDestroyers(gameTime);
            // keep track of time
            timeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeLeft <= 1)
            {
                state = GameState.GameOver;
            }
                


        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(textures[Textures.BackgroundPlay], new Rectangle(0, 0, Constants.screenWidth, Constants.screenHeight), Color.White);
            switch (state)
            {
                case GameState.GameOver:
                    gameOver.Draw(board.score);
                    break;
                default:
                    
                    board.Draw();
                    board.DrawDestroyers();
                    // draw score on the screen and time
                    score.Draw(gameTime, board.score, (int)timeLeft);
                    break;
            }
            
        }


    }
}
