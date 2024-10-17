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
    internal class BoxBlock : BlockBase
    {
        public BoxBlock(Point _position, Texture2D _wallTexture) : base(true, _position, _wallTexture)
        {
        }

        //public override void Draw(SpriteBatch _spriteBatch)
        //{
        //    _spriteBatch.Draw(this.texture,
        //                      this.position,
        //                      Color.White);
        //}
    }
}
