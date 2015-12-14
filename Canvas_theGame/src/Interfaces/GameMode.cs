using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Canvas_theGame;

namespace Canvas_theGame.src.Interfaces
{
    public interface GameMode
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void PlayerOutOfBounds();
        Player getPlayer();
        void addProjectile(Items.Projectile projectile);
    }
}
