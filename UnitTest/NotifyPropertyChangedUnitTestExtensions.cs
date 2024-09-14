using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTest
{
    /// <summary>
    /// <see cref="INotifyPropertyChanged.PropertyChanged"/>イベントの発火をテストするための拡張メソッドを定義したクラス
    /// </summary>
    internal static class NotifyPropertyChangedUnitTestExtensions
    {
        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/>イベントの発火の有無を検査する
        /// </summary>
        /// <param name="source">イベントの発火元となる<see cref="INotifyPropertyChanged"/></param>
        /// <param name="pred"><see cref="PropertyChangedEventArgs"/>に対する述語</param>
        /// <param name="timeoutMillseconds">タイムアウト時間(ms)</param>
        /// <returns><see cref="Task{bool}"/></returns>
        public static Task<bool> HasProprtyChangedAsync(this INotifyPropertyChanged source, Predicate<PropertyChangedEventArgs> pred = null, int timeoutMillseconds = 3000)
        {
            return Task.Run(() => {
                var called = false;

                PropertyChangedEventHandler handler = (sender, e) => {
                    if (!(pred is null || pred(e)))
                    {
                        return;
                    }

                    called = true;
                };

                source.PropertyChanged += handler;

                bool result = SpinWait.SpinUntil(() => called, timeoutMillseconds);

                source.PropertyChanged -= handler;

                return result;
            });
        }

        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/>イベントの発火の有無を検査する
        /// </summary>
        /// <param name="source">イベントの発火元となる<see cref="INotifyPropertyChanged"/></param>
        /// <param name="propertyName">検査するプロパティ名</param>
        /// <param name="timeoutMillseconds">タイムアウト時間(ms)</param>
        /// <returns><see cref="Task{bool}"/></returns>
        public static Task<bool> HasProprtyChangedAsync(this INotifyPropertyChanged source, string propertyName = null, int timeoutMillseconds = 3000)
        {
            return HasProprtyChangedAsync(source, (e) => e.PropertyName == (propertyName ?? e.PropertyName), timeoutMillseconds);
        }
    }
}