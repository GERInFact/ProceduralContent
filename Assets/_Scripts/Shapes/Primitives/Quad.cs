using Assets._Scripts.Shapes.Primitives;

namespace _Scripts.Shapes.Primitives
{
    public class Quad : Shape
    {
        #region methods

        private void SetFrontFaceVertices()
        {
            Vertices.Add(InspectorVerts[0]);
            Vertices.Add(InspectorVerts[1]);
            Vertices.Add(InspectorVerts[3]);
            Vertices.Add(InspectorVerts[0]);
            Vertices.Add(InspectorVerts[3]);
            Vertices.Add(InspectorVerts[2]);
        }

        #endregion

        #region abstract_methods

        protected override void SetVertexAndTriangleCount()
        {
            VertexCount = 6;
            TriangleCount = 6;
        }

        protected override void SetVertices()
        {
            Vertices.Clear();
            SetFrontFaceVertices();
        }

        protected override void SetTriangles()
        {
            Triangles.Clear();

            for (var i = 0; i < Vertices.Count; i++)
                Triangles.Add(i);
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