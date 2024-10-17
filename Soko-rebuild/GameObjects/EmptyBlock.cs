using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Soko_rebuild.GameObjects.Interfaces;

namespace Soko_rebuild.GameObjects
{
    internal class EmptyBlock : BlockBase
    {
        public EmptyBlock(Point _position, Texture2D _wallTexture) : base(false, _position, _wallTexture, _isFixed: true)
        {
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            //_spriteBatch.Draw(this.texture,
            //                  this.position,
            //                  Color.White);
        }
    }
}
