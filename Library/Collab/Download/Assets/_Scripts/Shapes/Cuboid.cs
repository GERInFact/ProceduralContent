using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts.Shapes
{
    public class Cuboid : Shape
    {
        [SerializeField] private Slider _rotationSpeed;

        private Slider RotationSpeed => this._rotationSpeed;

        [SerializeField] private Slider _animationSpeed;

        private Slider AnimationSpeed => this._animationSpeed;

        [SerializeField] private Slider[] _vectorMultiplier;

        private Slider[] VectorMultiplier => this._vectorMultiplier;

        [SerializeField] private Text _dataText;

        private Text DataText => this._dataText;

        private bool AnimateCuboid { get; set; }

        public void SetAnimationBool()
        {
            this.AnimateCuboid = !this.AnimateCuboid;
        }

        public override void SetVertexAndTriangleCount()
        {
            this.VertexCount = 36; /* 6 faces * (3 vertices / triangle) * (2 triangles / face) => 36,
                                   even though 24 would be efficient enough for appropriate shading, since the faces are flat*/
            this.TriangleCount = 36;
        }

        private void HandleAnimation()
        {
            var scalar = 1f;
            if (this.AnimateCuboid)
                scalar = Mathf.Sin(Time.time / this.AnimationSpeed.value) / 2f + .5f;

            for (var i = 0; i < this.Vertices.Count; i++)
            {
                this.Vertices[i] *= scalar;
            }

            if (this.RotationSpeed == null) return;
            this.gameObject.transform?.Rotate(
                new Vector3(this.RotationSpeed.value, this.RotationSpeed.value, this.RotationSpeed.value) *
                Time.deltaTime);
        }

        private void SetDataText()
        {
            if (this.DataText == null || this.InspectorVerts == null || this.AnimationSpeed == null ||
                this.RotationSpeed == null) return;


            this.DataText.text =
                $"V0 = {this.InspectorVerts[0]}\nV1 = {this.InspectorVerts[1].ToString()}\nV2 = {this.InspectorVerts[2]}\nV3 = {this.InspectorVerts[3]}\nV4 = {this.InspectorVerts[4]}\n" +
                $"V5 = {this.InspectorVerts[5]}\nV6 = {this.InspectorVerts[6]}\nV7 = {this.InspectorVerts[7]}\n\n" +
                $"---------\nRotation  speed: {this.RotationSpeed.value:n2}\nAnim deceleration: {this.AnimationSpeed.value:n2}\nFPS: {Time.frameCount / Time.time:n2}\n";
            ;
        }

        protected override bool ValidateMesh()
        {
            return false;
        }

        protected override void SetVertices()
        {
            if (this.Vertices == null || this.InspectorVerts == null || this.VectorMultiplier == null ||
                this.AnimationSpeed == null) return;

            //start rendering at front bottom right
            this.Vertices.Add(this.InspectorVerts[3] * this.VectorMultiplier[3].value);
            this.Vertices.Add(this.InspectorVerts[0] * this.VectorMultiplier[0].value);
            this.Vertices.Add(this.InspectorVerts[1] * this.VectorMultiplier[1].value);

            this.Vertices.Add(this.InspectorVerts[3] * this.VectorMultiplier[3].value);
            this.Vertices.Add(this.InspectorVerts[1] * this.VectorMultiplier[1].value);
            this.Vertices.Add(this.InspectorVerts[2] * this.VectorMultiplier[2].value);

            this.Vertices.Add(this.InspectorVerts[7]);
            this.Vertices.Add(this.InspectorVerts[3] * this.VectorMultiplier[3].value);
            this.Vertices.Add(this.InspectorVerts[2]);

            this.Vertices.Add(this.InspectorVerts[7] * this.VectorMultiplier[7].value);
            this.Vertices.Add(this.InspectorVerts[2] * this.VectorMultiplier[2].value);
            this.Vertices.Add(this.InspectorVerts[6] * this.VectorMultiplier[6].value);

            this.Vertices.Add(this.InspectorVerts[4] * this.VectorMultiplier[4].value);
            this.Vertices.Add(this.InspectorVerts[6] * this.VectorMultiplier[6].value);
            this.Vertices.Add(this.InspectorVerts[5] * this.VectorMultiplier[5].value);

            this.Vertices.Add(this.InspectorVerts[4] * this.VectorMultiplier[4].value);
            this.Vertices.Add(this.InspectorVerts[7] * this.VectorMultiplier[7].value);
            this.Vertices.Add(this.InspectorVerts[6] * this.VectorMultiplier[6].value);

            this.Vertices.Add(this.InspectorVerts[2] * this.VectorMultiplier[2].value);
            this.Vertices.Add(this.InspectorVerts[1] * this.VectorMultiplier[1].value);
            this.Vertices.Add(this.InspectorVerts[5] * this.VectorMultiplier[5].value);

            this.Vertices.Add(this.InspectorVerts[2] * this.VectorMultiplier[2].value);
            this.Vertices.Add(this.InspectorVerts[5] * this.VectorMultiplier[5].value);
            this.Vertices.Add(this.InspectorVerts[6] * this.VectorMultiplier[6].value);

            this.Vertices.Add(this.InspectorVerts[0] * this.VectorMultiplier[0].value);
            this.Vertices.Add(this.InspectorVerts[4] * this.VectorMultiplier[4].value);
            this.Vertices.Add(this.InspectorVerts[5] * this.VectorMultiplier[5].value);

            this.Vertices.Add(this.InspectorVerts[0] * this.VectorMultiplier[0].value);
            this.Vertices.Add(this.InspectorVerts[5] * this.VectorMultiplier[5].value);
            this.Vertices.Add(this.InspectorVerts[1] * this.VectorMultiplier[1].value);

            this.Vertices.Add(this.InspectorVerts[7] * this.VectorMultiplier[7].value);
            this.Vertices.Add(this.InspectorVerts[0] * this.VectorMultiplier[0].value);
            this.Vertices.Add(this.InspectorVerts[3] * this.VectorMultiplier[3].value);

            this.Vertices.Add(this.InspectorVerts[7] * this.VectorMultiplier[7].value);
            this.Vertices.Add(this.InspectorVerts[4] * this.VectorMultiplier[4].value);
            this.Vertices.Add(this.InspectorVerts[0] * this.VectorMultiplier[0].value);

            this.HandleAnimation();

            this.SetDataText();
        }

        protected override void SetTriangles()
        {
            if (this.Vertices == null) return;

            for (var i = 0; i < this.TriangleCount; i++)
            {
                this.Triangles?.Add(i);
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