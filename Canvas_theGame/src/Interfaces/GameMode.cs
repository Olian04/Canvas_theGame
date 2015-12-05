using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Canvas_theGame.src.Interfaces
{
    interface GameMode
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void PlayerOutOfBounds();
        Player getPlayer();
    }
}
