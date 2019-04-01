using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _Scripts.Interfaces;
using _Scripts.Shapes;

namespace _Scripts.Manager
{
    public class SceneCreator : MonoBehaviour, ISceneRenderer
    {
        [SerializeField] private List<Shape> _shapes;

        #region properties

        public List<Shape> Shapes
        {
            get { return this._shapes ?? FindObjectsOfType<Shape>()?.ToList(); }
            set { _shapes = value; }
        }

        public Shape this[int index] => index < this.Shapes.Count ? this._shapes[index] : null;

        #endregion

        #region methods

        public void RenderScene(Action meshRenderHandler)
        {
            meshRenderHandler?.Invoke();
        }

        #endregion
    }
}