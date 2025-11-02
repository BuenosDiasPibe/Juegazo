using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juegazo.EntityComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Juegazo
{
    public enum EntityState
    {
        FALLING,
        ON_GROUND,
        JUMPING,
        WALKING,
        ON_JUMP_WALL,
        SWIMMING,
        ON_JUMPING_PAD,
        POWERUP,
        JUMPING_ON_AIR,
        DYING,
        TALKING,
        DASHING,
        GOT_KEY,
        UP_MOVING_BLOCK,
    }
    public class Entity : Sprite
    {
        public bool isPlayer = false; // temp
        public bool isPlayable = false; // temp
        public bool Destroyed { get; private set; }
        public bool Enable = true;
        public bool Visible = true;
        public List<Component> componentList { get; private set; } = new();
        private Dictionary<Type, Component> componentDictionary = new();
        public Vector2 velocity;
        public Vector2 baseVelocity = new();
        public Rectangle collider;
        public bool onGround = false;
        public int health;
        public int maxHealth;
        public bool directionLeft = false;
        public FACES direction = FACES.BOTTOM; //this should be inside another component
        public EntityState entityState = new(); //TODO: should this be added to another component?
        public bool touchingWaterBlock = false; //TODO: find a better way to check this
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
            collider.X = Destinationrectangle.Center.X - collider.Width / 2;
            collider.Y = Destinationrectangle.Center.Y - collider.Height / 2;
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in componentList.ToList()) // Create temporary copy for iteration //this is a copilot comment, fuck you c#, it took me a shit lot of time and writing to get this shit right fuck you
            {
                if (component.EnableUpdate) component.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            bool componentDrew = false;
            foreach (var component in componentList.ToList())
            {
                if (component.EnableDraw)
                {
                    component.Draw(gameTime, spriteBatch);
                    componentDrew = true;
                }
            }
            if (!componentDrew) //i swear to god it took a long time for trial and error, fuck this shit
                spriteBatch.Draw(texture, Destinationrectangle, sourceRectangle, color);
        }
        
        public Component AddComponent(Type type, Component component)
        {
            if (component.Owner != null)
            {
                throw new Exception("Component " + component + " already has an owner");
            }
            
            component.Owner = this;
            component.Start();
            
            componentList.Add(component);
            componentDictionary.Add(type, component);
            
            return component;
        }

        public Component RemoveComponent(Type type)
        {
            if (componentDictionary.TryGetValue(type, out Component component))
            {
                component.Destroy();
                component.Owner = null;
                componentList.Remove(component);
                componentDictionary.Remove(type);
                return component;
            }
            return null;
        }
        public List<Component> AddComponents(List<Component> components)
        {
            foreach (var component in components)
            {
                AddComponent(component.GetType(), component);
            }
            return components;
        }
        public Component GetComponent(Type type) => componentDictionary.TryGetValue(type, out Component component) ? component : null;
        public Component GetComponent<T>() where T : Component => componentDictionary.TryGetValue(typeof(T), out Component component) ? component : null;
        public bool hasComponent(Type type) => componentDictionary.ContainsKey(type);
        public bool hasComponent<T>() where T : Component => componentDictionary.ContainsKey(typeof(T));

        public Component RemoveComponent<T>() where T : Component => RemoveComponent(typeof(T));

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            var result = componentDictionary.TryGetValue(typeof(T), out Component c);
            component = (T)c;
            return result;
        }
    }
}