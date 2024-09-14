using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(@"UnitTest")]

namespace WpfApp.Properties
{
    internal class InternationalizationService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName =  null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        /// <summary>
        /// シングルトンインスタンス
        /// </summary>
        public static InternationalizationService Current { get; private set; }

        /// <summary>
        /// 現在のリソース
        /// </summary>
        public Resources Resources { get; private set; }

        /// <summary>
        /// ユーザーインターフェースに関する<see cref="CultureInfo"/>
        /// </summary>
        public CultureInfo UICultureInfo
        {
            get => CultureInfo.CurrentUICulture;
            set
            {
                CultureInfo.CurrentUICulture = value;

                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// <see cref="UICultureInfo"/>が変化した際のイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private static void OnUICultureInfoChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is InternationalizationService service))
            {
                return;
            }

            if (e.PropertyName != nameof(UICultureInfo))
            {
                return;
            }

            Resources.Culture = CultureInfo.CurrentUICulture;
            service.RaisePropertyChanged(nameof(Resources));
        }

        #region Constructors

        static InternationalizationService()
        {
            Current = new InternationalizationService();

            Current.PropertyChanged += OnUICultureInfoChanged;
        }

        internal InternationalizationService()
        {
            Resources = new Resources();
        }

        #endregion
    }
}