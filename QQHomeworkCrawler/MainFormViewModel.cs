using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Win32;
using MoreLinq;
using Newtonsoft.Json;
using Prism.Commands;
using QQHomeworkCrawler.Annotations;

namespace QQHomeworkCrawler
{
    public class MainFormViewModel : INotifyPropertyChanged
    {
        private string _jsonString;

        public string JsonString
        {
            get => _jsonString;
            set
            {
                _jsonString = value;
                OnPropertyChanged(nameof(JsonString));
            }
        }

        public ICommand GetImageUrlCommand { get; }
        public ICommand OpenFile { get; }
        public ICommand FindJpgCommand { get; }
        public ICommand GetImageCommand { get; }
        
        public MainFormViewModel()
        {
            OpenFile = new DelegateCommand<string>(OpenJsonFile);
            GetImageUrlCommand = new DelegateCommand<string>(GetImageUrls);
            FindJpgCommand = new DelegateCommand<string>(FindJpg);
            GetImageCommand = new DelegateCommand<string>(GetImage);
        }

        private void GetImage(string obj)
        {
            var urlList = JsonString.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            using (var webClient = new WebClient())
            {
                JsonString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ForEach((url,i) =>
                    {
                        var data = webClient.DownloadData(url);
                        using (var memStream = new MemoryStream(data))
                        {
                            var image = Image.FromStream(memStream);
                            image.Save($"Images/{i}.jpg", ImageFormat.Jpeg);
                        }

                        i++;
                    });
            }
        }

        private void FindJpg(string obj)
        {
            var stringBuilder = new StringBuilder();
            JsonString.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Where(line => line.ToLowerInvariant().Contains("jpg"))
                .Select(line => line.Split(new[] {"\""}, StringSplitOptions.RemoveEmptyEntries)[3])
                .ForEach(line => stringBuilder.AppendLine(line));

            JsonString = stringBuilder.ToString();
        }

        private void OpenJsonFile(string obj)
        {
            var result = new OpenFileDialog().ShowDialog();
        }

        private static void GetImageUrls(string json)
        {
            var result = JsonConvert.DeserializeObject(json);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}