namespace System {
    internal class Timers {
        internal class Timer {
            public Action<object, ElapsedEventArgs> Elapsed { get; internal set; }
            public bool AutoReset { get; internal set; }
        }
    }
}