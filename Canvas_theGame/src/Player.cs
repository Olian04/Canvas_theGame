using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Canvas_theGame.src
{
    class Player
    {
        private Color color;
        private AABB dimensions;
        private Texture2D texture;
        private Vector2 velocity;
        private Vector2 maxVelocity;
        private float deceleration, gravity, verticalSpeed;
        private bool onGround;

        public void Init(ContentManager Content) {
            texture = Content.Load<Texture2D>("square.png");
            velocity = new Vector2(0);
            maxVelocity = new Vector2(10, 8);
            deceleration = 3;
            gravity = 1;
            verticalSpeed = 1;
            onGround = false;
        }

        public Player(Rectangle dimensions, Color color)
        {
            this.dimensions = new AABB(dimensions);
            this.color = color;
        }

        public void Update(List<Barrier> barriers, KeyboardState ks, KeyboardState oldks)
        {
            updateInput(ks, oldks);
            
            updateVelocity();
            
            updateColission(barriers);

        }

        private void updateColission(List<Barrier> barriers) {
            onGround = false;
            foreach (Barrier b in barriers) {
                if (Game1.getSecondaryColor() != b.getColor()) {
                    if (dimensions.isAbove(b.getDimensions()))
                    {
                        dimensions.setPositionY(b.getDimensions().getBoundingBox().Top - dimensions.getBoundingBox().Height);
                        alterVelocityMultiplication(new Vector2(1, 0));
                        onGround = true;
                    }
                    if (dimensions.isBelow(b.getDimensions()))
                    {
                        dimensions.setPositionY(b.getDimensions().getBoundingBox().Bottom);
                        alterVelocityMultiplication(new Vector2(1, 0));
                    }

                    if (dimensions.isLeft(b.getDimensions()))
                    {
                        dimensions.setPositionX(b.getDimensions().getBoundingBox().Left - dimensions.getBoundingBox().Width);
                        alterVelocityMultiplication(new Vector2(0, 1));
                    }
                    if (dimensions.isRight(b.getDimensions()))
                    {
                        dimensions.setPositionX(b.getDimensions().getBoundingBox().Right);
                        alterVelocityMultiplication(new Vector2(0, 1));
                    }
                }
            }//foreach
        }

        private void updateInput(KeyboardState ks, KeyboardState oldks) {
            if (ks.IsKeyDown(Keys.Right))
            {
                alterVelocityAdition(new Vector2(verticalSpeed + deceleration, 0));
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                alterVelocityAdition(new Vector2(-(verticalSpeed + deceleration), 0));
            }
            if (onGround && ks.IsKeyDown(Keys.Up))
            {
                velocity.Y = -20; //TODO: FIX THIS WORKAROUND!
                //alterPositionAdition(new Point(0,-3)); //for debug
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                //alterPositionAdition(new Point(0,3)); //for debug
            }
        }

        private void updateVelocity() {
            if (velocity.X > deceleration)
            {
                alterVelocityAdition(new Vector2(-deceleration, 0));
            }
            else if (velocity.X < -deceleration)
            {
                alterVelocityAdition(new Vector2(deceleration, 0));
            }
            else if (velocity.X > 0)
            {
                alterVelocityAdition(new Vector2(-1, 0));
            }
            else if (velocity.X < 0)
            {
                alterVelocityAdition(new Vector2(1, 0));
            }

            if (velocity.Y < -maxVelocity.Y)
            {
                velocity.Y += gravity;
            }
            else
            {
                alterVelocityAdition(new Vector2(0, gravity));
            }
            alterPositionAdition(velocity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dimensions.getBoundingBox(), color);
        }



        public void setColor(Color color) {
            this.color = color;
        }
        public Color getColor() {
            return color;
        }
        public void setDimensions(Rectangle dimensions) {
            this.dimensions =  new AABB(dimensions);
        }
        public void setPosition(Vector2 position) {
            this.dimensions.setPosition(position);
        }
        public AABB getDimensions() {
            return dimensions;
        }
        public void alterPositionAdition(Vector2 position) {
            this.dimensions.alterPositionAdition(position);
        }
        public void alterVelocityAdition(Vector2 _velocity) {
            if (Math.Abs((this.velocity + _velocity).X) <= maxVelocity.X)
                this.velocity.X += _velocity.X;
            else
                this.velocity.X = maxVelocity.X * ((this.velocity + _velocity).X > 0 ? 1 : -1);

            if (Math.Abs((this.velocity + _velocity).Y) <= maxVelocity.Y)
                this.velocity.Y += _velocity.Y;
            else
                this.velocity.Y = maxVelocity.Y * ((this.velocity + _velocity).Y > 0 ? 1 : -1);
        }
        public void alterVelocityMultiplication(Vector2 _velocity) {
            this.velocity *= _velocity;
        }
    }
}
