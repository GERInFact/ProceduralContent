#region

using UnityEngine;

#endregion

namespace _Scripts.Tools
{
    [RequireComponent(typeof(MeshFilter))]
    [ExecuteInEditMode]
    public class NormalDisplayer : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField] private bool _drawNormals;

        [SerializeField] [Range(.1f, 10f)] private float _normalLength;

        #region unity_methods

        private void OnDrawGizmosSelected()
        {
            if (_drawNormals)
                DrawNormals();
        }

        #endregion

        #region methods

        private void DrawNormals()
        {
            var normalContainer = MeshNormalContainer();

            if (normalContainer == null) return;

            for (var i = 0; i < normalContainer.GetLength(1); i++)
            {
                //animate normals 
                Gizmos.color = _color * (Mathf.Sin(Time.time) / 2 + .5f);

                //draw normal foreach side 
                Gizmos.DrawLine(normalContainer[0, i], normalContainer[0, i] + _normalLength * normalContainer[1, i]);
            }
        }

        private Vector3[,] MeshNormalContainer()
        {
            var mesh = GetComponent<MeshFilter>().sharedMesh;

            if (mesh == null) return null;

            var normalContainer = new Vector3[2, mesh.vertexCount];


            for (var i = 0; i < mesh.vertexCount; i++)
            {
                normalContainer[0, i] = transform.TransformPoint(mesh.vertices[i]);
                normalContainer[1, i] = transform.TransformPoint(mesh.normals[i]);
            }

            return normalContainer;
        }

        #endregion
    }
}