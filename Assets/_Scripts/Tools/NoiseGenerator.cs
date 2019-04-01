#region

using UnityEngine;
using _Scripts.Structures;

#endregion

namespace _Scripts.Tools
{
    public class NoiseGenerator
    {
        #region fields

        private readonly int _layers;
        private readonly float _lacunarity;
        private readonly float _gain;
        private readonly float _perlinScale;

        #endregion

        #region constructor

        public NoiseGenerator()
        {
        }

        public NoiseGenerator(int layers, float lacunarity, float gain, float perlinScale)
        {
            _layers = layers;
            _lacunarity = lacunarity;
            _gain = gain;
            _perlinScale = perlinScale;
        }

        #endregion

        #region methods

        public float GetValueNoise()
        {
            return Random.value;
        }

        public float GetPerlinNoise(Vector2 landscapeCoordinates)
        {
            return 2 * Mathf.PerlinNoise(landscapeCoordinates.x, landscapeCoordinates.y) - 1;
        }

        public float GetFractalNoise(Vector2 landscapeCoordinates)
        {
            var map = new Heightmap();
            for (var i = 0; i < _layers; i++)
            {
                var freqVal = new Vector2(landscapeCoordinates.x * map.Frequency * _perlinScale,
                    landscapeCoordinates.y * map.Frequency * _perlinScale);

                map.Update(_lacunarity, _gain, GetPerlinNoise(freqVal));
            }

            return map.Noise;
        }

        #endregion
    }
}