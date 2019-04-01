#region

using UnityEngine;

#endregion

namespace Assets._Scripts.Shapes.Primitives
{
    public class Polygon : Shape
    {
        [SerializeField] private float _radius;
        [SerializeField] [Range(3, 100)] private int _sideCount = 3;

        #region properties

        protected float Radius
        {
            get { return this._radius; }
            set { this._radius = value; }
        }

        protected int SideCount
        {
            get { return this._sideCount; }
            set { this._sideCount = value; }
        }

        #endregion

        #region abstract_methods

        protected override void SetVertexAndTriangleCount()
        {
            this.VertexCount = this.SideCount;
            this.TriangleCount = 3 * (this.SideCount - 2);
        }

        protected override void SetVertices()
        {
            if (this.Vertices == null) return;

            this.Vertices.Clear();

            for (var i = 0; i < this.SideCount; i++)
            {
                //add vertices based on trigonometirc calculations 
                var angle = 2 * Mathf.PI * i / this.SideCount;
                this.Vertices.Add(new Vector3(Mathf.Cos(angle) * this.Radius, Mathf.Sin(angle) * this.Radius, 0f));
            }
        }

        protected override void SetTriangles()
        {
            if (this.Triangles == null || this.Vertices == null) return;

            this.Triangles.Clear();

            //define shape, based on calculated vertices 
            //0 will be the origin
            //i+1 the first vector to render
            //and i the very last of each triangle, since the coordinate system is orieted counter clockwise 

            for (var i = 1; i < this.SideCount - 1; i++)
            {
                this.Triangles.Add(0);
                this.Triangles.Add(i + 1);
                this.Triangles.Add(i);
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

        #endregion
    }
}