#region

using System;
using System.Collections.Generic;
using Assets._Scripts.Shapes.Primitives;

#endregion

namespace Assets._Scripts.Interfaces
{
    public interface ISceneRenderer
    {
        List<Shape> Shapes { get; }
        event EventHandler SceneRendered;

        /// <summary>
        ///     Renders all procedural meshes in the scene
        /// </summary>
        void RenderScene();
    }
}