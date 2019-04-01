using System;
using Assets._Scripts.Shapes;
using UnityEngine;
using _Scripts.Interfaces;
using _Scripts.Shapes;

namespace _Scripts
{
    [ExecuteInEditMode]
    public class SceneCreator : MonoBehaviour, ISceneRenderer
    {
        private Action _meshRenderHandlerDelegate;

        [SerializeField] private Shape[] _shapes;

        #region unitymethods

        private void Awake()
        {
            this.InitSceneShapes();
        }

        private void Update()
        {
            this.RenderScene(this._meshRenderHandlerDelegate);
        }

        #endregion

        #region methods

        public void RenderScene(Action meshRenderHandlerDelegate)
        {
            meshRenderHandlerDelegate?.Invoke();
        }

        /// <summary>
        /// applies all essential procedures for rendering meshes in scene
        /// </summary>
        private void InitSceneShapes()
        {
            if (this._shapes == null) return;

            foreach (var shape in this._shapes)
            {
                if (shape == null) continue;

                shape.InitMeshComponents();

                this._meshRenderHandlerDelegate += shape.CalculateMeshData;
                this._meshRenderHandlerDelegate += shape.ApplyMeshData;
            }
        }

        #endregion
    }
}