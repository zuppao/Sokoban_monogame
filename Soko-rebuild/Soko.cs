using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Soko_rebuild.GameObjects;
using Soko_rebuild.GameObjects.Interfaces;

namespace Soko_rebuild
{
    public class Soko : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;
        double elapsedTime_Milliseconds = 0;
        const float frameRate_Millisecon = 1000 / 7; // desired FPS
        // 24 fps => 1000/24 => 1 frame each 41,6 ms

        int currentLevel;// { get; set; }
        Level level;// { get; set; }

        Dictionary<GameObjectType, Texture2D> levelTexture2D;
        readonly List<IDrawableObject> drawableGameObjectList;
        readonly IEnumerable<Keys> defaultMovementKeys;

        //KeyboardState keyboardState;

        public Soko()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            drawableGameObjectList = new List<IDrawableObject>();
            levelTexture2D = new Dictionary<GameObjectType, Texture2D>();
            defaultMovementKeys = new Keys[] {Keys.Up, Keys.Down, Keys.Left, Keys.Right };
        }

        protected override void Initialize()
        {
            currentLevel = 1;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";
            spriteBatch = new SpriteBatch(GraphicsDevice);

            levelTexture2D.Add(GameObjectType.Wall, Content.Load<Texture2D>("Sprites/wall"));
            levelTexture2D.Add(GameObjectType.Slot, Content.Load<Texture2D>("Sprites/slot"));
            levelTexture2D.Add(GameObjectType.Box, Content.Load<Texture2D>("Sprites/box"));
            levelTexture2D.Add(GameObjectType.Player, Content.Load<Texture2D>("Sprites/player"));

            level = new Level(levelTexture2D);

            drawableGameObjectList.Add(level);
            LoadLevel();
        }


        void LoadLevel()
        {
            var levelToLoad = $"Content/Levels/Level{currentLevel:000}.txt";
            if (!File.Exists(levelToLoad))
            {
                Exit();
            }
            else
            {
                var levelContent = File.ReadAllLines(levelToLoad);
                level.LoadLevel(levelContent);
            }
        }




        

        protected override void Update(GameTime gameTime)
        {
            elapsedTime_Milliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime_Milliseconds < frameRate_Millisecon)
                return;

            elapsedTime_Milliseconds = 0;



            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            var pressedKey = Keyboard.GetState().GetPressedKeys()?.FirstOrDefault() ?? Keys.None;

            if (defaultMovementKeys.Contains(pressedKey))
            {
                level.MovePlayer(pressedKey);
                if(level.GameFinished)
                {
                    currentLevel++;
                    LoadLevel();
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (IDrawableObject obj in drawableGameObjectList)
            {
                obj.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}