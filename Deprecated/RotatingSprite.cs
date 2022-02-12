using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Match3
{
    class RotatingSprite
    {
        public Texture2D Texture;
        private float angle;

        public RotatingSprite(Texture2D texture)
        {
            Texture = texture;
            angle = 0f;
        }

        public void Update()
        {
            angle += 0.05f;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width/2, Texture.Height/2);
            //spriteBatch.Begin();
            spriteBatch.Draw(Texture, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);
            //spriteBatch.End(); 


        }




    }
}
