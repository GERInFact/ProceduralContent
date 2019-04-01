#region

using System.Collections.Generic;
using Assets._Scripts.Shapes.Primitives;
using UnityEngine;
using _Scripts.Tools;

#endregion

namespace Assets._Scripts.Shapes.Terrain._2D_Landscapes
{
    public class Terrain2D : Shape
    {
        #region fields

        [SerializeField] private int _resolution = 20;
        [SerializeField] private float _uvScale = .5f;
        [SerializeField] private float _texturesPerSquare = 1f;
        [SerializeField] private float _terrainScale = 1f;
        [SerializeField] private float _yScale = 1f;
        [SerializeField] private float _xScale = 1f;
        [SerializeField] private int _octaves = 1;
        [SerializeField] private float _lacunarity = 2f;
        [SerializeField] private float _gain = .5f;
        [SerializeField] private float _perlinScale = 1f;

        #endregion

        #region properties

        protected float UvScale => this._uvScale;
        protected float TerrainScale => this._terrainScale;
        protected float YScale => this._yScale;
        protected float XScale => this._xScale;
        protected int Octaves => this._octaves;
        protected float Lacunarity => this._lacunarity;
        protected float Gain => this._gain;
        protected float PerlinScale => this._perlinScale;

        #endregion

        #region abstract_methods 

        protected override void SetVertexAndTriangleCount()
        {
            this.VertexCount = 2 * this._resolution <= 65000 ? 2 * this._resolution : 65000;
            this.TriangleCount = 6 * (this._resolution - 1);
        }

        protected override void SetVertices()
        {
            if (this._resolution > 32500 || this._resolution < 0) return;

            this.Vertices.Clear();
            this.Vertices.AddRange(this.GetNoisedVertices());
        }


        protected override void SetTriangles()
        {
            if (this._resolution + 1 > 32500 || this._resolution < 0) return;

            this.Triangles.Clear();
            for (var i = 0; i < this._resolution - 1; i++)
            {
                this.SetBottomLeftTriangles(i);
                this.SetTopRightTriangles(i);
            }
        }

        protected override void SetNormals()
        {
        }


        protected override void SetUvs()
        {
            this.UVs.Clear();
            this.UVs.AddRange(this.CalculateUvs());
        }

        protected override void SetVertexColors()
        {
        }

        #endregion

        #region methods

        private Vector3 GetNoisedCoords(int index, NoiseGenerator noise)
        {
            var x = (float) index / this._resolution * this.XScale;
            var y = this.YScale * noise.GetFractalNoise(new Vector2(x, 0f));
            return new Vector3(x, y, 0f);
        }

        private IEnumerable<Vector3> GetNoisedVertices()
        {
            var verts = new Vector3[this.VertexCount];
            var noise = new NoiseGenerator(this.Octaves, this.Lacunarity, this.Gain, this.PerlinScale);

            for (var i = 0; i < this._resolution; i++)
            {
                var coords = this.GetNoisedCoords(i, noise);
                //top
                verts[i] = coords;
                //bottom
                verts[i + this._resolution] = new Vector3(coords.x, coords.y - this.TerrainScale, 0);
            }

            return verts;
        }

        private void SetTopRightTriangles(int index)
        {
            this.Triangles.Add(index);
            this.Triangles.Add(index + 1);
            this.Triangles.Add(index + this._resolution + 1);
        }

        private void SetBottomLeftTriangles(int index)
        {
            this.Triangles.Add(index);
            this.Triangles.Add(index + this._resolution + 1);
            this.Triangles.Add(index + this._resolution);
        }

        private IEnumerable<Vector2> CalculateUvs()
        {
            var uvs = new Vector2[this.VertexCount];
            for (var i = 0; i < this._resolution; i++)
            {
                uvs[i] = new Vector2(i * this._texturesPerSquare / this._uvScale, 1);
                uvs[i + this._resolution] = new Vector2(i * this._texturesPerSquare / this._uvScale, 0f);
            }

            return uvs;
        }

        #endregion
    }
}