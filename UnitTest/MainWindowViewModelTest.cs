using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WpfApp;
using WpfApp.Properties;

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

        /// <summary>
        /// <see cref="MainWindowViewModel.SelectedLanguage"/>を変更した最に<see cref="MainWindowViewModel.Message"/>が変化すること
        /// </summary>
        [TestCase()]
        public void MessageChangingOnSelectedLanguageChanged()
        {
            var vm = new MainWindowViewModel();

            {
                var task = vm.HasProprtyChangedAsync(nameof(MainWindowViewModel.Message));
                vm.SelectedLanguage = vm.LanguageCollection.First(lang => lang.Key == @"ja-JP");
                // イベントが発火済み
                Assert.That(task.Result, Is.True);

                // Messageが正しく変化している
                Assert.That(vm.Message, Is.EqualTo(@"こんにちは、世界。"));
            }

            {
                var task = vm.HasProprtyChangedAsync(nameof(MainWindowViewModel.Message));
                vm.SelectedLanguage = vm.LanguageCollection.First(lang => lang.Key == @"en-US");
                // イベントが発火済み
                Assert.That(task.Result, Is.True);

                // Messageが正しく変化している
                Assert.That(vm.Message, Is.EqualTo(@"Hello, world."));
            }
        }
    }
}