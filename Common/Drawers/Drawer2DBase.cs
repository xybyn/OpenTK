namespace Common
{
    public abstract class Drawer2DBase
    {
        protected readonly Drawer2DSettingsBase settings;

        protected Drawer2DBase(Drawer2DSettingsBase settings)
        {
            this.settings = settings;
        }

        public abstract void Draw();

        public abstract void Initialize();
    }
}