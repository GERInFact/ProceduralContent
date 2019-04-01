#region

using Assets._Scripts.Interfaces;
using UnityEngine;

#endregion

namespace _Scripts.Manager
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(ISceneRenderer))]
    public class GameManager : MonoBehaviour
    {
        #region properties

        private ISceneRenderer _sceneRenderer;

        #endregion

        #region unity_methods

        // Use this for initialization
        private void Awake()
        {
            this._sceneRenderer = this.GetComponent<ISceneRenderer>();
        }


        // Update is called once per frame
        private void Update()
        {
            this._sceneRenderer?.RenderScene();
        }

        #endregion

        #region methods

        /// <summary>
        ///     applies all essential procedures for rendering the meshes
        /// </summary>
        public void AddShapesToSceneRenderer()
        {
            this._sceneRenderer?.Shapes?.ForEach(shape =>
            {
                shape?.InitMeshComponents();
                shape?.AddToRenderer(this._sceneRenderer);
            });
        }

        public void RemoveShapesFromSceneRenderer()
        {
            this._sceneRenderer?.Shapes?.ForEach(shape => { shape?.RemoveFromRenderer(this._sceneRenderer); });
        }

        #endregion
    }
}