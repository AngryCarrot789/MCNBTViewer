using System;
using System.Threading.Tasks;

namespace MCNBTViewer.Core {
    /// <summary>
    /// A simple relay command, which does not take any parameters
    /// </summary>
    public class AsyncRelayCommand : BaseRelayCommand {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;

        private volatile bool isRunning;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute), "Execute callback cannot be null");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) {
            return base.CanExecute(parameter) && (this.canExecute == null || this.canExecute());
        }

        public override async void Execute(object parameter) {
            this.isRunning = true;
            try {
                this.RaiseCanExecuteChanged();
                await this.execute();
            }
            finally {
                this.isRunning = false;
                this.RaiseCanExecuteChanged();
            }
        }
    }

    public class AsyncRelayCommand<T> : BaseRelayCommand {
        private readonly Action<T> execute;

        private readonly Func<T, bool> canExecute;

        public bool ConvertParameter { get; set; }

        public AsyncRelayCommand(Action<T> execute, Func<T, bool> canExecute = null, bool convertParameter = false) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute), "Execute callback cannot be null");
            }

            this.execute = execute;
            this.canExecute = canExecute;
            this.ConvertParameter = convertParameter;
        }

        public override bool CanExecute(object parameter) {
            if (this.ConvertParameter) {
                parameter = GetConvertedParameter<T>(parameter);
            }

            return base.CanExecute(parameter) && (this.canExecute == null || (parameter == null || parameter is T) && this.canExecute((T) parameter));
        }

        public override void Execute(object parameter) {
            if (this.ConvertParameter) {
                parameter = GetConvertedParameter<T>(parameter);
            }

            if (parameter == null || parameter is T) {
                this.execute((T) parameter);
            }
            else {
                throw new InvalidCastException($"Parameter type ({parameter.GetType()}) cannot be used for the callback method (which requires type {typeof(T).Name})");
            }
        }
    }
}