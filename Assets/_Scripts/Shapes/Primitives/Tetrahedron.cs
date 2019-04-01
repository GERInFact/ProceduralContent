using Assets._Scripts.Shapes.Primitives;

namespace _Scripts.Shapes.Primitives
{
    public class Tetrahedron : Shape
    {
        protected override void SetVertexAndTriangleCount()
        {
            VertexCount = 12;
            TriangleCount = 12; //tetrahedron = 4 trangular sides, 4* physical triangles (3 ints) => 12 
        }

        protected override void SetVertices()
        {
            Vertices.Clear();

            Vertices.Add(InspectorVerts[0]);
            Vertices.Add(InspectorVerts[2]);
            Vertices.Add(InspectorVerts[1]);

            Vertices.Add(InspectorVerts[0]);
            Vertices.Add(InspectorVerts[3]);
            Vertices.Add(InspectorVerts[2]);

            Vertices.Add(InspectorVerts[2]);
            Vertices.Add(InspectorVerts[3]);
            Vertices.Add(InspectorVerts[1]);

            Vertices.Add(InspectorVerts[1]);
            Vertices.Add(InspectorVerts[3]);
            Vertices.Add(InspectorVerts[0]);
        }

        protected override void SetTriangles()
        {
            Triangles.Clear();

            for (var i = 0; i < TriangleCount; i++)
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
    }
}