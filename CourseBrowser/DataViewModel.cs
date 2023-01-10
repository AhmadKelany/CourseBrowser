using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace CourseBrowser
{
    public class DataViewModel : INotifyPropertyChanged
    {
        private List<FileDuration> durations = new List<FileDuration>();
        public List<FileDuration> Durations
        {
            get => durations;
            set
            {
                durations = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalDuration));
                RaisePropertyChanged(nameof(TotalDurationString));
                RaisePropertyChanged(nameof(VideosCount));
                RaisePropertyChanged(nameof(DoneDuration));
                RaisePropertyChanged(nameof(DoneDurationString));
                RaisePropertyChanged(nameof(RemainingDurationString));
                RaisePropertyChanged(nameof(RemainingDuration));
                RaisePropertyChanged(nameof(DonePercent));
            }
        }
        public TimeSpan TotalDuration => TimeSpan.FromTicks(Durations.Sum(d => d.DurationSpan.Ticks));
        public TimeSpan DoneDuration => TimeSpan.FromTicks(Durations.Where(d => d.IsDone).Sum(d => d.DurationSpan.Ticks));
        public TimeSpan RemainingDuration => TotalDuration - DoneDuration;
        public double DonePercent => double.IsNaN( DoneDuration/TotalDuration) ? 0 : DoneDuration / TotalDuration;
        public string TotalDurationString => TotalDuration.ToHourMinuteString();
        public string DoneDurationString => DoneDuration.ToHourMinuteString();
        public string RemainingDurationString => RemainingDuration.ToHourMinuteString();
        public int VideosCount => Durations.Count;

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task GetData(string path)
        {
            Durations.Clear();
            var infos = new List<FileDuration>();
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).
                        Where(f => f.ToLower().EndsWith(".mp4") | f.ToLower().EndsWith(".avi")).ToList();

            foreach (var f in files)
            {
                infos.Add(new FileDuration
                {
                    IsDone = f.Contains("done") ,
                    FileName = Path.GetFileNameWithoutExtension(f),
                    DurationSpan = await GetVideoDuration(f)
                }); ;
            }
            Durations = infos;
        }

        private async Task<TimeSpan> GetVideoDuration(string filePath)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(filePath);
            VideoProperties p = await file.Properties.GetVideoPropertiesAsync();
            return p.Duration;
        }

    }
}
