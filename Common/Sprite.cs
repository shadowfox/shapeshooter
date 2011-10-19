using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public class Sprite
    {
        //The asset name for the Sprite's Texture.
        public string AssetName;

        //The Size of the Sprite (with scale applied).
        public Rectangle Size;

        //The amount to increase/decrease the size of the original sprite. 
        private float scale = 1.0f;

        // The current position of the Sprite.
        public Vector2 Position = new Vector2(0, 0);

        // The angle of rotation of the sprite.
        private float rotation = 0f;

        // The texture object used when drawing the sprite.
        public Texture2D texture;

        // Anchor point that the sprite will be drawn at. Default is 0, 0.
        private Vector2 origin = new Vector2(0, 0);

        // When the scale is modified through he property, the Size of the 
        // sprite is recalculated with the new scale applied.
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;

                //Recalculate the Size of the Sprite with the new scale.
                Size = new Rectangle(0, 0, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                if (rotation < -MathHelper.TwoPi)
                {
                    rotation = MathHelper.TwoPi;
                }
                if (rotation > MathHelper.TwoPi)
                {
                    rotation = -MathHelper.TwoPi;
                }
            }
        }

        public Vector2 Origin
        {
            get { return this.origin; }
            set { this.origin = value; }
        }

        public int Width
        {
            get { return texture.Width; }
        }

        public int Height
        {
            get { return texture.Height; }
        }

        public Sprite() { }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            this.LoadContent(texture);
        }

        // Load the texture for the sprite using the Content Pipeline.
        public void LoadContent(ContentManager contentManager, string assetName)
        {
            this.texture = contentManager.Load<Texture2D>(assetName);
            this.AssetName = assetName;
            Size = new Rectangle(0, 0, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
        }

        public void LoadContent(Texture2D texture)
        {
            this.texture = texture;
            this.AssetName = texture.Name;
            Size = new Rectangle(0, 0, (int)(texture.Width * Scale), (int)(texture.Height * Scale));
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
        }

        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(texture, Position,
            new Rectangle(0, 0, texture.Width, texture.Height),
            Color.White, this.rotation, origin, Scale, SpriteEffects.None, 0);
        }

        // Draw the sprite to the screen with a given position.
        public void Draw(SpriteBatch theSpriteBatch, Vector2 position)
        {
            theSpriteBatch.Draw(texture, position,
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White, this.rotation, origin, Scale, SpriteEffects.None, 0);
        }

        // Draw the sprite to the screen with a given rotation.
        public void Draw(SpriteBatch theSpriteBatch, float rotation)
        {
            theSpriteBatch.Draw(texture, Position,
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White, rotation, origin, Scale, SpriteEffects.None, 0);
        }
    }
}
