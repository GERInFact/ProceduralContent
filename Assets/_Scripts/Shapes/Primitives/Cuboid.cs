#region

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Assets._Scripts.Shapes.Primitives
{
    public class Cuboid : Shape
    {
        #region properties

        private Text DataText => this._dataText;

        #endregion

        #region fields

        [SerializeField] private Slider _rotationSpeedSlider;
        [SerializeField] private Slider _animationSpeedSlider;
        [SerializeField] private Slider[] _verticiesManipulatorSlider;
        [SerializeField] private Text _dataText;
        private bool _animateCuboid;

        #endregion

        #region abstract_Methods

        protected override void SetVertexAndTriangleCount()
        {
            this.VertexCount = 36; /* 6 faces * (3 vertices / triangle) * (2 triangles / face) => 36,
                                   even though 24 would be efficient enough for appropriate shading, since the faces are flat*/
            this.TriangleCount = 36;
        }

        protected virtual bool ValidateMesh()
        {
            return false;
        }

        protected override void SetVertices()
        {
            if (this.InspectorVerts == null || this.IsUiElementMissing()) return;

            this.Vertices.Clear();

            this.SetFrontFaceVertices();
            this.SetRightFaceVertices();
            this.SetBackFaceVertices();
            this.SetTopFaceVertices();
            this.SetLeftFaceVertices();
            this.SetBottomFaceVertices();

            this.HandleAnimation();

            this.SetDataText();
        }

        protected override void SetTriangles()
        {
            this.Triangles.Clear();

            for (var i = 0; i < this.TriangleCount; i++)
                this.Triangles.Add(i);
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

        #region methods

        private void SetFrontFaceVertices()
        {
            this.Vertices.Add(this.InspectorVerts[3] * this._verticiesManipulatorSlider[3].value);
            this.Vertices.Add(this.InspectorVerts[0] * this._verticiesManipulatorSlider[0].value);
            this.Vertices.Add(this.InspectorVerts[1] * this._verticiesManipulatorSlider[1].value);
            this.Vertices.Add(this.InspectorVerts[3] * this._verticiesManipulatorSlider[3].value);
            this.Vertices.Add(this.InspectorVerts[1] * this._verticiesManipulatorSlider[1].value);
            this.Vertices.Add(this.InspectorVerts[2] * this._verticiesManipulatorSlider[2].value);
        }

        private void SetRightFaceVertices()
        {
            this.Vertices.Add(this.InspectorVerts[7]);
            this.Vertices.Add(this.InspectorVerts[3] * this._verticiesManipulatorSlider[3].value);
            this.Vertices.Add(this.InspectorVerts[2]);
            this.Vertices.Add(this.InspectorVerts[7] * this._verticiesManipulatorSlider[7].value);
            this.Vertices.Add(this.InspectorVerts[2] * this._verticiesManipulatorSlider[2].value);
            this.Vertices.Add(this.InspectorVerts[6] * this._verticiesManipulatorSlider[6].value);
        }

        private void SetBackFaceVertices()
        {
            this.Vertices.Add(this.InspectorVerts[4] * this._verticiesManipulatorSlider[4].value);
            this.Vertices.Add(this.InspectorVerts[6] * this._verticiesManipulatorSlider[6].value);
            this.Vertices.Add(this.InspectorVerts[5] * this._verticiesManipulatorSlider[5].value);
            this.Vertices.Add(this.InspectorVerts[4] * this._verticiesManipulatorSlider[4].value);
            this.Vertices.Add(this.InspectorVerts[7] * this._verticiesManipulatorSlider[7].value);
            this.Vertices.Add(this.InspectorVerts[6] * this._verticiesManipulatorSlider[6].value);
        }

        private void SetTopFaceVertices()
        {
            this.Vertices.Add(this.InspectorVerts[2] * this._verticiesManipulatorSlider[2].value);
            this.Vertices.Add(this.InspectorVerts[1] * this._verticiesManipulatorSlider[1].value);
            this.Vertices.Add(this.InspectorVerts[5] * this._verticiesManipulatorSlider[5].value);
            this.Vertices.Add(this.InspectorVerts[2] * this._verticiesManipulatorSlider[2].value);
            this.Vertices.Add(this.InspectorVerts[5] * this._verticiesManipulatorSlider[5].value);
            this.Vertices.Add(this.InspectorVerts[6] * this._verticiesManipulatorSlider[6].value);
        }

        private void SetLeftFaceVertices()
        {
            this.Vertices.Add(this.InspectorVerts[0] * this._verticiesManipulatorSlider[0].value);
            this.Vertices.Add(this.InspectorVerts[4] * this._verticiesManipulatorSlider[4].value);
            this.Vertices.Add(this.InspectorVerts[5] * this._verticiesManipulatorSlider[5].value);
            this.Vertices.Add(this.InspectorVerts[0] * this._verticiesManipulatorSlider[0].value);
            this.Vertices.Add(this.InspectorVerts[5] * this._verticiesManipulatorSlider[5].value);
            this.Vertices.Add(this.InspectorVerts[1] * this._verticiesManipulatorSlider[1].value);
        }

        private void SetBottomFaceVertices()
        {
            this.Vertices.Add(this.InspectorVerts[7] * this._verticiesManipulatorSlider[7].value);
            this.Vertices.Add(this.InspectorVerts[0] * this._verticiesManipulatorSlider[0].value);
            this.Vertices.Add(this.InspectorVerts[3] * this._verticiesManipulatorSlider[3].value);
            this.Vertices.Add(this.InspectorVerts[7] * this._verticiesManipulatorSlider[7].value);
            this.Vertices.Add(this.InspectorVerts[4] * this._verticiesManipulatorSlider[4].value);
            this.Vertices.Add(this.InspectorVerts[0] * this._verticiesManipulatorSlider[0].value);
        }

        public void SetAnimationBool()
        {
            this._animateCuboid = !this._animateCuboid;
        }


        private void HandleAnimation()
        {
            var scalar = 1f;
            if (this._animateCuboid)
                scalar = Mathf.Sin(Time.time / this._animationSpeedSlider.value) / 2f + .5f;

            for (var i = 0; i < this.Vertices.Count; i++)
                this.Vertices[i] *= scalar;


            this.gameObject.transform.Rotate(
                new Vector3(this._rotationSpeedSlider.value, this._rotationSpeedSlider.value,
                    this._rotationSpeedSlider.value) *
                Time.deltaTime);
        }

        private void SetDataText()
        {
            this.DataText.text =
                $"V0 = {this.InspectorVerts[0] * this._verticiesManipulatorSlider[0].value}\nV1 = {this.InspectorVerts[1] * this._verticiesManipulatorSlider[1].value}\n" +
                $"V2 = {this.InspectorVerts[2] * this._verticiesManipulatorSlider[2].value}\nV3 = {this.InspectorVerts[3] * this._verticiesManipulatorSlider[3].value}\n" +
                $"V4 = {this.InspectorVerts[4] * this._verticiesManipulatorSlider[4].value}\nV5 = {this.InspectorVerts[5] * this._verticiesManipulatorSlider[5].value}\n" +
                $"V6 = {this.InspectorVerts[6] * this._verticiesManipulatorSlider[6].value}\nV7 = {this.InspectorVerts[7] * this._verticiesManipulatorSlider[7].value}\n\n" +
                $"---------\nRotation  speed: {this._rotationSpeedSlider.value:n2}\nAnim deceleration: {this._rotationSpeedSlider.value:n2}\nFPS: {Time.frameCount / Time.time:n2}\n";
        }

        private bool IsUiElementMissing()
        {
            return this.DataText == null ||
                   this._animationSpeedSlider == null ||
                   this._rotationSpeedSlider == null ||
                   this._verticiesManipulatorSlider == null;
        }

        #endregion
    }
}