#region

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Assets._Scripts.Shapes.Primitives
{
    public class ArbitraryPrism : Polygon
    {
        #region fields

        [SerializeField] private Slider[] _radiiSlider;
        [SerializeField] private Slider _depthSlider;
        [SerializeField] private Slider _sides;
        [SerializeField] private Gradient _vertexColorGradient;
        [SerializeField] private float _tailRadius;
        [SerializeField] private float _depth;

        #endregion

        #region abstract_methods

        protected override void SetVertexAndTriangleCount()
        {
            this.VertexCount = 6 * this.SideCount;
            this.TriangleCount = 12 * (this.SideCount - 1);
        }

        protected override void SetVertices()
        {
            this.Vertices.Clear();

            var verts = this.DefineVertices();

            this.SetFrontFaceVertices(verts);
            this.SetCenterVertices(verts);
            this.SetBackFaceVertices(verts);

            this.transform.Rotate(new Vector3(3f, 1f, 5f) * Time.deltaTime);
        }


        protected override void SetTriangles()
        {
            this.Triangles.Clear();

            //head 
            base.SetTriangles();

            this.SetCenterTriangles();
            this.SetBackFaceTriangles();
        }

        protected override void SetUvs()
        {
            this.UVs.Clear();

            this.SetFrontFaceUvs();
            this.SetCenterUvs();
            this.CalcBackFaceUvs();
        }


        protected override void SetNormals()
        {
            this.Normals.Clear();

            this.SetFrontFaceNormals();
            this.SetCenterNormals();
            this.SetBackFaceNormals();
        }


        protected override void SetVertexColors()
        {
            if (this._vertexColorGradient == null) return;

            this.VertexColors.Clear();
            for (var i = 0; i < this.Vertices.Count; i++)
                this.VertexColors.Add(this._vertexColorGradient.Evaluate((float) i / this.VertexCount));
        }

        #endregion

        #region methods

        private Vector3[] DefineVertices()
        {
            var verts = new Vector3[this.SideCount * 2];
            for (var i = 0; i < this.SideCount; i++)
            {
                var angle = 2 * Mathf.PI * i / this.SideCount;

                verts[i] = new Vector3(Mathf.Cos(angle) * this.Radius, Mathf.Sin(angle) * this.Radius, 0);
                verts[i + this.SideCount] = new Vector3(Mathf.Cos(angle) * this._tailRadius,
                    Mathf.Sin(angle) * this._tailRadius,
                    this._depth);
            }

            return verts;
        }

        private void SetBackFaceVertices(IReadOnlyList<Vector3> verts)
        {
            for (var i = 0; i < this.SideCount; i++)
                this.Vertices.Add(verts[i + this.SideCount]);
        }

        private void SetCenterVertices(IReadOnlyList<Vector3> verts)
        {
            for (var i = 0; i < this.SideCount; i++)
            {
                this.Vertices.Add(verts[i]);

                var secondIndex = i == 0 ? 2 * this.SideCount - 1 : this.SideCount + i - 1;
                this.Vertices.Add(verts[secondIndex]);
                var thirdIndex = i == 0 ? this.SideCount - 1 : i - 1;
                this.Vertices.Add(verts[thirdIndex]);

                this.Vertices.Add(verts[i + this.SideCount]);
            }
        }

        private void SetFrontFaceVertices(IReadOnlyList<Vector3> verts)
        {
            for (var i = 0; i < this.SideCount; i++)
                this.Vertices.Add(verts[i]);
        }


        private void SetBackFaceTriangles()
        {
            for (var i = 1; i < this.SideCount - 1; i++)
            {
                this.Triangles.Add(5 * this.SideCount);
                this.Triangles.Add(i + 5 * this.SideCount);
                this.Triangles.Add(i + 1 + 5 * this.SideCount);
            }
        }

        private void SetCenterTriangles()
        {
            for (var i = 1; i <= this.SideCount; i++)
            {
                var order = this.SideCount + 4 * (i - 1);

                this.Triangles.Add(order);
                this.Triangles.Add(order + 1);
                this.Triangles.Add(order + 2);

                this.Triangles.Add(order);
                this.Triangles.Add(order + 3);
                this.Triangles.Add(order + 1);
            }
        }

        private void CalcBackFaceUvs()
        {
            for (var i = 0; i < this.SideCount; i++)
                this.UVs.Add(this.Vertices[i + this.SideCount]);
        }

        private void SetCenterUvs()
        {
            for (var i = 0; i < this.SideCount; i++)
            {
                this.UVs.Add(new Vector2(1, 0));
                this.UVs.Add(new Vector2(0, 1));
                this.UVs.Add(new Vector2(0, 0));
                this.UVs.Add(new Vector2(1, 1));
            }
        }

        private void SetFrontFaceUvs()
        {
            for (var i = 0; i < this.SideCount; i++)
                this.UVs.Add(this.Vertices[i]);
        }

        private void SetBackFaceNormals()
        {
            for (var i = 0; i < this.SideCount; i++)
                this.Normals.Add(Vector3.forward);
        }

        private void SetCenterNormals()
        {
            for (var i = 0; i < this.SideCount; i++)
            {
                var index = 4 * i + this.SideCount;
                var v1 = this.Vertices[index + 2] - this.Vertices[index];
                var v2 = this.Vertices[index + 3] - this.Vertices[index];

                var normal = Vector3.Cross(v2, v1).normalized;
                for (var nC = 0; nC < 4; nC++)
                    this.Normals.Add(normal);
            }
        }

        private void SetFrontFaceNormals()
        {
            for (var i = 0; i < this.SideCount; i++)
                this.Normals.Add(Vector3.back);
        }

        #endregion

        #region ui_event_methods

        public void SetRadius()
        {
            if (this._radiiSlider?.Length > 0) this.Radius = this._radiiSlider[0].value;
        }

        public void SetTailRadius()
        {
            if (this._radiiSlider?.Length > 1) this._tailRadius = this._radiiSlider[1].value;
        }

        public void SetDepth()
        {
            if (this._depthSlider == null) return;

            this._depth = this._depthSlider.value;
        }

        public void SetSides()
        {
            if (this._sides == null) return;

            this._sides.minValue = 3;
            this.SideCount = (int) this._sides.value;
        }

        #endregion
    }
}