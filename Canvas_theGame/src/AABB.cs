using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;


namespace Canvas_theGame.src
{
    /**
     * AABB = Axis Alined Bounding Box
     * (Rectangle hitbox)
     */
    class AABB
    {
        private Rectangle boundingBox;
        private Vector2 position;
        private Point size;


        public AABB(Point _position, Point _size)
        {
            position = _position.ToVector2();
            size = _size;
            changeBoundingBox();
        }
        public AABB(Rectangle rectangle) {
            position = new Vector2(rectangle.X, rectangle.Y);
            size = new Point(rectangle.Width, rectangle.Height);
            changeBoundingBox();
        }

        private void changeBoundingBox()
        {
            boundingBox = new Rectangle(new Point((int)position.X, (int)position.Y), size);
        }


        public void setPosition(Vector2 newValue)
        {
            position = newValue;
            changeBoundingBox();
        }
        public void setPositionX(float x) {
            position.X = x;
            changeBoundingBox();
        }
        public void setPositionY(float y) {
            position.Y = y;
            changeBoundingBox();
        }
        public void alterPositionAdition(Vector2 adition)
        {
            position += adition;
            changeBoundingBox();
        }
        public Rectangle getBoundingBox()
        {
            changeBoundingBox();
            return boundingBox;
        }


        public bool Intersects(AABB targetAABB)
        {
            changeBoundingBox();
            return this.getBoundingBox().Intersects(targetAABB.getBoundingBox());
        }
        public bool isLeft(AABB targetAABB)
        {
            if (this.Intersects(targetAABB)) {
                Rectangle rect = Rectangle.Intersect(this.getBoundingBox(), targetAABB.getBoundingBox());
                if (targetAABB.getBoundingBox().Left == rect.Left) {
                    if (rect.Height > rect.Width) {
                        return true;
                    }
                } //Is on left
            } //Intersects
            return false;
        }
        public bool isRight(AABB targetAABB)
        {
            if (this.Intersects(targetAABB)){
                Rectangle rect = Rectangle.Intersect(this.getBoundingBox(), targetAABB.getBoundingBox());
                if (targetAABB.getBoundingBox().Right == rect.Right) {
                    if (rect.Height > rect.Width) {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool isAbove(AABB targetAABB)
        {
            if (this.Intersects(targetAABB)) {
                Rectangle rect = Rectangle.Intersect(this.getBoundingBox(), targetAABB.getBoundingBox());
                if (targetAABB.getBoundingBox().Top == rect.Top) {
                    if (rect.Height < rect.Width) {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool isBelow(AABB targetAABB)
        {
            if (this.Intersects(targetAABB)) {
                Rectangle rect = Rectangle.Intersect(this.getBoundingBox(), targetAABB.getBoundingBox());
                if (targetAABB.getBoundingBox().Bottom == rect.Bottom) {
                    if (rect.Height < rect.Width) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
