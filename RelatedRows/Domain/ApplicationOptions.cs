using DynamicData.Binding;
using MahApps.Metro.Controls;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RelatedRows.Helpers;
using Serilog.Events;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace RelatedRows.Domain
{
    public class  ApplicationOptions: AbstractNotifyPropertyChanged
    {
        public Theme Theme { get; set; }
        private bool _useDarkTheme;
        [JsonIgnore]
        public bool UseDarkTheme
        {
            get { return _useDarkTheme; }
            set
            {
                SetAndRaise(ref _useDarkTheme, value);
            }
        }

        private int _scale;
        public int Scale {
            get { return _scale; }
            set
            {
                SetAndRaise(ref _scale, value);
                OnPropertyChanged("ScaleRatio");
            }
        }

        public decimal ScaleRatio
        {
            get { return (decimal)Scale / (decimal)100; }
        }

        private long _rowsPerPage;
        public long RowsPerPage {
            get { return _rowsPerPage; }
            set
            {
                SetAndRaise(ref _rowsPerPage, value);
            }
        }

        private string _primaryColor;
        public string PrimaryColor {
            get { return _primaryColor; }
            set {
                SetAndRaise(ref _primaryColor, value);
                OnPropertyChanged("SelectedPrimaryColor");
            }
        }
        private string _accentColor;
        public string AccentColor {
            get { return _accentColor; }
            set {
                SetAndRaise(ref _accentColor, value);
                OnPropertyChanged("SelectedAccentColor");
            }
        }
        public bool OpenRecentOnStartup { get; set; }

        private LogEventLevel _logLevel = LogEventLevel.Error;
        public LogEventLevel LogLevel
        {
            get { return _logLevel; }
            set
            {
                SetAndRaise(ref _logLevel, value);
            }
        }

        [JsonIgnore]
        public PaletteSelectorViewModel PaletteSelector { get; private set; }

        [JsonIgnore]
        public Swatch SelectedPrimaryColor
        {
            get { return PaletteSelector.Swatches.FirstOrDefault(s => s.Name.Equals(PrimaryColor)); }
            set { PrimaryColor = value.Name; }
        }

        [JsonIgnore]
        public Swatch SelectedAccentColor
        {
            get { return PaletteSelector.Swatches.FirstOrDefault(s => s.Name.Equals(AccentColor)); }
            set { AccentColor = value.Name; }
        }

        public ApplicationOptions(Theme theme, int scale, long rowsPerPage, string primaryColor, string accentColor, bool openRecentOnStartup)
        {
            PaletteSelector = new PaletteSelectorViewModel();
            Theme = theme;
            UseDarkTheme = Theme.Equals(Theme.Dark);
            Scale = scale;
            RowsPerPage = rowsPerPage;
            PrimaryColor = primaryColor;
            AccentColor = accentColor;
            OpenRecentOnStartup = openRecentOnStartup;
        }

        private static string OptionsFile { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "options.json"); } }

        private ICommand _applyCommand;
        [JsonIgnore]
        public ICommand ApplyCommand
        {
            get {
                return _applyCommand ?? (_applyCommand = new Command(() => {
                    File.WriteAllText(OptionsFile, JsonConvert.SerializeObject(this));
                    OnPropertyChanged("AlternatingRowBackground");
                }));
            }
        }

        public void ApplyTheme()
        {
            PaletteSelector.ApplyPrimaryCommand.Execute(SelectedPrimaryColor);
            PaletteSelector.ApplyAccentCommand.Execute(SelectedAccentColor);
        }

        private ICommand _applyTheme;
        [JsonIgnore]
        public ICommand ApplyThemeCommand
        {
            get
            {
                return _applyTheme ?? (_applyTheme = new Command<bool>((isDark) => {
                    new PaletteHelper().SetLightDark(isDark);
                    Theme = isDark ? Theme.Dark : Theme.Light;
                }));
            }
        }

        public static ApplicationOptions Get()
        {
            var optionsFile = OptionsFile;
            if(!File.Exists(optionsFile))
            {
                var options = new ApplicationOptions(Theme.Light, 100, 20, "teal", "teal", true);
                File.WriteAllText(optionsFile, JsonConvert.SerializeObject(options));
                return options;
            }

            return JsonConvert.DeserializeObject<ApplicationOptions>(File.ReadAllText(optionsFile));
        }
    }
}
