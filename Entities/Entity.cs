using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo
{
    public class Entity : Sprite
    {
        public bool Destroyed { get; private set; }
        public bool Enable = true;
        public bool Visible = true;
        private List<Component> componentList = new();
        private Dictionary<Type, Component> componentDictionary = new();
        public Vector2 velocity; //only variable that makes sense right now, and that's debatable.
        public Vector2 baseVelocity = new();
        public Rectangle collider;
        public bool onGround;
        public int health;
        public int maxHealth;
        public bool directionLeft = false;

        public Entity(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Rectangle collider, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = false;
            maxHealth = 1;
            health = maxHealth;
            this.collider = collider;
        }
        public Entity(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, float collider, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = false;
            maxHealth = 1;
            health = maxHealth;
            this.collider = new(Destinationrectangle.X, Destinationrectangle.Y, (int)(Destinationrectangle.Width * collider), (int)(Destinationrectangle.Height * collider));
        }
        public Entity(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, List<Component> componentList, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = false;
            maxHealth = 1;
            health = maxHealth;
            collider = new(Destinationrectangle.X, Destinationrectangle.Y, (int)(Destinationrectangle.Width * 0.8), (int)(Destinationrectangle.Height * 0.8));
            foreach (Component component in componentList)
            {
                AddComponent(component.GetType(), component);
            }

        }
        public Entity(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, List<Component> componentList, float collider, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = false;
            maxHealth = 1;
            health = maxHealth;
            int WidthCollider = (int)(Destinationrectangle.Width * collider);
            int HeightCollider = (int)(Destinationrectangle.Height * collider);
            this.collider = new((int)(Destinationrectangle.X + WidthCollider*0.5), (int)(Destinationrectangle.Y * WidthCollider*0.5), WidthCollider, HeightCollider);
            foreach (Component component in componentList)
            {
                AddComponent(component.GetType(), component);
            }

        }
        public void UpdateColliderFromDest()
        {
            //Put the collider in the center of the DestinationRectangle (sprite)
            collider.X = Destinationrectangle.Center.X - collider.Width/2;
            collider.Y = Destinationrectangle.Center.Y - collider.Height/2;
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in componentList)
            {
                if (component.EnableUpdate) component.Update(gameTime);
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var component in componentList)
            {
                if (component.EnableDraw)
                    component.Draw(gameTime, spriteBatch);
            }
        }
        
        public Component AddComponent(Type type, Component component)
        {
            if (component.Owner != null)
            {
                throw new Exception("Component " + component + " already has an owner");
            }
            component.Owner = this; //I FUCKING FORFOT TO ADD THIS COMPONENT IM GOING TO KMS
            component.Start(); //i need this if i have to make some rectangles or shit
            componentList.Add(component);
            componentDictionary.Add(type, component);
            return component; //why? because fuck it why not
        }
        public List<Component> AddComponents(List<Component> components)
        {
            foreach (var component in components)
            {
                AddComponent(component.GetType(), component);
            }
            return components;
        }
        public Component getComponent(Type type) => componentDictionary[type];
        public T getComponent<T>() where T : Component => (T)componentDictionary[typeof(T)];
        public bool hasComponent(Type type) => componentDictionary.ContainsKey(type);
        public bool hasComponent<T>() where T : Component => componentDictionary.ContainsKey(typeof(T));
        public Component RemoveComponent(Type type)
        {
            if (componentDictionary.TryGetValue(type, out Component component))
            {
                component.Destroy();
                componentDictionary.Remove(type);
                componentList.Remove(component);
                component.Owner = null;
                return component;
            }
            return null;
        }
        public Component RemoveComponent<T>() where T : Component => RemoveComponent(typeof(T));

        internal bool TryGetComponent<T>(out Component component)
        {
            if (componentDictionary.TryGetValue(typeof(T), out Component cmp))
            {
                component = cmp;
                return true;
            }
            else
            {
                component = null;
                return false;
            }
        }
    }
}