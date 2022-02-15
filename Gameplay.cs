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


        public Gameplay(SpriteBatch spriteBatch, Dictionary<Textures, Texture2D> textures)
        {
            this.spriteBatch = spriteBatch;
            this.textures = textures;
            board = new Board(this.textures, this.spriteBatch);
            board.FindMatches();
        }

        public void Update()
        {
            if (!board.isAnimating)
            {
                board.UserInput();
            }
            board.Update();
            
        }

        public void Draw()
        {
            board.Draw();
        }


    }
}
