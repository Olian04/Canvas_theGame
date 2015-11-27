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
        private Rectangle dimensions;
        private Texture2D texture;
        private Vector2 velocity;
        private Vector3 maxVelocity;
        private float deceleration, gravity, verticalSpeed;
        private bool onGround;

        public void Init(ContentManager Content) {
            texture = Content.Load<Texture2D>("square.png");
            velocity = new Vector2(0);
            maxVelocity = new Vector3(20, 10, 20); //Y = falla, X = hoppa
            deceleration = 3;
            gravity = 1;
            verticalSpeed = 1;
            onGround = false;
        }

        public Player(Rectangle dimensions, Color color)
        {
            this.dimensions = dimensions;
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
                if (b.getColor() != Game1.getSecondaryColor() && this.dimensions.Intersects(b.getDimensions())) {
                    relDirection relDir = relativDirection(dimensions, b.getDimensions());

                    if (relDir == relDirection.ABOVE)
                    {
                        dimensions.Y = b.getDimensions().Top - dimensions.Height;
                        onGround = true;
                    }
                    if (relDir == relDirection.BELOW)
                    {
                        dimensions.Y = b.getDimensions().Bottom;
                    }
                    
                    if (relDir == relDirection.LEFT)
                    {
                        dimensions.X = b.getDimensions().Left - dimensions.Width;
                    }
                    if (relDir == relDirection.RIGHT)
                    {
                        dimensions.X = b.getDimensions().Right;
                    }
                }
            }
        }

        enum relDirection {ABOVE, BELOW, RIGHT, LEFT, err};
        private relDirection relativDirection(Rectangle scource, Rectangle target)
        {
            Point[] scourceCorners = new Point[4], targetCorners = new Point[4];
            scourceCorners[0] = scource.Location; //Up & Left
            scourceCorners[1] = scource.Location + new Point(scource.Width, 0); //Up & Right
            scourceCorners[2] = scource.Location + new Point(0, scource.Height); //Down & Left
            scourceCorners[3] = scource.Location + scource.Size; //Down & Right
            targetCorners[0] = target.Location; //Up & Left
            targetCorners[1] = target.Location + new Point(target.Width, 0); //Up & Rights
            targetCorners[2] = target.Location + new Point(0, target.Height); //Down & Left
            targetCorners[3] = target.Location + target.Size; //Down & Right

            bool isAbove = false, isBelow = false, isLeft = false, isRight = false;

            if ((scourceCorners[0].Y < targetCorners[0].Y) &&
                (scourceCorners[1].Y < targetCorners[0].Y) &&
                (scourceCorners[2].Y > targetCorners[0].Y) &&
                (scourceCorners[3].Y > targetCorners[0].Y) &&
                (scourceCorners[0].Y < targetCorners[1].Y) &&
                (scourceCorners[1].Y < targetCorners[1].Y) &&
                (scourceCorners[2].Y > targetCorners[1].Y) &&
                (scourceCorners[3].Y > targetCorners[1].Y))
            {
                isAbove = true;
            }
            if ((scourceCorners[0].Y < targetCorners[2].Y) &&
                (scourceCorners[1].Y < targetCorners[2].Y) &&
                (scourceCorners[2].Y > targetCorners[2].Y) &&
                (scourceCorners[3].Y > targetCorners[2].Y) &&
                (scourceCorners[0].Y < targetCorners[3].Y) &&
                (scourceCorners[1].Y < targetCorners[3].Y) &&
                (scourceCorners[2].Y > targetCorners[3].Y) &&
                (scourceCorners[3].Y > targetCorners[3].Y))
            {
                isBelow = true;
            }
            if ((scourceCorners[0].X < targetCorners[0].X) &&
                (scourceCorners[1].X > targetCorners[0].X) &&
                (scourceCorners[2].X < targetCorners[0].X) &&
                (scourceCorners[3].X > targetCorners[0].X) &&
                (scourceCorners[0].X < targetCorners[2].X) &&
                (scourceCorners[1].X > targetCorners[2].X) &&
                (scourceCorners[2].X < targetCorners[2].X) &&
                (scourceCorners[3].X > targetCorners[2].X))
            {
                isLeft = true;
            }
            if ((scourceCorners[0].X < targetCorners[1].X) &&
                (scourceCorners[1].X > targetCorners[1].X) &&
                (scourceCorners[2].X < targetCorners[1].X) &&
                (scourceCorners[3].X > targetCorners[1].X) &&
                (scourceCorners[0].X < targetCorners[3].X) &&
                (scourceCorners[1].X > targetCorners[3].X) &&
                (scourceCorners[2].X < targetCorners[3].X) &&
                (scourceCorners[3].X > targetCorners[3].X))
            {
                isRight = true;
            }

            if (isAbove)
            {
                return relDirection.ABOVE;
            }
            if (isBelow)
            {
                return relDirection.BELOW;
            }
            if (isLeft)
            {
                return relDirection.LEFT;
            }
            if (isRight)
            {
                return relDirection.RIGHT;
            }

            return relDirection.err;
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
                velocity.Y = -20;
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
            alterPositionAdition(velocity.ToPoint());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, dimensions, color);
        }



        public void setColor(Color color) {
            this.color = color;
        }
        public Color getColor() {
            return color;
        }
        public void setDimensions(Rectangle dimensions) {
            this.dimensions = dimensions;
        }
        public void setPosition(Point position) {
            this.dimensions.Location = position;
        }
        public Rectangle getDimensions() {
            return dimensions;
        }
        public void alterPositionAdition(Point position) {
            this.dimensions.Location += position;
        }
        public void alterVelocityAdition(Vector2 _velocity) {
            if (Math.Abs((this.velocity + _velocity).X) <= maxVelocity.X)
                this.velocity.X += _velocity.X;

            if (Math.Abs((this.velocity + _velocity).Y) <= maxVelocity.Y)
                this.velocity.Y += _velocity.Y;
        }
        public void alterVelocityMultiplication(Vector2 _velocity) {
            this.velocity *= _velocity;
        }
    }
}
