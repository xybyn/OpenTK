using OpenTK.Graphics.OpenGL4;

namespace Common.Misc
{
    public class VertexAttribPointer
    {
        public int Index { get; init; }
        public int Size { get; init; }
        public VertexAttribPointerType Type { get; init; }
        public bool Normalize { get; init; }
        public int StrideInBytes { get; init; }
        public int OffsetInBytes { get; init; }

        public void Enable()
        {
            GL.VertexAttribPointer(Index, Size, Type, Normalize, StrideInBytes, OffsetInBytes);
            GL.EnableVertexAttribArray(Index);
        }
    }
}