using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using WpfApp;

namespace UnitTest
{
    /// <summary>
    /// <see cref="MainWindowViewModel"/>の単体テスト
    /// </summary>
    [TestFixture]
    public class MainWindowViewModelTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// ウィンドウタイトルが正しく設定されていること
        /// </summary>
        [TestCase()]
        public void HasValidWindowTitle()
        {
            var vm = new MainWindowViewModel();

            Assert.That(vm.Title, Is.EqualTo(@"Test Application"));
        }

        /// <summary>
        /// <see cref="MainWindowViewModel.Message"/>を変更した最に<see cref="MainWindowViewModel.PropertyChanged"/>が発火すること
        /// </summary>
        [TestCase()]
        public void NotifyPropertyChangeOnMessageChanged()
        {
            var called = false;

            var vm = new MainWindowViewModel();
            vm.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName != nameof(MainWindowViewModel.Message))
                {
                    return;
                }

                called = true;
            };

            vm.Message = @"This is a new value";

            Assert.ThatAsync(async () => await Task.Run(() => SpinWait.SpinUntil(() => called, 3000)), Is.EqualTo(true));
            Assert.That(vm.Message, Is.EqualTo(@"This is a new value"));
        }
    }
}