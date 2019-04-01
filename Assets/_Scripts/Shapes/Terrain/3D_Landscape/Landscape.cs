#region

using Assets._Scripts.Shapes.Terrain._2D_Landscapes;
using UnityEngine;
using _Scripts.Tools;

#endregion

namespace Assets._Scripts.Shapes.Terrain._3D_Landscape
{
    public class Landscape : Terrain2D
    {
        #region fields

        [SerializeField] private int _xResolution = 20;
        [SerializeField] private int _zResolution = 20;
        [SerializeField] private float _zScale = 1f;
        [SerializeField] private Gradient _gradient;
        [SerializeField] private float _gradientMin = -10f;
        [SerializeField] private float _gradientMax = 15f;

        #endregion

        #region abstract methods

        protected override void SetVertexAndTriangleCount()
        {
            this.VertexCount = (this._xResolution + 1) * (this._zResolution + 1);
            this.TriangleCount = 6 * this._xResolution * this._zResolution;
        }

        protected override void SetVertices()
        {
            this.Vertices.Clear();

            var noise = new NoiseGenerator(this.Octaves, this.Lacunarity, this.Gain, this.PerlinScale);
            this.CalculateVertices(noise);
        }

        protected override void SetTriangles()
        {
            this.Triangles.Clear();

            var triCount = 0;
            for (var z = 0; z < this._zResolution; z++)
            {
                for (var x = 0; x < this._xResolution; x++)
                {
                    this.SetBottomLeftTriangles(triCount);
                    this.SetTopRightTriangles(triCount);

                    triCount++;
                }

                triCount++;
            }
        }

        protected override void SetUvs()
        {
            this.UVs.Clear();

            for (var z = 0; z <= this._zResolution; z++)
            for (var x = 0; x <= this._xResolution; x++)
                this.UVs.Add(new Vector2(x / this.UvScale * this._xResolution, z / this.UvScale * this._zResolution));
        }

        protected override void SetVertexColors()
        {
            this.VertexColors.Clear();

            var diff = this._gradientMax - this._gradientMin;
            for (var i = 0; i < this.VertexCount; i++)
                this.VertexColors.Add(this._gradient.Evaluate(this.Vertices[i].y - this._gradientMin) / diff);
        }

        #endregion

        #region methods

        private void CalculateVertices(NoiseGenerator noise)
        {
            if (noise == null) return;

            for (var z = 0; z <= this._zResolution; z++)
            for (var x = 0; x <= this._xResolution; x++)
            {
                var xx = (float) x / this._xResolution * this.XScale * this.TerrainScale;
                var zz = (float) z / this._zResolution * this._zScale * this.TerrainScale;
                var y = this.YScale * noise.GetFractalNoise(new Vector2(xx, zz));

                this.Vertices.Add(new Vector3(xx, y, zz));
            }
        }

        private void SetBottomLeftTriangles(int index)
        {
            this.Triangles.Add(index);
            this.Triangles.Add(index + this._xResolution + 1);
            this.Triangles.Add(index + 1);
        }

        private void SetTopRightTriangles(int index)
        {
            this.Triangles.Add(index + 1);
            this.Triangles.Add(index + this._xResolution + 1);
            this.Triangles.Add(index + this._xResolution + 2);
        }

        #endregion
    }
}