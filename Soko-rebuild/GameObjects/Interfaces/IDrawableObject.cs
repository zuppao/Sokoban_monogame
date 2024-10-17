using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soko_rebuild.GameObjects.Interfaces
{
    internal interface IDrawableObject
    {
        void Draw(SpriteBatch _spriteBatch);
    }
}
