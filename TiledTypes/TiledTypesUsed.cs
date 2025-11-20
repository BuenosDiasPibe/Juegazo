using System;
using System.Collections.Generic;
using DotTiled;
using Juegazo.CustomTiledTypes;
using Juegazo.Map;
using Microsoft.Xna.Framework;

namespace Juegazo.CustomTiledTypes
{
    public class PlayerSpawner
    {
        public bool isPlayer { get; set; } = false;
        public bool isPlayable { get; set; } = false;
        public bool isVisible { get; set; } = false;
    }
    public class LevelPropieties
    {
        public bool ClampCameraToBoundries { get; set; } = true;
        public float zoom { get; set; } = 0f;
        public bool loadAudio { get; set; } = false;

    }
    public class NPC
    {
        public int dialogEnd { get; set; } = 0;
        public int dialogStart { get; set; } = 1;
        public string name { get; set; } = "";
    }
    public class CollisionBlock
    {
        public bool canCollide { get; set; } = true;
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
        public uint Door { get; set; } = 0;
        public bool isColected { get; set; } = false;
    }
    public class DoorBlock
    {
        public bool isOpen { get; set; } = false;

    }
    public class DoubleJump
    {
        public int numberOfJumps { get; set; } = 5;
    }
    public class SpeedUpBlock
    {
        public int speedAmount { get; set; } = 5;
    }
    public class MoveOneDirection
    {
        public bool canMove { get; set; } = false;
        public uint initialPosition { get; set; } = 0;
        public uint lastPosition { get; set; } = 0;
        public int velocity { get; set; } = 0;
    }
    public class OneWayBlock
    {
        public FACES face { get; set; } = FACES.TOP;
    }
    public enum FACES //uhhhh i... cant explain it
    {
        TOP, BOTTOM, LEFT, RIGHT
    }
    public class SlowDownBlock
    {
        public int slowAmount { get; set; } = 5;
    }
    public class WaterBlock
    {
        public bool canLoadComponent { get; set; } = true;
    }
    public class JumpingOrb
    {
        public int JumpAmmount { get; set; } = 0;
        public bool isUsable { get; set; } = false;
    }
    public class GravityChangerOrbBlock
    {
        public bool changeHorizontal {get; set; } = false;
        public bool changeVertical {get; set; } = false;
    }
    public class GravityChangerPadBlock
    {
        public FACES snapTo { get; set; } = FACES.TOP;
    }
    public class Portal
    {
        public uint portalLink { get; set; } = 0;
        public float delayTimeSeconds { get; set; } = 0f;
        public bool canTeleport { get; set; } = false;
    }
    public class MovingDamageBlock //ERROR: cant use MovementBlock or DamageBlock instances here, throws error saying "Object of type 'System.Int32' cannot be converted to type 'System.UInt32'", probably not recognizing type or something 
    {
        public bool canDamage { get; set; } = false;
        public bool canMove { get; set; } = false;
        public int damageAmmount { get; set; } = 0;
        public uint endBlockPosition { get; set; } = 0;
        public uint initialBlockPosition { get; set; } = 0;
        public float velocity { get; set; } = 0;
    }
    public class GravityChangerMode
    {
        public bool changeHorizontal { get; set; } = false;
        public bool changeVertical { get; set; } = false;
    }
}
namespace Juegazo.CustomTiledTypesImplementation
{

    public abstract class TiledTypesUsed
    {
        public abstract Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null);
        public abstract void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map);
        public abstract List<uint> neededObjects();
        protected Rectangle GetRect(int TILESIZE, DotTiled.Map map, DotTiled.Object obj = null)
        {
            if (obj == null) return new();
            int tileOffset = -(int)(obj.Height / map.TileHeight); //Tiled draws objects from bottom to top
            return new Rectangle(
                            (int)(obj.X / map.TileWidth * TILESIZE),
                            (int)(((obj.Y / map.TileHeight) + tileOffset) * TILESIZE),
                            (int)(obj.Width / map.TileWidth * TILESIZE),
                            (int)(obj.Height / map.TileHeight * TILESIZE)
                        );
        }
    }
    public class MovementBlock : TiledTypesUsed
    {
        public MovementBlock() { }
        public override string ToString()
        {
            return $"MovementBlock: \tEndBlockPosition: {mblock.EndBlockPosition}\n\tCanMove: {mblock.canMove}\n\tInitialBlockPosition: {mblock.initialBlockPosition}\n\tSpeed: {mblock.velocity}\n\tInitialBlock: {InitialBlockPosition}\n\tEndBlock: {endBlockPosition}";
        }
        public CustomTiledTypes.MovementBlock mblock;
        public Rectangle InitialBlockPosition { get; set; } = new();
        public Rectangle endBlockPosition { get; set; } = new();
        public MovementBlock(CustomTiledTypes.MovementBlock mblock)
        {
            this.mblock = mblock;
        }
        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            if (obj.ID == mblock.initialBlockPosition)
            {
                var newObj = new RectangleObject
                {
                    ID = obj.ID,
                    X = obj.X,
                    Y = obj.Y,
                    Width = obj.Width,
                    Height = obj.Height
                };
                newObj.Y += obj.Height;
                InitialBlockPosition = GetRect(TILESIZE, map, newObj);
            }
            if (obj.ID == mblock.EndBlockPosition)
            {
                var newObj = new RectangleObject
                {
                    ID = obj.ID,
                    X = obj.X,
                    Y = obj.Y,
                    Width = obj.Width,
                    Height = obj.Height
                };
                newObj.Y += obj.Height;
                endBlockPosition = GetRect(TILESIZE, map, newObj);
            }
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.MovementBlock( //TODO: change it to get "this" instead of InitialBlockPos, endBlockPos, etc.
                GetRect(TILESIZE, map, obj),
                InitialBlockPosition,
                endBlockPosition,
                mblock
            );
        }

        public override List<uint> neededObjects()
        {
            return new([mblock.initialBlockPosition, mblock.EndBlockPosition]);
        }

    }
    public class DamageBlock : TiledTypesUsed
    {
        public DamageBlock() { }
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

        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.DamageBlock(
               GetRect(TILESIZE, map, obj), damageAmmount, canDamage
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
        public JumpWallBlock() { }
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

        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.JumpWallBlock(
                GetRect(TILESIZE, map, obj),
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
        public VerticalBoostBlock() { }
        public override string ToString()
        {
            return $"Amount: {vblock.Ammount}, IsCompleteBlock: {vblock.isCompleteBlock}, ToUp: {vblock.toUp}";
        }
        public CustomTiledTypes.VerticalBoostBlock vblock; //TODO: passing references is expensive, maybe change that when game finishes

        public VerticalBoostBlock(CustomTiledTypes.VerticalBoostBlock vblock)
        {
            this.vblock = vblock;
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            // No additional properties needed from objects
        }

        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.VerticalBoostBlock(
                GetRect(TILESIZE, map, obj),
                vblock
            );
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class CompleteLevelBlock : TiledTypesUsed
    {
        public CompleteLevelBlock() { }
        public CustomTiledTypes.CompleteLevelBlock coso;
        public CompleteLevelBlock(CustomTiledTypes.CompleteLevelBlock coso)
        {
            this.coso = coso;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.CompleteLevelBlock(
                GetRect(TILESIZE, map, obj),
                coso
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
        public CheckPointBlock() { }
        public Vector2 Position = new();
        public CustomTiledTypes.CheckPointBlock cpb;
        public CheckPointBlock(CustomTiledTypes.CheckPointBlock cpb)
        {
            this.cpb = cpb;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.CheckPointBlock(
                GetRect(TILESIZE, map, obj),
                cpb,
                Position
            );
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            if (obj.ID == cpb.position)
                Position = new(obj.X / map.TileWidth * TILESIZE, obj.Y / map.TileHeight * TILESIZE);

        }

        public override List<uint> neededObjects()
        {
            return new([cpb.position]);
        }
    }
    public class Key : TiledTypesUsed
    {
        public Key() { }
        public bool isCollected { get; } = true;
        public uint DoorID { get; } = 0;

        public Key(CustomTiledTypes.Key key)
        {
            isCollected = key.isColected;
            DoorID = key.Door;
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }

        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            if(obj is TileObject)
            {
                return new Map.Blocks.Key(DoorID, GetRect(TILESIZE, map, obj), isCollected);
            }
            throw new Exception("you cant create keys in non-ObjectTiles");
        }


        public override List<uint> neededObjects()
        {
            return new();
        }

        public override string ToString()
        { return $"Key: DoorID: {DoorID} IsCollected: {isCollected}"; }
    }
    public class DoorBlock : TiledTypesUsed
    {
        public DoorBlock() { }
        public bool isOpen = false;
        public DoorBlock(CustomTiledTypes.DoorBlock block)
        {
            isOpen = block.isOpen;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            if (obj == null) throw new Exception("DoorBlock created in non-ObjectLayer, move it to an ObjectLayer");
            return new Map.Blocks.DoorBlock(
                GetRect(TILESIZE, map, obj),
                isOpen,
                obj.ID
                );
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class CollisionBlock : TiledTypesUsed
    {
        public CollisionBlock() { }
        public bool canCollide = true;
        public CollisionBlock(CustomTiledTypes.CollisionBlock cBlock)
        {
            canCollide = cBlock.canCollide;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            if (!canCollide) return null; // this causes some issues with a lot of things, its best to create a Collision block instead for now
            return new Map.Blocks.CollisionBlock(GetRect(TILESIZE, map, obj));
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map) { }

        public override List<uint> neededObjects()
        { return new(); }
    }
    public class SpeedUpBlock : TiledTypesUsed
    {
        public SpeedUpBlock() { }
        public CustomTiledTypes.SpeedUpBlock sppeedUp;
        public SpeedUpBlock(CustomTiledTypes.SpeedUpBlock sppeedUp)
        {
            this.sppeedUp = sppeedUp;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.SpeedUpBlock(GetRect(TILESIZE, map, obj),sppeedUp);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }

        public override List<uint> neededObjects()
        { return new(); }
    }
    public class MoveOneDirection : TiledTypesUsed
    {
        public MoveOneDirection() { }
        public override string ToString()
        {
            return $"MoveOneDirectionBlock: \tEndBlockPosition: {mblock.lastPosition}\n\tCanMove: {mblock.canMove}\n\tInitialBlockPosition: {mblock.initialPosition}\n\tSpeed: {mblock.velocity}\n\tInitialBlock: {initialPos}\n\tEndBlock: {endPos}";
        }
        public CustomTiledTypes.MoveOneDirection mblock;
        public Rectangle initialPos { get; set; } = new();
        public Rectangle endPos { get; set; } = new();
        public MoveOneDirection(CustomTiledTypes.MoveOneDirection mblock)
        {
            this.mblock = mblock;
        }
        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            if (obj is RectangleObject rObject)
            {
                if (obj.ID == mblock.initialPosition)
                {
                    var newObj = new RectangleObject
                    {
                        ID = obj.ID,
                        X = obj.X,
                        Y = obj.Y,
                        Width = obj.Width,
                        Height = obj.Height
                    };
                    newObj.Y += obj.Height;
                    initialPos = GetRect(TILESIZE, map, newObj);
                }
                if (obj.ID == mblock.lastPosition)
                {
                    var newObj = new RectangleObject
                    {
                        ID = obj.ID,
                        X = obj.X,
                        Y = obj.Y,
                        Width = obj.Width,
                        Height = obj.Height
                    };
                    newObj.Y += obj.Height;
                    endPos = GetRect(TILESIZE, map, newObj);
                }
            }
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.MoveOneDirection( //TODO: change it to get "this" instead of InitialBlockPos, endBlockPos, etc.
                GetRect(TILESIZE, map, obj),
                initialPos,
                endPos,
                mblock
                );
        }

        public override List<uint> neededObjects()
        {
            return new([mblock.initialPosition, mblock.lastPosition]);
        }
    }
    public class OneWayBlock : TiledTypesUsed
    {
        public CustomTiledTypes.FACES face { get; set; } = CustomTiledTypes.FACES.TOP;
        public OneWayBlock() { }
        public OneWayBlock(CustomTiledTypes.OneWayBlock o)
        {
            face = o.face;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.OneWayBlock(GetRect(TILESIZE, map, obj), face);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class SlowDownBlock : TiledTypesUsed
    {
        public int slowDownAmmount = 5;
        public SlowDownBlock() { }
        public SlowDownBlock(CustomTiledTypes.SlowDownBlock o)
        {
            slowDownAmmount = o.slowAmount;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.SlowDownBlock(GetRect(TILESIZE, map, obj), slowDownAmmount);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class WaterBlock : TiledTypesUsed
    {
        public bool canLoadComponent = true;
        public WaterBlock() { }
        public WaterBlock(CustomTiledTypes.WaterBlock a)
        {
            canLoadComponent = a.canLoadComponent;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj)
        {
            return new Map.Blocks.WaterBlock(GetRect(TILESIZE, map, obj), canLoadComponent);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class JumpingOrb : TiledTypesUsed
    {
        public int jumpAmmount = 0;
        public CustomTiledTypes.JumpingOrb orb = new();

        public JumpingOrb() { }
        public JumpingOrb(CustomTiledTypes.JumpingOrb orb)
        {
            this.orb = orb;
            this.jumpAmmount = orb.JumpAmmount;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.JumpingOrb(GetRect(TILESIZE, map, obj), this);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class GravityChangerOrbBlock : TiledTypesUsed
    {
        public CustomTiledTypes.GravityChangerOrbBlock c;
        public GravityChangerOrbBlock() { }
        public GravityChangerOrbBlock(CustomTiledTypes.GravityChangerOrbBlock c)
        {
            this.c = c;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.GravityChangerOrbBlock(
                GetRect(TILESIZE, map, obj),
                c.changeHorizontal,
                c.changeVertical
            );
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class GravityChangerPadBlock : TiledTypesUsed
    {
        public FACES faces;
        public GravityChangerPadBlock() { }
        public GravityChangerPadBlock(CustomTiledTypes.GravityChangerPadBlock c)
        {
            faces = c.snapTo switch
            {
                CustomTiledTypes.FACES.TOP => FACES.TOP,
                CustomTiledTypes.FACES.BOTTOM => FACES.BOTTOM,
                CustomTiledTypes.FACES.LEFT => FACES.LEFT,
                CustomTiledTypes.FACES.RIGHT => FACES.RIGHT,
                _ => throw new NotImplementedException(),
            };
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            return new Map.Blocks.GravityChangerPadBlock(GetRect(TILESIZE, map, obj), faces);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class Portal : TiledTypesUsed
    {
        CustomTiledTypes.Portal portal = new();
        public Portal() { }
        public Portal(CustomTiledTypes.Portal portal)
        {
            this.portal = portal;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            if (obj == null) throw new Exception("Cant create Portal Block, add it to a ObjectLayer");
            return new Map.Blocks.Portal(
                GetRect(TILESIZE, map, obj),
                portal.portalLink,
                obj.ID,
                portal.delayTimeSeconds,
                portal.canTeleport
            );
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        { }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
    public class MovingDamageBlock : TiledTypesUsed
    {
        public CustomTiledTypes.MovingDamageBlock data = new();
        public Rectangle InitialBlockPosition = new();
        public Rectangle endBlockPosition = new();
        public MovingDamageBlock() { }
        public MovingDamageBlock(CustomTiledTypes.MovingDamageBlock data)
        {
            this.data = data;
        }
        public override Block createBlock(int TILESIZE, DotTiled.Map map, TileObject obj = null)
        {
            if (obj == null) throw new Exception("MovingDamageBlock created in non-ObjectLayer, delete it");
            return new Map.Blocks.MovingDamageBlock(GetRect(TILESIZE, map, obj), this);
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE, DotTiled.Map map)
        {
            if (obj.ID == data.initialBlockPosition)
            {
                var newObj = new RectangleObject
                {
                    ID = obj.ID,
                    X = obj.X,
                    Y = obj.Y,
                    Width = obj.Width,
                    Height = obj.Height
                };
                newObj.Y += newObj.Height;
                InitialBlockPosition = GetRect(TILESIZE, map, newObj);
            }
            if (obj.ID == data.endBlockPosition)
            {
                var newObj = new RectangleObject
                {
                    ID = obj.ID,
                    X = obj.X,
                    Y = obj.Y,
                    Width = obj.Width,
                    Height = obj.Height
                };
                newObj.Y += newObj.Height;
                endBlockPosition = GetRect(TILESIZE, map, newObj);
            }
        }

        public override List<uint> neededObjects()
        {
            return new([data.initialBlockPosition,
                        data.endBlockPosition]);
        }


    }
}
