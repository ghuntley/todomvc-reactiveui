using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using Todo.Shared.Business.Persistence;
using Todo.Shared.Presentation;

namespace Todo.Shared
{
    public sealed class Bootstrapper
    {
        public static ViewModel BuildViewModel()
        {
            var userInteface = RxApp.MainThreadScheduler;
            var background = CurrentThreadScheduler.Instance;

            var storageProvider = new StorageProvider();

            return new ViewModel(storageProvider, userInteface, background);
        }
    }
}
