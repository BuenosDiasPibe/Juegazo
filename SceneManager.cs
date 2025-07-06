using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Juegazo
{
    public class SceneManager
    {
        private Stack<IScene> sceneManager;
        public SceneManager()
        {
            sceneManager = new();
        }
        public void AddScene(IScene scene)
        {
            scene.LoadContent();
            sceneManager.Push(scene);
        }
        public void RemoveScene()
        {
            sceneManager.Pop();
        }
        public IScene GetScene()
        {
            return sceneManager.Peek();
        }
        public void RemoveAllScenes(){
            sceneManager.Clear();
        }
        public bool hasScenes(){
            return sceneManager.Count > 2;
        }
    }
}