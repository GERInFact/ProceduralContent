#region

using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.Interfaces;
using Assets._Scripts.Shapes.Primitives;
using UnityEngine;
using _Scripts.Shapes.Primitives;

#endregion

namespace _Scripts.Tools
{
    public class SceneCreator : MonoBehaviour, ISceneRenderer
    {
        [SerializeField] private List<Shape> _shapes;


        #region properties

        public List<Shape> Shapes
        {
            get { return _shapes ?? FindObjectsOfType<Shape>()?.ToList(); }
            set { _shapes = value; }
        }

        public event EventHandler SceneRendered;

        public Shape this[int index] => index < Shapes.Count ? _shapes[index] : null;

        #endregion

        #region methods

        protected virtual void OnSceneRender()
        {
            SceneRendered?.Invoke(this, EventArgs.Empty);
        }

        public void RenderScene()
        {
            OnSceneRender();
        }

        #endregion
    }
}