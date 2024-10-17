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
    internal abstract class BlockBase : IDrawableObject
    {
        public bool RigidBody { get; private set; }
        public bool IsFixed { get; private set; }

        Texture2D texture;
        Vector2 position;
        Point location;
        public Point Location 
        {
            get
            {
                return this.location;
            }
            set 
            {
                this.location = value;
                position = new Vector2(Location.X * (texture?.Width ?? 30),
                                   Location.Y * (texture?.Height ?? 30));
            }
        }
        

        public BlockBase(bool _rigibody, Point _position, Texture2D _texture, bool _isFixed=false)
        {
            RigidBody = _rigibody;
            texture = _texture;
            Location = _position;
            IsFixed = _isFixed;
        }
       

        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.texture,
                              this.position,
                              Color.White);
        }
    }
}
