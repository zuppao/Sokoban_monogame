using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Soko_rebuild.GameObjects.Interfaces;

namespace Soko_rebuild.GameObjects
{
    public enum GameObjectType
    {
        Wall,
        Box,
        Slot,
        Player
    }

    internal class Level : IDrawableObject
    {
        readonly Dictionary<GameObjectType, Texture2D> levelTexture2D;
        PlayerBlock player;
        public uint Moves { get; private set; }
        public uint Pushes { get; private set; }
        public bool GameFinished { get; private set; }
        List<BlockBase> blockObjectList;
        List<IDrawableObject> fixedObjects;
        List<IDrawableObject> nonFixedObjects;
        IEnumerable<BlockBase> boxList;
        IEnumerable<BlockBase> slotList;

        public Level(Dictionary<GameObjectType, Texture2D> _levelTexture2D)
        {
            levelTexture2D = _levelTexture2D;
        }
        public void LoadLevel(string[] _levelContent)
        {
            blockObjectList = new List<BlockBase>();
            fixedObjects = new List<IDrawableObject>();
            nonFixedObjects = new List<IDrawableObject>();
            Moves = 0;
            Pushes = 0;
            GameFinished = false;

            for (int y = 0; y < _levelContent.Length; y++)
            {
                var lineBlockArray = _levelContent[y].ToArray();
                for (int x = 0; x < lineBlockArray.Length; x++)
                {
                    var block = NewBlock(lineBlockArray[x], new Point(x, y));
                    if (block == null) continue;

                    blockObjectList.Add(block);
                }
            }

            boxList = blockObjectList.Where(b => b.GetType().Equals(typeof(BoxBlock)));
            slotList = blockObjectList.Where(b => b.GetType().Equals(typeof(SlotBlock)));
        }
        BlockBase NewBlock(char _blockChar, Point _position)
        {
            BlockBase block = null;
            switch (_blockChar)
            {
                case '#':
                    block = new WallBlock(_position, levelTexture2D[GameObjectType.Wall]);
                    fixedObjects.Add(block);
                    break;
                case 'S':
                    block = new SlotBlock(_position, levelTexture2D[GameObjectType.Slot]);
                    fixedObjects.Add(block);
                    break;
                case 'B':
                    block = new BoxBlock(_position, levelTexture2D[GameObjectType.Box]);
                    nonFixedObjects.Add(block);
                    break;
                case 'P':
                    player = new PlayerBlock(_position, levelTexture2D[GameObjectType.Player]);
                    block = player;
                    nonFixedObjects.Add(block);
                    break;
                case '.':
                default:
                    break;
            }

            return block;
        }

        public void MovePlayer(Keys _key)
        {
            var playerNextLocation = GetNextLocation(player.Location, _key);
            var playerNextLocationBlock = blockObjectList.Where(b => b.Location.Equals(playerNextLocation)).FirstOrDefault();

            if (playerNextLocationBlock == null || !playerNextLocationBlock.RigidBody) // empty or slot
            {
                player.Location = playerNextLocation;
                Moves++;
                return;
            }
            else if (playerNextLocationBlock.IsFixed) // wall
                return;
            else // box
            {
                var boxNextLocation = GetNextLocation(playerNextLocation, _key);
                var boxNextLocationBlock = blockObjectList.Where(b => b.Location.Equals(boxNextLocation)).FirstOrDefault();

                if (boxNextLocationBlock == null || !boxNextLocationBlock.RigidBody)
                {
                    // move block
                    playerNextLocationBlock.Location = boxNextLocation;
                    Pushes++;
                    CheckIfGameFinished();

                    // move player
                    player.Location = playerNextLocation;
                    Moves++;
                    return;
                }
                else
                    return;
            }
        }

        Point GetNextLocation(Point _currentLocation, Keys _direction)
        {
            var nextLocation = new Point(_currentLocation.X, _currentLocation.Y);

            if (_direction == Keys.Left)
                nextLocation.X--;
            else if (_direction == Keys.Right)
                nextLocation.X++;
            else if (_direction == Keys.Up)
                nextLocation.Y--;
            else if (_direction == Keys.Down)
                nextLocation.Y++;

            return nextLocation;
        }

        // TODO: 
        //      in the 'controller', if GameFinished true, show to player and load second level
        //      in the 'controller', check if there is other levels or not and show finish or gameover, etc

        void CheckIfGameFinished()
        {
            var boxRightPlaced = (from b in boxList
                                  join s in slotList on b.Location equals s.Location
                                  select b).Count();

            if (boxRightPlaced == slotList.Count())
                GameFinished = true;
        }


        public void Draw(SpriteBatch _spriteBatch)
        {
            foreach (var block in fixedObjects)
            {
                block.Draw(_spriteBatch);
            }

            foreach (var block in nonFixedObjects)
            {
                block.Draw(_spriteBatch);
            }
        }
    }
}
