namespace ResumeReviewer_WebApiCore.Application.Services
{
    public class CosineSimilarityService
    {
        #region Helper Methods
        public static byte[] FloatArrayToBytes(float[] floats)
        {
            var bytes = new byte[floats.Length * 4];
            Buffer.BlockCopy(floats, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        // Helper: byte[] → float[]
        public static float[] BytesToFloatArray(byte[] bytes)
        {
            var floats = new float[bytes.Length / 4];
            Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);
            return floats;
        }

        // Helper: Cosine similarity
        public static float CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            float dot = 0, magA = 0, magB = 0;

            if (vectorA.Length != vectorB.Length)
                return 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dot += vectorA[i] * vectorB[i];
                magA += vectorA[i] * vectorA[i];
                magB += vectorB[i] * vectorB[i];
            }
            return dot / ((float)Math.Sqrt(magA) * (float)Math.Sqrt(magB) + 1e-6f);
        }
        #endregion
    }
}
