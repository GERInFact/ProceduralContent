#region

using System;
using System.Collections.Generic;
using Assets._Scripts.Interfaces;
using UnityEngine;
using _Scripts.Structures;

#endregion

namespace Assets._Scripts.Shapes.Primitives
{
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public abstract class Shape : MonoBehaviour
    {
        #region fields

        [SerializeField] private Material _material;
        [SerializeField] private readonly Vector3[] _inspectorVerts = new Vector3[3];
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        #endregion

        #region properties

        protected Vector3[] InspectorVerts => this._inspectorVerts;

        protected int VertexCount { get; set; }
        protected int TriangleCount { get; set; }
        protected List<Vector3> Vertices { get; } = new List<Vector3>();
        protected List<int> Triangles { get; } = new List<int>();
        protected List<Vector3> Normals { get; } = new List<Vector3>();
        protected List<Vector4> Tangents { get; } = new List<Vector4>();
        protected List<Vector2> UVs { get; } = new List<Vector2>();
        protected List<Color32> VertexColors { get; } = new List<Color32>();
        protected Mesh Mesh { get; private set; }

        #endregion

        #region abstractMethods

        /// <summary>
        ///     set the meshs vertex and triangle count
        /// </summary>
        protected abstract void SetVertexAndTriangleCount();

        /// <summary>
        ///     calculates the meshs verticies order
        /// </summary>
        protected abstract void SetVertices();

        /// <summary>
        ///     calculates triangle order based on the mesh's verticies order
        /// </summary>
        protected abstract void SetTriangles();

        protected abstract void SetNormals();

        protected abstract void SetUvs();

        protected virtual void SetTangents()
        {
            if (this.VertexCount == 0 || this.Normals.Count == 0) return;

            this.Tangents.Clear();

            var geometricTriangleCount = this.TriangleCount / 3;
            var tangents = new Vector3[this.VertexCount];
            var biTangents = new Vector3[this.VertexCount];

            var index = 0;
            for (var i = 0; i < geometricTriangleCount; i++)
            {
                var gT = this.GetGeometricTriangle(index);
                gT.SetUVs(this.UVs);

                var dirA = this.Vertices[gT.TriB] - this.Vertices[gT.TriA];
                var dirB = this.Vertices[gT.TriC] - this.Vertices[gT.TriA];

                var uvDiffA = new Vector2(gT.Uvb.x - gT.Uva.x, gT.Uvc.x - gT.Uva.x);
                var uvDiffB = new Vector2(gT.Uvb.y - gT.Uva.y, gT.Uvc.y - gT.Uva.y);

                var determinat = 1f / (uvDiffA.x * uvDiffB.y - uvDiffA.y * uvDiffB.x);
                var sDir = determinat * new Vector3(uvDiffB.y * dirA.x - uvDiffB.x * dirB.x,
                               uvDiffB.y * dirA.y - uvDiffB.x * dirB.y,
                               uvDiffB.y * dirA.z - uvDiffB.x * dirB.z);
                var tDir = determinat * new Vector3(uvDiffA.x * dirB.x - uvDiffA.y * dirA.x,
                               uvDiffA.x * dirB.y - uvDiffA.y * dirA.y,
                               uvDiffA.x * dirB.z - uvDiffA.y * dirA.z);

                tangents[gT.TriA] += sDir;
                tangents[gT.TriB] += sDir;
                tangents[gT.TriC] += sDir;

                biTangents[gT.TriA] += tDir;
                biTangents[gT.TriB] += tDir;
                biTangents[gT.TriC] += tDir;

                index += 3;
            }

            this.CalculateTangents(tangents, biTangents);
        }

        private void CalculateTangents(IReadOnlyList<Vector3> tangents, IReadOnlyList<Vector3> biTangents)
        {
            if (tangents == null || biTangents == null) return;

            for (var i = 0; i < this.VertexCount; i++)
            {
                var normal = this.Normals[i];
                var tan = tangents[i];

                var tempTangent = (tan - Vector3.Dot(normal, tan) * normal).normalized;
                Vector4 tangent = tempTangent;

                tangent.w = Vector3.Dot(Vector3.Cross(normal, tan), biTangents[i]) < 0f ? -1f : 1f;
                this.Tangents.Add(tangent);
            }
        }

        protected abstract void SetVertexColors();

        #endregion

        #region methods

        /// <summary>
        ///     handles essential mesh calculations (verts, tris, uvs,...)
        /// </summary>
        private void CalculateMeshData()
        {
            this.SetVertexAndTriangleCount();
            this.SetVertices();
            this.SetTriangles();
            this.SetNormals();
            this.SetUvs();
            this.SetTangents();
            this.SetVertexColors();
        }


        private GeometricTriangle GetGeometricTriangle(int index)
        {
            return new GeometricTriangle(this.Triangles[index], this.Triangles[index + 1], this.Triangles[index + 2]);
        }

        /// <summary>
        ///     initialises mesh filter and mesh renderer
        /// </summary>
        public void InitMeshComponents()
        {
            if (this._material == null)
                return;

            this._meshFilter = this.GetComponent<MeshFilter>();
            this._meshRenderer = this.GetComponent<MeshRenderer>();
            this._meshRenderer.material = this._material;
        }


        /// <summary>
        ///     applies mesh data to the actual unity mesh to be rendered
        /// </summary>
        private void ApplyMeshData(object sender, EventArgs e)
        {
            this.Mesh = new Mesh();

            this.CalculateMeshData();
            this.Mesh.SetVertices(this.Vertices);
            this.Mesh.SetTriangles(this.Triangles, 0);
            this.Mesh.SetUVs(0, this.UVs);
            this.Mesh.SetTangents(this.Tangents);
            this.Mesh.SetColors(this.VertexColors);
            this.Mesh.RecalculateNormals();

            this._meshFilter.mesh = this.Mesh;
        }

        public void AddToRenderer(ISceneRenderer sceneRenderer)
        {
            sceneRenderer.SceneRendered += this.ApplyMeshData;
        }

        public void RemoveFromRenderer(ISceneRenderer sceneRenderer)
        {
            sceneRenderer.SceneRendered -= this.ApplyMeshData;
        }

        #endregion
    }
}