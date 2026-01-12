namespace ResumeReviewer_WebApiCore.Application.Interfaces
{
    public interface IEmbeddingService
    {
        public float[] GenerateEmbedding(string text);
        //Later you can replace dummy logic with actual LLaMA inference.
    }
}
