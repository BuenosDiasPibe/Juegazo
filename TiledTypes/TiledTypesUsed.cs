using System;
using System.Collections.Generic;
using DotTiled;
using Juegazo.CustomTiledTypes;
using Juegazo.Map.Blocks;
using Microsoft.Xna.Framework;

namespace Juegazo.CustomTiledTypes
{
    public class NPC
    {
        public int dialogEnd { get; set; } = 0;
        public int dialogStart { get; set; } = 1;
        public string name { get; set; } = "";
    }
    public class CollisionBlock
    {
        public bool canCollide {get; set;} = true;
    }
    public class CollisionBlockObjectLayer
    {
        public bool canOverrideCollisionLayer { get; set; } = true;
    }
    public class MovementBlock
    {
        public uint EndBlockPosition { get; set; } = 0;
        public bool canMove { get; set; } = false;
        public uint initialBlockPosition { get; set; } = 0;
        public float velocity { get; set; } = 1;
    }
    public class DamageBlock
    {
        public bool canDamage { get; set; } = false;
        public int damageAmount { get; set; } = 1;
    }
    public class JumpWallBlock
    {
        public bool canJump { get; set; } = true;
        public int jumpIntensity { get; set; } = 10;
        public int recoilIntensity { get; set; } = 11;
    }
    public class VerticalBoostBlock
    {
        public int Ammount { get; set; } = 20;
        public bool isCompleteBlock { get; set; } = false;
        public bool toUp { get; set; } = true;
    }
    public class CompleteLevelBlock
    {
        public bool isEnabled { get; set; } = true;
        public int nextLevel { get; set; } = 0;
    }
    public class CheckPointBlock
    {
        public bool isEnabled { get; set; } = true;
        public int message { get; set; } = 0;
        public uint position { get; set; } = 0;
    }
    public class Key
    {
        public DotTiled.Color color {get; set;}
        public bool isColected { get; set; } = true;
    }
    public class DoorBlock
    {
        public DotTiled.Color color { get; set; }
        public bool isOpen { get; set; } = false;
        public uint key { get; set; } = 0;

    }
    public class DoubleJump
    {
        public int numberOfJumps { get; set; } = 5;
    }
    public class SpeedUp
    {
        public int SpeedAmmount { get; set; } = 5;
    }
    public class MoveOneDirection
    {
        public bool canMove { get; set; } = false;
        public uint initialPosition { get; set; } = 0;
        public uint lastPosition { get; set; } = 0;
        public int velocity { get; set; } = 0;
    }
}
namespace Juegazo.CustomTiledTypesImplementation
{

    public abstract class TiledTypesUsed
    {
        public abstract Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map); 
        public abstract void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map);
        public abstract List<uint> neededObjects();
        protected Rectangle GetRect(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            int what = -(int)(obj.Height / map.TileHeight); //what the actual fuck? why do i need to do this????
            return new Rectangle(
                            (int)(obj.X / map.TileWidth * TILESIZE),
                            (int)(((obj.Y / map.TileHeight) + what) * TILESIZE),
                            (int)(obj.Width / map.TileWidth * TILESIZE),
                            (int)(obj.Height / map.TileHeight * TILESIZE)
                        );
        }
    }
    public class MovementBlock : TiledTypesUsed
    {
        public override string ToString()
        {
            return $"MovementBlock: \tEndBlockPosition: {EndBlockPosition}\n\tCanMove: {canMove}\n\tInitialBlockPosition: {initialBlockPosition}\n\tSpeed: {speed}\n\tInitialBlock: {InitialBlockPosition}\n\tEndBlock: {endBlockPosition}";
        }
        public uint EndBlockPosition { get; } = 0;
        public bool canMove { get; } = false;
        public uint initialBlockPosition { get; } = 0;
        public float speed { get; } = 1;
        public Rectangle InitialBlockPosition { get; set; } = new();
        public Rectangle endBlockPosition { get; set; } = new();
        public MovementBlock(CustomTiledTypes.MovementBlock mblock)
        {
            EndBlockPosition = mblock.EndBlockPosition;
            canMove = mblock.canMove;
            initialBlockPosition = mblock.initialBlockPosition;
            speed = mblock.velocity;
        }
        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            if (obj is RectangleObject rObject)
            {
                if (obj.ID == initialBlockPosition)
                    InitialBlockPosition = GetRect(obj, TILESIZE, map);
                if (obj.ID == EndBlockPosition)
                {
                    rObject.Y += rObject.Height;
                    endBlockPosition = GetRect(obj, TILESIZE, map);
                }
            }
        }
        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new Map.Blocks.MovementBlock(
                GetRect(obj, TILESIZE, map),
                InitialBlockPosition,
                endBlockPosition,
                speed,
                canMove
            );
        }

        public override List<uint> neededObjects()
        {
            return new([initialBlockPosition, EndBlockPosition]);
        }
    }
    public class DamageBlock : TiledTypesUsed
    {
        public override string ToString()
        {
            return $"CanDamage: {canDamage}, DamageAmount: {damageAmmount}";
        }
        public bool canDamage { get; } = false;
        public int damageAmmount { get; } = 0;

        public DamageBlock(CustomTiledTypes.DamageBlock dblock)
        {
            canDamage = dblock.canDamage;
            damageAmmount = dblock.damageAmount;
        }

        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new Map.Blocks.KillBlock(
                GetRect(obj, TILESIZE, map), damageAmmount, canDamage
            );
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }
    }

    public class JumpWallBlock : TiledTypesUsed
    {
        public override string ToString()
        {
            return $"CanJump: {canJump}, RecoilIntensity: {recoilIntensity}";
        }
        public bool canJump { get; } = true;
        public int recoilIntensity { get; } = 11;
        public int jumpIntensity { get; } = 10;

        public JumpWallBlock(CustomTiledTypes.JumpWallBlock jblock)
        {
            canJump = jblock.canJump;
            recoilIntensity = jblock.recoilIntensity;
            jumpIntensity = jblock.jumpIntensity;
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            // No additional properties needed from objects
        }

        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new JumpWall(
                GetRect(obj, TILESIZE, map),
                jumpIntensity,
                canJump,
                recoilIntensity
            );
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }

    public class VerticalBoostBlock : TiledTypesUsed
    {
        public override string ToString()
        {
            return $"Amount: {Ammount}, IsCompleteBlock: {isCompleteBlock}, ToUp: {toUp}";
        }
        public int Ammount { get; } = 20;
        public bool isCompleteBlock { get; } = false;
        public bool toUp { get; } = true;

        public VerticalBoostBlock(CustomTiledTypes.VerticalBoostBlock vblock)
        {
            Ammount = vblock.Ammount;
            isCompleteBlock = vblock.isCompleteBlock;
            toUp = vblock.toUp;
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            // No additional properties needed from objects
        }

        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new Map.Blocks.VerticalBoostBlock(
                GetRect(obj, TILESIZE, map),
                Ammount,
                isCompleteBlock,
                toUp
            );
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class CompleteLevelBlock : TiledTypesUsed
    {
        public bool isEnabled { get; set; } = false;
        public int nextLevel = 0;
        public CompleteLevelBlock(CustomTiledTypes.CompleteLevelBlock coso)
        {
            isEnabled = coso.isEnabled;
            nextLevel = coso.nextLevel;
        }
        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new CompleteBlock(
                GetRect(obj, TILESIZE, map),
                isEnabled,
                nextLevel
            );
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class CheckPointBlock : TiledTypesUsed
    {
        public bool isEnabled { get; set; } = true;
        public int message { get; set; } = 0;
        public uint position = 0;
        public Vector2 Position = new();
        public CheckPointBlock(CustomTiledTypes.CheckPointBlock cpb)
        {
            isEnabled = cpb.isEnabled;
            message = cpb.message;
            position = cpb.position;
        }
        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new Map.Blocks.CheckPointBlock(
                GetRect(obj, TILESIZE, map),
                isEnabled,
                message,
                Position
            );
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            if (obj.ID == position)
                Position = new(obj.X / map.TileWidth * TILESIZE, obj.Y / map.TileHeight * TILESIZE);

        }

        public override List<uint> neededObjects()
        {
            return new([position]);
        }
    }
    public class Key : TiledTypesUsed
    {
        public bool isCollected { get; } = true;

        public Key(CustomTiledTypes.Key key)
        {
            isCollected = key.isColected;
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            // No additional properties needed from objects
        }

        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            var rect = GetRect(obj, TILESIZE, map);
            return new Map.Blocks.Key(obj.ID, rect);
        }


        public override List<uint> neededObjects()
        {
            return new();
        }

        public override string ToString()
        { return $"Key: IsCollected: {isCollected}"; }
    }
    public class DoorBlock : TiledTypesUsed
    {
        public bool isOpen = false;
        public uint key = 0;
        public DoorBlock(CustomTiledTypes.DoorBlock block)
        {
            isOpen = block.isOpen;
            key = block.key;
        }
        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new Map.Blocks.DoorBlock(GetRect(obj, TILESIZE, map), key, isOpen);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class CollisionBlock : TiledTypesUsed
    {
        public bool canCollide = true;
        public CollisionBlock(CustomTiledTypes.CollisionBlock cBlock)
        {
            canCollide = cBlock.canCollide;
        }
        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            if (!canCollide) return null; // this causes some issues with a lot of things, its best to create a Collision block instead for now
            return new Map.Blocks.CollisionBlock(GetRect(obj, TILESIZE, map), canCollide);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class SpeedUp : TiledTypesUsed
    {
        public int speedAmmount = 5;
        public SpeedUp(CustomTiledTypes.SpeedUp sppeedUp)
        {
            speedAmmount = sppeedUp.SpeedAmmount;
        }
        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new SpeedUpBlock(GetRect(obj, TILESIZE, map), speedAmmount);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }

        public override List<uint> neededObjects()
        { return new(); }
    }
    public class MoveOneDirection : TiledTypesUsed
    {
        public override string ToString()
        {
            return $"MoveOneDirectionBlock: \tEndBlockPosition: {EndBlockPosition}\n\tCanMove: {canMove}\n\tInitialBlockPosition: {initialBlockPosition}\n\tSpeed: {speed}\n\tInitialBlock: {initialPos}\n\tEndBlock: {endPos}";
        }
        public uint EndBlockPosition { get; } = 0;
        public bool canMove { get; } = false;
        public uint initialBlockPosition { get; } = 0;
        public int speed { get; } = 1;
        public Rectangle initialPos { get; set; } = new();
        public Rectangle endPos { get; set; } = new();
        public MoveOneDirection(CustomTiledTypes.MoveOneDirection mblock)
        {
            EndBlockPosition = mblock.lastPosition;
            canMove = mblock.canMove;
            initialBlockPosition = mblock.initialPosition;
            speed = mblock.velocity;
        }
        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            if (obj is RectangleObject rObject)
            {
                if (obj.ID == initialBlockPosition)
                    initialPos = GetRect(obj, TILESIZE, map);
                if (obj.ID == EndBlockPosition)
                    endPos = GetRect(obj, TILESIZE, map);
            }
        }
        public override Block createBlock(TileObject obj, int TILESIZE, DotTiled.Map map)
        {
            return new Map.Blocks.MoveOneDirection(
                GetRect(obj, TILESIZE, map),
                initialPos,
                endPos,
                speed,
                canMove
            );
        }

        public override List<uint> neededObjects()
        {
            return new([initialBlockPosition, EndBlockPosition]);
        }
    }
}