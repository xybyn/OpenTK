// unset

namespace OpenTKProject
{
    public abstract class BufferBase
    {
        public int Program { get; protected set; }

        public abstract void Bind();

        public abstract void Free();
    }
}