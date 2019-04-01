namespace _Scripts.Structures
{
    public class Heightmap
    {
        #region methods

        public void Update(float lacunarity, float gain, float noise)
        {
            Amplitude *= gain;
            Frequency *= lacunarity;
            Noise += Amplitude * noise;
        }

        #endregion

        #region properties

        public float Frequency { get; set; }
        public float Amplitude { get; set; }
        public float Noise { get; set; }

        #endregion

        #region contructor

        public Heightmap()
        {
            Noise = .0f;
            Frequency = 1f;
            Amplitude = 1f;
        }

        public Heightmap(float frequency, float amplitude, float noise)
        {
            Noise = noise;
            Frequency = frequency;
            Amplitude = amplitude;
        }

        #endregion
    }
}