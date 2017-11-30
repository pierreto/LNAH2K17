
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace InterfaceGraphique.Controls.WPF.Tutorial
{
    public class TutorialViewModel : ViewModelBase
    {
        private string titleTutorial = "";

        public Image[] MatchSlides { get; set; }
        public Image[] EditorSlides { get; set; }

        public Image[] currentSlides;
        public Image[] CurrentSlides
        {
            get => currentSlides;
            set
            {
                currentSlides = value;
                OnPropertyChanged();
            }
        }
        public string TitleTutorial
        {
            get => titleTutorial;
            set
            {
                titleTutorial = value;
                OnPropertyChanged();
            }
        }

        private string selectedItem;
        private int activeSlideIndex;

        public TutorialViewModel()
        {
            string[] filenames = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\media\\image\\tutorial\\match\\", "*.*",
                SearchOption.AllDirectories);

            MatchSlides = GetImages(filenames);

            filenames = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\media\\image\\tutorial\\editor\\", "*.*",
                SearchOption.AllDirectories);

            EditorSlides = GetImages(filenames);

            CurrentSlides = MatchSlides;

            ActiveSlideIndex = 0;
        }

        public int ActiveSlideIndex
        {
            get => activeSlideIndex;
            set
            {
                this.activeSlideIndex = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanShowNext));
                OnPropertyChanged(nameof(CanShowPrevious));
            }
        }
        private Image[] GetImages(string[] filenames)
        {
            Image[] images = new Image[filenames.Length];
            for (int i = 0; i < filenames.Length; i++)
            {
                images[i] = new Image() { Source = new BitmapImage(new Uri(filenames[i])) };
            }
            return images;
        }

        public async Task SwitchToMatchSlides()
        {
            TitleTutorial = "Tutoriel d'une partie";
            CurrentSlides = MatchSlides;
            ActiveSlideIndex = 0;
        }

        public async Task SwitchToEditorSlides()
        {
            TitleTutorial = "Tutoriel d'édition";
            CurrentSlides = EditorSlides;
            ActiveSlideIndex = 0;
        }


        public override void InitializeViewModel()
        {
        }

        private ICommand previousCommand;
        public ICommand PreviousCommand
        {
            get
            {
                return previousCommand ??
                       (previousCommand = new RelayCommandAsync(Previous, (o) => CanShowPrevious));
            }
        }

        private async Task Previous()
        {
            this.ActiveSlideIndex--;
        }

        public bool CanShowPrevious
        {
            get => (this.activeSlideIndex - 1) >= 0;
        }

        private ICommand nextCommand;
        public ICommand NextCommand
        {
            get
            {
                return nextCommand ??
                       (nextCommand = new RelayCommandAsync(Next, (o) => CanShowNext));
            }
        }

        public bool CanShowNext
        {
            get => (this.activeSlideIndex + 1) < CurrentSlides.Length;
        }

        private ICommand editorSlideCommand;
        public ICommand EditorSlideCommand
        {
            get
            {
                return editorSlideCommand ??
                       (editorSlideCommand = new RelayCommandAsync(SwitchToEditorSlides, (o) => this.CurrentSlides!= this.EditorSlides));
            }
        }

        private ICommand matchSlideCommand;
        public ICommand MatchSlideCommand
        {
            get
            {
                return matchSlideCommand ??
                       (matchSlideCommand = new RelayCommandAsync(SwitchToMatchSlides, (o) => this.CurrentSlides != this.MatchSlides));
            }
        }

        private async Task Next()
        {
            this.ActiveSlideIndex++;
        }
    }
}
