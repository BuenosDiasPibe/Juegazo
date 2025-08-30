using System;
using System.Collections.Generic;
using DotTiled;
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
        public int movementSpeed { get; set; } = 2;
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
        public int Ammount = 5;
        public bool isCompleteBlock = false;
        public bool toUp = true;
    }
}
namespace Juegazo.CustomTiledTypesImplementation
{

    public abstract class TiledTypesUsed
    {
        public abstract Block changeBlock(Block block);
        public abstract void getNeededObjectPropeties(DotTiled.Object obj);
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
        public int speed { get; } = 2;
        public Rectangle InitialBlockPosition { get; set; } = new();
        public Rectangle endBlockPosition { get; set; } = new();
        public MovementBlock(CustomTiledTypes.MovementBlock mblock)
        {
            EndBlockPosition = mblock.EndBlockPosition;
            canMove = mblock.canMove;
            initialBlockPosition = mblock.initialBlockPosition;
            speed = mblock.movementSpeed;
        }
        public override void getNeededObjectPropeties(DotTiled.Object obj)
        {
            if (obj is RectangleObject rObject)
            {
                if (obj.ID == initialBlockPosition)
                    InitialBlockPosition = new((int)rObject.X, (int)rObject.Y, (int)rObject.Width, (int)rObject.Height);
                if (obj.ID == EndBlockPosition)
                    endBlockPosition = new((int)rObject.X, (int)rObject.Y, (int)rObject.Width, (int)rObject.Height);
            }
        }
        public override Block changeBlock(Block block)
        {
            if (block is Map.Blocks.MovementBlock mbBlock)
            {
                Map.Blocks.MovementBlock returnBlock = new(mbBlock.collider, InitialBlockPosition, endBlockPosition);
                return returnBlock;
            }
            Console.WriteLine("you fucked up");
            return block;
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

        public override void getNeededObjectPropeties(DotTiled.Object obj)
        {
            if (obj is RectangleObject rObject && obj.ID == collisionHitbox)
            {
                Console.WriteLine("we gucci");
                hitbox = new((int)rObject.X, (int)rObject.Y, (int)rObject.Width, (int)rObject.Height);
            }
        }

        public override Block changeBlock(Block block)
        {
            if (block is Map.Blocks.KillBlock)
            {
                Map.Blocks.KillBlock returnBlock = new(hitbox, damageAmmount);
                return returnBlock;
            }
            Console.WriteLine("you fucked up");
            return block;
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

        public override void getNeededObjectPropeties(DotTiled.Object obj)
        {
            // No additional properties needed from objects
        }

        public override Block changeBlock(Block block)
        {
            if (block is Map.Blocks.JumpWall jwBlock)
            {
                int jumpAmmount = canJump ? 11 : 0;
                Map.Blocks.JumpWall returnBlock = new(jwBlock.collider, jumpAmmount, canJump, recoilIntensity);
                return returnBlock;
            }
            Console.WriteLine("you fucked up");
            return block;
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
        public int Ammount { get; } = 5;
        public bool isCompleteBlock { get; } = false;
        public bool toUp { get; } = true;

        public VerticalBoostBlock(CustomTiledTypes.VerticalBoostBlock vblock)
        {
            Ammount = vblock.Ammount;
            isCompleteBlock = vblock.isCompleteBlock;
            toUp = vblock.toUp;
        }

        public override void getNeededObjectPropeties(DotTiled.Object obj)
        {
            // No additional properties needed from objects
        }

        public override Block changeBlock(Block block)
        {
            if (block is Map.Blocks.VerticalBoostBlock vbBlock)
            {
                Map.Blocks.VerticalBoostBlock returnBlock = new(vbBlock.collider, Ammount, isCompleteBlock, toUp);
                return returnBlock;
            }
            Console.WriteLine("you fucked up");
            return block;
        }

        public override List<uint> neededObjects()
        {
            return new();
        }
    }
}