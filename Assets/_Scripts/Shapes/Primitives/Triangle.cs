#region

using UnityEngine;

#endregion

namespace Assets._Scripts.Shapes.Primitives
{
    public class Triangle : Shape
    {
        [SerializeField] private bool _isBackFaceCulling;


        protected override void SetVertexAndTriangleCount()
        {
            this.VertexCount = 3;
            this.TriangleCount = 3;
        }

        protected override void SetVertices()
        {
            if (this.InspectorVerts == null) return;

            this.Vertices.Clear();

            this.Vertices.AddRange(this.InspectorVerts);
        }


        protected override void SetTriangles()
        {
            this.Triangles.Clear();

            if (this._isBackFaceCulling)
            {
                this.Triangles.Add(0);
                this.Triangles.Add(2);
                this.Triangles.Add(1);
            }
            else
            {
                this.Triangles.Add(0);
                this.Triangles.Add(1);
                this.Triangles.Add(2);
            }
        }


        protected override void SetNormals()
        {
        }

        protected override void SetTangents()
        {
        }

        protected override void SetUvs()
        {
        }

        protected override void SetVertexColors()
        {
        }
    }
}