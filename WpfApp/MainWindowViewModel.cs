using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using WpfApp.Properties;

namespace WpfApp
{
    /// <summary>
    /// <see cref="MainWindow"/>のViewModel
    /// </summary>
    internal sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// <see cref="PropertyChanged"/>イベントを発火する
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Title

        private string title = @"Test Application";

        /// <summary>
        /// ウィンドウタイトル
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        #endregion

        #region Message

        private string message = Resources.InitialMessage;

        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        #endregion

        #region Languages

        private static readonly Dictionary<string, string> languages = new Dictionary<string, string>();

        private readonly ObservableCollection<KeyValuePair<string, string>> languageCollection;

        /// <summary>
        /// 言語コレクション
        /// </summary>
        public ObservableCollection<KeyValuePair<string, string>> LanguageCollection
        {
            get => languageCollection;
        }

        #endregion

        #region SelectedLanguage

        private KeyValuePair<string, string> selectedLanguage;

        /// <summary>
        /// 選択された言語
        /// </summary>
        public KeyValuePair<string, string> SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                selectedLanguage = value;
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }

        #endregion

        private void OnSeletedLanguageChanged(string languageKey)
        {
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(languageKey);

            Message = Resources.InitialMessage;
        }

        static MainWindowViewModel()
        {
            languages.Add(@"ja-JP", @"日本語");
            languages.Add(@"en-US", @"English");
        }

        public MainWindowViewModel()
        {
            languageCollection = new ObservableCollection<KeyValuePair<string, string>>(languages.ToList());

            selectedLanguage = languages.First();

            this.PropertyChanged += (sender, e) => {
                if (e.PropertyName != nameof(SelectedLanguage))
                {
                    return;
                }

                OnSeletedLanguageChanged(SelectedLanguage.Key);
            };
        }
    }
}