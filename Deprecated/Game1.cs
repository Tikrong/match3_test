using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Match3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // sprites
        public Texture2D marbleGrey;
        public Texture2D marbleBlue;
        public Texture2D marbleOrange;
        public Texture2D marbleGreen;
        public Texture2D marblePink;
        public Texture2D background;

        //fonts
        private SpriteFont scoreFont;

        // game core
        private int score = 0;

        // animations
        public AnimatedSprite explosion;
        private RotatingSprite rotatingMarble;


        // for testing
        private Cell testCell;
        private Board board;

        private MouseState previousState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferHeight = 512;
            _graphics.PreferredBackBufferWidth = 512;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // loading content
            background = Content.Load<Texture2D>("stars");
            marbleOrange = Content.Load<Texture2D>("orange");
            marbleBlue = Content.Load<Texture2D>("blue");
            marbleGrey = Content.Load<Texture2D>("grey");
            marbleGreen = Content.Load<Texture2D>("green");
            marblePink = Content.Load<Texture2D>("pink");

            scoreFont = Content.Load<SpriteFont>("scoreFont");

            Texture2D texture = Content.Load<Texture2D>("explosion");
            explosion = new AnimatedSprite(texture, 2, 4);

            rotatingMarble = new RotatingSprite(marbleGrey);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (board == null)
            {
                board = new Board(this);
            }

            //testCell = new Cell(Type.Orange, this);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            score++;
            explosion.Update();

            rotatingMarble.Update();

            // Control
            MouseState currentState = Mouse.GetState();

            if (currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released)
            {
                board.MouseClick(currentState);
            }
            previousState = currentState;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Drawing sprites

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            //_spriteBatch.Draw(marbleRed, new Vector2(10, 50), Color.White);
            //_spriteBatch.Draw(marbleBlue, new Vector2(400, 50), Color.White);

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    _spriteBatch.Draw(marblePink, new Rectangle(50 * x, 50 * y, 100,100), Color.White);
                }
            }


            // Draw score
            _spriteBatch.DrawString(scoreFont, "Score: " + score, new Vector2(350, 100), Color.White);
            //testCell.DrawCell(_spriteBatch, new Vector2(0, 0));
            _spriteBatch.End();
            //explosion.Draw(_spriteBatch, new Vector2(200, 200));

            //rotatingMarble.Draw(_spriteBatch, new Vector2(200, 200));

            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, 1120, 1120), Color.White);
            board.DrawBoard(_spriteBatch);
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
