using System;
using System.Collections.Generic;
using DotTiled;
using Juegazo.Map.Blocks;
using Microsoft.Xna.Framework;

namespace Juegazo.CustomTiledTypes
{
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
        public uint collisionHitbox { get; set; } = 0;
        public int damageAmmount { get; set; } = 0;
    }
    public class JumpWallBlock
    {
        public bool canJump = true;
        public int recoilIntensity = 11;
    }
    public class VerticalBoostBlock
    {
        public int Ammount = 20;
        public bool isCompleteBlock = false;
        public bool toUp = true;
    }
}
namespace Juegazo.CustomTiledTypesImplementation
{

    public abstract class TiledTypesUsed
    {
        public abstract Block createBlock(TileObject obj, int TILESIZE);
        public abstract void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE);
        public abstract List<uint> neededObjects();
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
        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE)
        {
            if (obj is RectangleObject rObject)
            {
                if (obj.ID == initialBlockPosition)
                    InitialBlockPosition = new((int)rObject.X, (int)rObject.Y, (int)rObject.Width, (int)rObject.Height);
                if (obj.ID == EndBlockPosition)
                    endBlockPosition = new((int)rObject.X, (int)rObject.Y, (int)rObject.Width, (int)rObject.Height);
            }
        }
        public override Block createBlock(TileObject obj, int TILESIZE)
        {
            return new Map.Blocks.MovementBlock(
                new((int)(obj.X / obj.Width * TILESIZE),
                    (int)(((obj.Y / obj.Height) - 1) * TILESIZE),
                    TILESIZE,
                    TILESIZE),
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
    //This has been done with AI, probably something will be chahged
    public class DamageBlock : TiledTypesUsed
    {
        public override string ToString()
        {
            return $"CanDamage: {canDamage}, CollisionHitbox: {collisionHitbox}, DamageAmount: {damageAmmount}, Hitbox: {hitbox}";
        }
        public bool canDamage { get; } = false;
        public uint collisionHitbox { get; } = 0;
        public int damageAmmount { get; } = 0;
        public Rectangle hitbox { get; set; } = new();

        public DamageBlock(CustomTiledTypes.DamageBlock dblock)
        {
            canDamage = dblock.canDamage;
            collisionHitbox = dblock.collisionHitbox;
            damageAmmount = dblock.damageAmmount;
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE)
        {
            if (obj.ID == collisionHitbox)
            {
                hitbox = new((int)(obj.X / obj.Width * TILESIZE),
                (int)(((obj.Y / obj.Height) - 1) * TILESIZE),
                TILESIZE,
                TILESIZE);
            }
        }

        public override Block createBlock(TileObject obj, int TILESIZE)
        {
            return new Map.Blocks.KillBlock(
                hitbox, damageAmmount
            );
        }

        public override List<uint> neededObjects()
        {
            return new([collisionHitbox]);
        }
    }

    public class JumpWallBlock : TiledTypesUsed
    {
        public override string ToString()
        {
            return $"CanJump: {canJump}, RecoilIntensity: {recoilIntensity}";
        }
        public bool canJump { get; } = true;
        public int recoilIntensity { get; } = 11;

        public JumpWallBlock(CustomTiledTypes.JumpWallBlock jblock)
        {
            canJump = jblock.canJump;
            recoilIntensity = jblock.recoilIntensity;
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE)
        {
            // No additional properties needed from objects
        }

        public override Block createBlock(TileObject obj, int TILESIZE)
        {
            return new JumpWall(
                new((int)(obj.X / obj.Width * TILESIZE),
                    (int)(((obj.Y / obj.Height) - 1) * TILESIZE),
                    TILESIZE,
                    TILESIZE),
                15, //fuck i forgot to add that
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

        public override void getNeededObjectPropeties(DotTiled.Object obj, int TILESIZE)
        {
            // No additional properties needed from objects
        }

        public override Block createBlock(TileObject obj, int TILESIZE)
        {
            return new Map.Blocks.VerticalBoostBlock(
                new((int)(obj.X / obj.Width * TILESIZE),
                    (int)(((obj.Y / obj.Height) - 1) * TILESIZE),
                    TILESIZE,
                    TILESIZE),
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
}