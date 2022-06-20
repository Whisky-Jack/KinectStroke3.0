using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MeepEngine
{
    public class EntBackground
        : Entity
    {
        public EntBackground(Game game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            Enabled = true;
            Visible = true;
            
            // Set sprite parameters
            sprite = Assets.nosprite;
            imageAngle = 0f;
            imageScale = 1f;
            layer = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        int bgWidth = 1805;
        float index = 0;
        float scrollSpeed = 0.25f;
        

        public override void Create()
        {

        }

        public override void Destroy()
        {

        }

        public override void Draw(GameTime gameTime)
        {
            index += scrollSpeed;
            if (index >= bgWidth)
                index = 0;

            Main.DrawSprite(Assets.sprBackground, new Vector2(Main.roomWidth / 2 - (float)Math.Floor(index), Main.roomHeight / 2), 0f, 1f, 1f, 1f);
            Main.DrawSprite(Assets.sprBackground, new Vector2(Main.roomWidth / 2 + bgWidth + 1 - (float)Math.Floor(index), Main.roomHeight / 2), 0f, 1f, 1f, 1f);

            base.Draw(gameTime);
        }
    }
}