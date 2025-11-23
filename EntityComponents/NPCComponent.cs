using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum.Forms.Controls;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MarinMol;

namespace Juegazo.EntityComponents
{
  public class NPCComponent : Component
  {
    private int dialogStart = 0;
    private int dialogEnd = 1;
    public string name { get; protected set; }
    public Rectangle interactiveArea { get; protected set; } = new();
    public bool displayBox = false;
    private ColoredRectangleRuntime background;
    private bool displayCreature;
    StackPanel stack;
    public NPCComponent(string name, int dialogStart, int dialogEnd)
    {
      this.EnableUpdate = true;
      this.name = name;
      this.dialogStart = dialogStart;
      this.dialogEnd = dialogEnd;
      this.EnableDraw = true;
      stack = new();
      stack.AddToRoot();
    }
    public override void Start()
    {
      interactiveArea = new Rectangle(
              Owner.collider.X - Owner.collider.Width,
              Owner.collider.Y - Owner.collider.Height,
              Owner.collider.Width * 3, Owner.collider.Height * 3);
    }
    public override void Destroy()
    {
      Console.WriteLine("hay causita no quiero irme");
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
      Debugger.Instance.DrawRectHollow(spriteBatch, interactiveArea, 4, Color.White);
    }
    public void DrawUI(GameTime gameTime, SpriteBatch spriteBatch)
    {
      if(displayBox)
      {
        ColoredRectangleRuntime c = new()
        { Color = new(Colorazos.GruvBlue), };
        TextRuntime t = new()
        { Text="hola" };
        c.AddChild(t);
        stack.AddChild(c);
      }
    }

    public override void Update(GameTime gameTime)
    { }

    public void Collisions(Entity entity)
    {
      if (interactiveArea.Intersects(entity.Destinationrectangle) && entity.TryGetComponent(out KeyboardInputComponent coso) )
      {
        if (coso.btnpSpecial1)
        {
            displayBox = !displayBox;
            entity.entityState = EntityState.TALKING;
        }
      }
    }
  }
}
