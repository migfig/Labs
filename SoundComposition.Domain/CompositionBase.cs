using Windows.UI.Composition;

namespace SoundComposition.Domain
{
    public abstract class CompositionBase
    {
        protected Visual _element;
        public abstract Visual ComposeElement(Compositor compositor);
    }
}
