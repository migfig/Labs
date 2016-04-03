using System.Text.RegularExpressions;

namespace SoundPlayer.Models
{
    public partial class Song
    {
        public string FileName { get; set; }

        public string Instrument { get; set; }
        public string Note { get; set; }
        public int Octave { get; set; }
        public float Tempo { get; set; }
        public string Intensity { get; set; }
        public string Mode { get; set; }

        public Song(string fileName)
            :this(fileName.Split('_'))
        {
            FileName = fileName;
        }

        private Song(string[] parts)
        {
            Instrument = parts[0];
            var re = new Regex(@"(?<note>[a-zA-Z]*)(?<octave>\d{1,3})");
            var match = re.Match(parts[1]);
            if (match.Success)
            {
                Note = match.Groups["note"].Value;
                Octave = int.Parse(match.Groups["octave"].Value);
            }

            re = new Regex(@"(?<tempo>\d{1,3})");
            match = re.Match(parts[2]);
            if (match.Success)
            {
                Tempo = float.Parse(match.Groups["tempo"].Value);
                if (Tempo > 1 && Tempo < 16)
                    Tempo = Tempo * 0.1F;
                else if (Tempo == 25.0F)
                    Tempo = 0.25F;
            }
            else {
                Tempo = 1.0F;
            }
            Intensity = parts[3];
            Mode = parts[4];
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
