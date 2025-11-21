using System;
using System.Collections.Generic;
using System.Linq;
using MarinMol.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;

namespace Juegazo
{
    public class SceneManager
    {
      private List<IScene> sceneManager = new();
      public Dictionary<string, Action> ActionByName = new();
      public ContentManager Content; 
      public GraphicsDeviceManager graphics;
      public Viewport viewport;
      GumService gum;
      public SceneManager(ContentManager Content, GraphicsDeviceManager graphics,  GumService gum) //TODO: probably add more things here
      {
        this.Content = Content;
        this.gum = gum;
        this.graphics = graphics;
        this.viewport = new(0,0,graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
      }

      public void AddScene(IScene scene)
      {
          scene.LoadContent();
          sceneManager.Add(scene);
      }
      public void RemoveScene()
      {
          GetScene().UnloadContent();
          sceneManager.RemoveAt(sceneManager.Count - 1);
      }
      public void RemoveAndLoadLastScene()
      {
        GetScene().UnloadContent();
        sceneManager.RemoveAt(sceneManager.Count - 1);
        sceneManager.Last().LoadContent();
      }
      public IScene GetScene()
      {
          return sceneManager.Last();
      }
      public void RemoveAllScenes(){
          sceneManager.Clear();
      }
      public bool hasScenes(){
          return sceneManager.Count > 1;
      }
    }
}
