using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;

namespace SoundComposition.Domain
{
    public partial class Instrument
    {
        public async Task LoadSongs()
        {
            if (null != _songs) return;

            if (null != _folder)
            {
                var items = await _folder.GetItemsAsync();
                _songs = new ObservableCollection<Song>(
                    from i in items
                    select new Song(i.Path)                        
                );
            }

            return;
        }

        public override Visual ComposeElement(Compositor compositor)
        {
            var random = new Random();
            //
            // Each _element consists of three visuals, which produce the appearance
            // of a framed rectangle
            //
            var _element = compositor.CreateContainerVisual();
            _element.Size = new Vector2(100.0f, 100.0f);
            //
            // Position this visual randomly within our window
            //
            _element.Offset = new Vector3((float)(random.NextDouble() * 400), (float)(random.NextDouble() * 400), 0.0f);
            //_element.Children.InsertAtTop(ElementCompositionPreview.GetElementVisual(new TextBlock { Text = "C#" }));

            //
            // The outer rectangle is always white
            //
            var visual = compositor.CreateSpriteVisual();
            _element.Children.InsertAtTop(visual);
            visual.Brush = compositor.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            visual.Size = new Vector2(100.0f, 100.0f);

            //
            // The inner rectangle is inset from the outer by three pixels all around
            //
            var child = compositor.CreateSpriteVisual();
            visual.Children.InsertAtTop(child);
            child.Offset = new Vector3(3.0f, 3.0f, 0.0f);
            child.Size = new Vector2(94.0f, 94.0f);

            //
            // Pick a random color for every rectangle
            //
            child.Brush = compositor.CreateColorBrush(FromInstrument(Name));            

            //
            // Make the subtree root visual partially transparent. This will cause each visual in the subtree
            // to render partially transparent, since a visual's opacity is multiplied with its parent's
            // opacity
            //
            _element.Opacity = 0.8f;
            return _element;
        }

        public static Color FromInstrument(string Name)
        {
            var dict = new Dictionary<string, Color>{

                { "cello", Color.FromArgb(0xFF, 100, 150, 200) },
                { "flute", Color.FromArgb(0xFF, 130, 150 ,200) },
                { "guitar",Color.FromArgb(0xFF, 160, 150 ,200) },
                { "oboe",  Color.FromArgb(0xFF, 190, 150 ,200) },
                { "violin",Color.FromArgb(0xFF, 220, 150 ,200) },
            };

            return dict[Name];
        }

        public static string FromElement(ContainerVisual element)
        {
            var brush = ((SpriteVisual)((SpriteVisual)element.Children.First()).Children.First()).Brush as CompositionColorBrush;
            var dict = new Dictionary<Color, string>{

                { Color.FromArgb(0xFF, 100, 150, 200), "cello" },
                { Color.FromArgb(0xFF, 130, 150 ,200), "flute" },
                { Color.FromArgb(0xFF, 160, 150 ,200), "guitar"},
                { Color.FromArgb(0xFF, 190, 150 ,200), "oboe" },
                { Color.FromArgb(0xFF, 220, 150 ,200), "violin" },
            };

            return dict[brush.Color];
        }
    }
}
