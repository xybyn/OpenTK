using GlmNet;

namespace Common
{
    public abstract class Drawer3DBase
    {
        protected readonly Drawer3DSettingsBase settings;

        protected Drawer3DBase(Drawer3DSettingsBase settings)
        {
            this.settings = settings;
        }

        public abstract void Draw(ref mat4 view, ref mat4 projection);
    }
}