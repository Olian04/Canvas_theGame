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
        #region Values
        private Game1.okColors outerColorEnum, innerColorEnum;
        private AABB dimensions;
        private Rectangle innerRectangle;
        private Texture2D texture;
        private Vector2 velocity, maxVelocity;
        private float jumpStrength, deceleration, verticalSpeedGround, verticalSpeedAir;
        private bool onGround;
        private int innerBorder;

        public void Init(ContentManager Content) {
            texture = Content.Load<Texture2D>("square.png");
            velocity = new Vector2(0);
            maxVelocity = new Vector2(7, 10); //X=horizontal, Y=vertical.
            deceleration = 2;
            verticalSpeedGround = 0.5f;
            verticalSpeedAir = 0.25f;
            jumpStrength = 17;
            innerBorder = 5;
            onGround = false;
        }
        #endregion

        public Player(Rectangle dimensions, Game1.okColors outerColorEnum, Game1.okColors innerColorEnum)
        {
            this.dimensions = new AABB(dimensions);
            this.outerColorEnum = outerColorEnum;
            this.innerColorEnum = innerColorEnum;
        }

        public void Update(List<Barrier> barriers, KeyboardState ks, KeyboardState oldks)
        {
            updateInput(ks, oldks);
            
            updateVelocity();
            
            updateColission(barriers);

            updateInnerRectangel();

            Level.Update(dimensions);
        }

        #region Sub Updates
        private void updateInnerRectangel() {
            this.innerRectangle = this.dimensions.getBoundingBox();
            innerRectangle.X += innerBorder;
            innerRectangle.Y += innerBorder;
            innerRectangle.Width -= innerBorder * 2;
            innerRectangle.Height -= innerBorder * 2;
        }

        private void updateColission(List<Barrier> barriers) {
            onGround = false;
            foreach (Barrier b in barriers) {
                if (Game1.getPrimraryColor() == b.getColor()) {
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
            if (onGround)
            {
                if (ks.IsKeyDown(Keys.Right))
                {
                    alterVelocityAdition(new Vector2(verticalSpeedGround + deceleration, 0));
                }
                if (ks.IsKeyDown(Keys.Left))
                {
                    alterVelocityAdition(new Vector2(-(verticalSpeedGround + deceleration), 0));
                }
            }
            else
            {
                if (ks.IsKeyDown(Keys.Right))
                {
                    alterVelocityAdition(new Vector2(verticalSpeedAir + deceleration, 0));
                }
                if (ks.IsKeyDown(Keys.Left))
                {
                    alterVelocityAdition(new Vector2(-(verticalSpeedAir + deceleration), 0));
                }
            }
            if (onGround && ks.IsKeyDown(Keys.Up))
            {
                velocity.Y = -jumpStrength; //TODO: FIX THIS WORKAROUND!
                //alterPositionAdition(new Point(0,-3)); //for debug
            }
            if (ks.IsKeyDown(Keys.Down))
            {
                //alterPositionAdition(new Point(0,3)); //for debug
            }
        }

        private void updateVelocity() {
            if (onGround) {
                if (Math.Abs(velocity.X) < 1f)
                {
                    velocity.X = 0;
                }
                else if (velocity.X > deceleration)
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
            }
            else
            {
                if (Math.Abs(velocity.X) < 0.1f) {
                    velocity.X = 0;
                }
                else if (velocity.X > 0)
                {
                    alterVelocityAdition(new Vector2(-(0.1f), 0));
                }
                else if (velocity.X < 0)
                {
                    alterVelocityAdition(new Vector2((0.1f), 0));
                }
            }

            if (velocity.Y < maxVelocity.Y)
            {
                alterVelocityAdition(new Vector2(0, Game1.getGravity()));
            }

            alterPositionAdition(velocity);
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dimensions.getBoundingBox(), Game1.getAvailableColors()[outerColorEnum]);
            spriteBatch.Draw(texture, innerRectangle, Game1.getAvailableColors()[innerColorEnum]);
        }

        #region set/get
        public void setOuterColor(Game1.okColors colorEnum) {
            this.outerColorEnum = colorEnum;
        }
        public Game1.okColors getOuterColor() {
            return outerColorEnum;
        }
        public void setInnerColor(Game1.okColors colorEnum)
        {
            this.innerColorEnum = colorEnum;
        }
        public Game1.okColors getInnerColor()
        {
            return innerColorEnum;
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
        #endregion

        #region Alter pos/veloc
        public void alterPositionAdition(Vector2 position) {
            this.dimensions.alterPositionAdition(position);
        }
        public void alterVelocityAdition(Vector2 _velocity) {
            if (Math.Abs((this.velocity + _velocity).X) < maxVelocity.X)
            {
                this.velocity.X += _velocity.X;
            }
            else
            {
                this.velocity.X = maxVelocity.X * ((this.velocity + _velocity).X > 0 ? 1 : -1);
            }

            if ((this.velocity + _velocity).Y > 0)
            { //Falling
                if (Math.Abs((this.velocity + _velocity).Y) < maxVelocity.Y)
                {
                    this.velocity.Y += _velocity.Y;
                }
                else
                {
                    this.velocity.Y = maxVelocity.Y * ((this.velocity + _velocity).Y > 0 ? 1 : -1);
                }
            }
            else
            {
                this.velocity.Y += _velocity.Y;
            }
        }
        public void alterVelocityMultiplication(Vector2 _velocity) {
            this.velocity *= _velocity;
        }
        #endregion
    }
}
