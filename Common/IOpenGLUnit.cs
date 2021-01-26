namespace Common
{
    public interface IOpenGLUnit
    {
        int ProgramID { get; }

        void Free();

        void Bind();
    }
}