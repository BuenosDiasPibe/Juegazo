using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public Entity(Texture2D texture, Rectangle sourceRectangle, Rectangle Destrectangle, Rectangle collider, List<Component> componentList, Color color) : base(texture, sourceRectangle, Destrectangle, color)
        {
            velocity = new();
            onGround = false;
            maxHealth = 1;
            health = maxHealth;
            this.collider = collider;
            foreach (Component component in componentList)
            {
                AddComponent(component.GetType(), component);
            }

        }
        private void UpdateColliderPosition()
        {
            collider.X = Destinationrectangle.X + Destinationrectangle.Width / 2;
            collider.Y = Destinationrectangle.Y + Destinationrectangle.Height / 2;
        }
        public void UpdateDestinationFromCollider()
        {
            Destinationrectangle.X = collider.X - Destinationrectangle.Width / 2;
            Destinationrectangle.Y = collider.Y - Destinationrectangle.Height / 2;
        }
        public virtual void Update(GameTime gameTime)
        {
            if (Destinationrectangle.X != collider.X - Destinationrectangle.Width / 2 ||
                Destinationrectangle.Y != collider.Y - Destinationrectangle.Height / 2)
            {
                UpdateDestinationFromCollider();
            }
            else
            {
                UpdateColliderPosition();
            }
            foreach (var component in componentList)
            {
                if (component.Enable) component.Update(gameTime);
            }
        }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var component in componentList)
            {
                if (component.Visible) component.Draw(gameTime, spriteBatch);
            }
        }
        public Component AddComponent(Type type, Component component)
        {
            if (component.Owner != null)
            {
                throw new Exception("Component " + component + " already has an owner");
            }
            componentList.Add(component);
            componentDictionary.Add(type, component);
            component.Owner = this; //I FUCKING FORFOT TO ADD THIS COMPONENT IM GOING TO KMS
            return component; //why? because fuck it why not
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
    }
}