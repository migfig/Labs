using Newtonsoft.Json;
using SoundPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoundPlayer.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SoundPlayer.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SoundPlayer.Controls;assembly=SoundPlayer.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class InstrumentControl : Control
    {
        public static readonly DependencyProperty instrumentProp =
            DependencyProperty.Register("Instrument", typeof(Instrument), typeof(InstrumentControl));

        public static readonly DependencyProperty instrumentNameProp =
            DependencyProperty.Register("Name", typeof(string), typeof(InstrumentControl));

        static InstrumentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InstrumentControl), new FrameworkPropertyMetadata(typeof(InstrumentControl)));
        }

        protected Instrument _instrument
        {
            get { return (Instrument)GetValue(instrumentProp); }
        }

        public string InstrumentName
        {
            get { return (string)GetValue(instrumentNameProp); }
            set
            {
                SetValue(instrumentNameProp, value);
                var instrument = _instrument;
                if (null == instrument && !string.IsNullOrEmpty(value))
                {                   
                    var fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value + ".json");
                    if (File.Exists(fileName))
                    {
                        using (var stream = File.OpenText(fileName))
                        {
                            instrument = JsonConvert.DeserializeObject<Instrument>(stream.ReadToEnd());
                        }
                        SetValue(instrumentProp, instrument);
                    }
                }
            }
        }

        protected Ellipse _outerEllipse;
        protected Ellipse _innerEllipse;
        protected TextBlock _text;
        protected ItemsControl _itemsControl;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            _outerEllipse = GetTemplateChild("elpOuter") as Ellipse;
            _innerEllipse = GetTemplateChild("elpInner") as Ellipse;
            _text = GetTemplateChild("tbInstrument") as TextBlock;
            if (null != _text)
            {
                _text.Text = string.Format("{0}:{1},{2}", InstrumentName, _instrument.SelectedNote, _instrument.SelectedOctave);
            }
            _itemsControl = GetTemplateChild("itemDiagnostics") as ItemsControl;
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            base.OnTouchUp(e);

            if(_instrument != null && _instrument.Songs.Any())
            {
                switch(_instrument.Movement)
                {
                    case eMovement.Right:
                        _instrument.SetNext(Instrument.kNotes);
                        break;
                    case eMovement.Up:
                        _instrument.SetNext(Instrument.kOctaves);
                        break;
                    case eMovement.Left:
                        _instrument.SetPrev(Instrument.kNotes);
                        break;
                    case eMovement.Down:
                        _instrument.SetPrev(Instrument.kOctaves);
                        break;
                }

                _text.Text = string.Format("{0}:{1},{2}", InstrumentName, _instrument.SelectedNote, _instrument.SelectedOctave);
                _itemsControl.ItemsSource = _instrument.Points;
                var song = _instrument.Songs.FirstOrDefault(x => x.Note == _instrument.SelectedNote
                    && x.Octave == _instrument.SelectedOctave);
                if(song != null)
                {
                    song.Play();
                }
            }
        }

        protected override void OnTouchMove(TouchEventArgs e)
        {
            base.OnTouchMove(e);

            var points = e.GetIntermediateTouchPoints(_itemsControl);
            var positions = points.Select(x => x.Position);
            _instrument.Points = positions;
        }
    }
}
