using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class SceneManager
    {
        private List<IScene> sceneManager;
        public SceneManager()
        {
            sceneManager = new();
        }
        public void AddScene(IScene scene)
        {
            scene.LoadContent();
            sceneManager.Add(scene);
        }
        public void RemoveScene()
        {
            //Maybe unnesesary, but it works so fuck it we ball
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