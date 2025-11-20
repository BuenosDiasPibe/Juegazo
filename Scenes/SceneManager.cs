using System;
using System.Collections.Generic;
using System.Linq;
using MarinMol.Scenes;
using Microsoft.Xna.Framework.Content;
using MonoGameGum;

namespace Juegazo
{
    public class SceneManager
    {
      private List<IScene> sceneManager = new();
      public Dictionary<string, Action> ActionByName = new();
      public ContentManager Content; 
      GumService gum;
      public SceneManager(ContentManager Content, GumService gum) //TODO: probably add more things here
      {
        this.Content = Content;
        this.gum = gum;
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
