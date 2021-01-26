using OpenTK.Graphics.OpenGL4;

namespace Common
{
    public class VertexAttribPointer
    {
        public int Index { get; set; }
        public int SizeInBytes { get; set; }
        public VertexAttribPointerType Type { get; set; }
        public bool Normalize { get; set; }
        public int StrideInBytes { get; set; }
        public int OffsetInBytes { get; set; }

        public void Enable()
        {
            GL.VertexAttribPointer(Index, SizeInBytes, Type, Normalize, StrideInBytes, OffsetInBytes);
            GL.EnableVertexAttribArray(Index);
        }
    }
}