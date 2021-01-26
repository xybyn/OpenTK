using OpenTK.Graphics.OpenGL4;

namespace Common
{
    public abstract class Drawer3DSettingsBase : Drawer2DSettingsBase
    {
        public float MinZ { get; set; }
        public float MaxZ { get; set; }
        public MaterialFace MaterialFace { get; set; }
        public PolygonMode PolygonMode { get; set; }
    }
}