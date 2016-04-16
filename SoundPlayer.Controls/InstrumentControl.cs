using Newtonsoft.Json;
using SoundPlayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        protected Dictionary<string, Func<object>> _items = new Dictionary<string, Func<object>>();
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _items.Add("instrument", () => InstrumentName);
            _items.Add("notes", () => _instrument.SelectedNote);
            _items.Add("octaves", () => _instrument.SelectedOctave);
            _items.Add("tempos", () => _instrument.SelectedTempo);
            _items.Add("intensities", () => _instrument.SelectedIntensity);
            _instrument.SetPoints("instrument", new List<Point>());

            foreach (var key in _items.Keys)
            {
                var ellipse = GetTemplateChild(key) as Ellipse;
                if (null != ellipse)
                {
                    ellipse.TouchMove += Ellipse_TouchMove;
                    ellipse.TouchUp += Ellipse_TouchUp;
                }
                var text = GetTemplateChild("txt" + key) as TextBlock;
                if (null != text)
                {
                    text.Text = _items[key].Invoke().ToString();
                }
            }
        }

        private void Ellipse_TouchUp(object sender, TouchEventArgs e)
        {
            if (_instrument != null && _instrument.Songs.Any())
            {
                var itemName = (sender as Ellipse).Name;
                if (!itemName.Equals("instrument"))
                {
                    switch (_instrument.GetMovement(itemName))
                    {
                        case eMovement.Right:
                        case eMovement.Up:
                            _instrument.SetNext(itemName);
                            break;
                        case eMovement.Left:
                        case eMovement.Down:
                            _instrument.SetPrev(itemName);
                            break;
                    }

                    (GetTemplateChild("txt" + itemName) as TextBlock).Text = _items[itemName].Invoke().ToString();

                    (GetTemplateChild("itemDiagnostics") as ItemsControl).ItemsSource = _instrument.GetPoints(itemName);
                }

                var song = _instrument.Songs.FirstOrDefault(x => x.Note == _instrument.SelectedNote
                    && x.Octave == _instrument.SelectedOctave
                    && x.Tempo == _instrument.SelectedTempo
                    && x.Intensity == _instrument.SelectedIntensity);
                if (song != null)
                {
                    for(var i=0;i<Math.Max(1,_instrument.GetPoints(itemName).Count());i++)
                        song.Play();
                }
            }
        }

        private void Ellipse_TouchMove(object sender, TouchEventArgs e)
        {
            if((sender as Ellipse).Name != "instrument")
               _instrument.SetPoints((sender as Ellipse).Name
                    , e.GetIntermediateTouchPoints(GetTemplateChild("itemDiagnostics") as ItemsControl)
                        .Select(x => x.Position));
        }

    }
}
