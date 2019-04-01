#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace _Scripts.Structures
{
    public struct GeometricTriangle
    {
        public int TriA { get; }
        public int TriB { get; }
        public int TriC { get; }
        public Vector2 Uva { get; private set; }
        public Vector2 Uvb { get; private set; }
        public Vector2 Uvc { get; private set; }


        public GeometricTriangle(int triA, int triB, int triC)
        {
            TriA = triA;
            TriB = triB;
            TriC = triC;
            Uva = Vector2.zero;
            Uvb = Vector2.zero;
            Uvc = Vector2.zero;
        }

        public void SetUVs(IReadOnlyList<Vector2> uvs)
        {
            if (uvs == null) return;

            Uva = uvs[TriA];
            Uvb = uvs[TriB];
            Uvc = uvs[TriC];
        }
    }
}