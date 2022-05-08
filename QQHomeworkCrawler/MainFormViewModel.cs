using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
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
        private ObservableCollection<HomeworkViewModel> _homeworkEntities;

        public string JsonString
        {
            get => _jsonString;
            set
            {
                _jsonString = value;
                OnPropertyChanged(nameof(JsonString));
            }
        }

        public ObservableCollection<HomeworkViewModel> HomeworkEntities
        {
            get => _homeworkEntities;
            set
            {
                _homeworkEntities = value;
                OnPropertyChanged(nameof(HomeworkEntities));
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
            GetImageCommand = new DelegateCommand(GetHomeworkContentText);
        }

        private void GetHomeworkContentText()
        {
            Parallel.ForEach(
                HomeworkEntities.Select(entity => (entity.StudentName, entity.ImageUrl)),
                item => DownloadImage(item.StudentName, item.ImageUrl));
        }

        private static void DownloadImage(string studentName, IEnumerable<string> imageUrls)
        {
            using (var webClient = new WebClient())
            {
                imageUrls.ForEach((url, i) =>
                {
                    var data = webClient.DownloadData(url);
                    using (var memStream = new MemoryStream(data))
                    {
                        var image = Image.FromStream(memStream);
                        if (!Directory.Exists("Images/"))
                            Directory.CreateDirectory("Images/");

                        image.Save($"Images/{studentName}{(i == 0 ? string.Empty : i.ToString())}.jpg",
                            ImageFormat.Jpeg);
                    }
                });
            }
        }

        private void GetImage(string obj)
        {
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
            var openFileDialog = new OpenFileDialog {Filter = "Json File | *.json"};
            var result = openFileDialog.ShowDialog();
            if (!result.HasValue || !result.Value)
                return;

            var fileName = openFileDialog.FileName;
            using (var reader = new StreamReader(fileName, Encoding.UTF8))
                JsonString = reader.ReadToEnd();
        }

        private void GetImageUrls(string json)
        {
            HomeworkEntities = new ObservableCollection<HomeworkViewModel>(
                JsonConvert.DeserializeObject<HomeworkWrapper>(json)
                    .data
                    .Feedback
                    .Select(item => new HomeworkViewModel(
                        item.nick,
                        item.content.main.SelectMany(content => content.text.c.Where(i => i.type.Equals("img")).Select(i => i.url)))
                    ));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}