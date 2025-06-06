// https://qiita.com/matarillo/items/9958b90ab04de5c2234e

using System;
using System.Threading;
using System.Threading.Tasks;

namespace OmoSeitoku.Threadings
{
    public sealed class Throttle
    {
        private readonly TimeSpan _timeSpan;
        private readonly Delegate _action;
        private long _signalSequence;

        /// <summary>
        /// 一定時間の間から最後のみ実行。Signal毎にTaskを作成。
        /// </summary>
        public Throttle(TimeSpan timeSpan, Delegate action)
        {
            _timeSpan = timeSpan;
            _action = action;
        }

        public async void Signal(params object[] args)
        {
            var id = Interlocked.Increment(ref _signalSequence);

            await Task.Delay(_timeSpan);

            var current = Interlocked.Read(ref _signalSequence);

            if (current == id)
            {
                _action.DynamicInvoke(args);
            }
        }
    }

    public sealed class Throttle<T>
    {
        private readonly TimeSpan _timeSpan;
        private readonly Action<T> _action;
        private long _signalSequence;

        /// <summary>
        /// 一定時間の間から最後のみ実行。Signal毎にTaskを作成。
        /// </summary>
        public Throttle(TimeSpan timeSpan, Action<T> action)
        {
            _timeSpan = timeSpan;
            _action = action;
        }

        public async void Signal(T input)
        {
            var id = Interlocked.Increment(ref _signalSequence);

            await Task.Delay(_timeSpan);

            var current = Interlocked.Read(ref _signalSequence);

            if (current == id)
            {
                _action(input);
            }
        }
    }

    public sealed class Throttle<T1, T2>
    {
        private readonly TimeSpan _timeSpan;
        private readonly Action<T1, T2> _action;
        private long _signalSequence;

        /// <summary>
        /// 一定時間の間から最後のみ実行。Signal毎にTaskを作成。
        /// </summary>
        public Throttle(TimeSpan timeSpan, Action<T1, T2> action)
        {
            _timeSpan = timeSpan;
            _action = action;
        }

        public async void Signal(T1 input1, T2 input2)
        {
            var id = Interlocked.Increment(ref _signalSequence);

            await Task.Delay(_timeSpan);

            var current = Interlocked.Read(ref _signalSequence);

            if (current == id)
            {
                _action(input1, input2);
            }
        }
    }

    public sealed class Throttle<T1, T2, T3>
    {
        private readonly TimeSpan _timeSpan;
        private readonly Action<T1, T2, T3> _action;
        private long _signalSequence;

        /// <summary>
        /// 一定時間の間から最後のみ実行。Signal毎にTaskを作成。
        /// </summary>
        public Throttle(TimeSpan timeSpan, Action<T1, T2, T3> action)
        {
            _timeSpan = timeSpan;
            _action = action;
        }

        public async void Signal(T1 input1, T2 input2, T3 input3)
        {
            var id = Interlocked.Increment(ref _signalSequence);

            await Task.Delay(_timeSpan);

            var current = Interlocked.Read(ref _signalSequence);

            if (current == id)
            {
                _action(input1, input2, input3);
            }
        }
    }

    public sealed class Throttle<T1, T2, T3, T4>
    {
        private readonly TimeSpan _timeSpan;
        private readonly Action<T1, T2, T3, T4> _action;
        private long _signalSequence;

        /// <summary>
        /// 一定時間の間から最後のみ実行。Signal毎にTaskを作成。
        /// </summary>
        public Throttle(TimeSpan timeSpan, Action<T1, T2, T3, T4> action)
        {
            _timeSpan = timeSpan;
            _action = action;
        }

        public async void Signal(T1 input1, T2 input2, T3 input3, T4 input4)
        {
            var id = Interlocked.Increment(ref _signalSequence);

            await Task.Delay(_timeSpan);

            var current = Interlocked.Read(ref _signalSequence);

            if (current == id)
            {
                _action(input1, input2, input3, input4);
            }
        }
    }

    public sealed class OneTaskThrottle<T> : IDisposable
    {
        private readonly AutoResetEvent _producer = new AutoResetEvent(true);
        private readonly AutoResetEvent _consumer = new AutoResetEvent(false);
        private Tuple<T, SynchronizationContext> _value;

        private readonly TimeSpan _timeSpan;
        private readonly Action<T> _action;
        private volatile bool _disposed = false;
        private int _producers = 0;

        /// <summary>
        /// 一定時間の間から最後のみ実行。Taskはひとつ。
        /// </summary>
        public OneTaskThrottle(TimeSpan timeSpan, Action<T> action)
        {
            _timeSpan = timeSpan;
            _action = action;
            Task.Run(() => WaitAndFire());
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns>タイムアウトの場合 true</returns>
        //public bool WaitOne(TimeSpan timeSpan)
        //{
        //    var timedOut = !_producer.WaitOne(timeSpan);
        //    return timedOut;
        //}

        private void WaitAndFire()
        {
            try
            {
                while (true)
                {
                    // シグナルを設定して、待機しているスレッドを進行させる
                    _producer.Set();
                    // シグナルを受け取るまでブロック
                    _consumer.WaitOne();

                    if (_disposed) return;

                    while (true)
                    {
                        _producer.Set();
                        var timedOut = !_consumer.WaitOne(_timeSpan);
                        if (_disposed) return;
                        if (timedOut) break;
                    }
                    Fire(_action, _value.Item1, _value.Item2);
                    _value = null;
                }
            }
            catch (ObjectDisposedException)
            {
                // ignore and exit
            }
        }

        private static void Fire(Action<T> action, T input, SynchronizationContext context)
        {
            if (context != null)
            {
                context.Post(state => action((T)state), input);
            }
            else
            {
                Task.Run(() => action(input));
            }
        }

        public void Signal(T input)
        {
            if (_disposed) return;

            try
            {
                Interlocked.Increment(ref _producers);
                _producer.WaitOne();
                if (_disposed) return;
                _value = Tuple.Create(input, SynchronizationContext.Current);
                _consumer.Set();
            }
            catch (ObjectDisposedException)
            {
                // ignore and exit
            }
            finally
            {
                Interlocked.Decrement(ref _producers);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                _disposed = true;
                // 残っている _producer を処理。
                while (0 < Interlocked.CompareExchange(ref _producers, value: 0, comparand: 0))
                {
                    _producer.Set();
                }
                _consumer.Set();
                _producer.Dispose();
                _consumer.Dispose();
            }
            catch (Exception) { }
        }
    }
}
