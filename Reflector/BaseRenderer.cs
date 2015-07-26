namespace Reflector
{
    public abstract class BaseRenderer
    {
        protected readonly bool _includeSystemObjects;
        protected readonly string _sourcePath;

        public BaseRenderer()
            :this("", true)
        {
        }

        public BaseRenderer(string sourcePath = "", bool includeSystemObjects = false)
        {
            _sourcePath = sourcePath;
            _includeSystemObjects = includeSystemObjects;
        }
    }
}
