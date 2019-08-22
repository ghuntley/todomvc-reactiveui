using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Todo.Shared.Business.Entities;
using Todo.Shared.Business.Persistence;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Todo.Shared.Presentation
{
    public class ViewModel : ReactiveObject, ISupportsActivation
    {
        public bool IsLoading => _isLoading.Value;
        private readonly ObservableAsPropertyHelper<bool> _isLoading;

        public bool IsReady => _isReady.Value;
        private readonly ObservableAsPropertyHelper<bool> _isReady;

        public ReadOnlyObservableCollection<TodoItem> Todos => _todos;
        private readonly ReadOnlyObservableCollection<TodoItem> _todos;

        private TodoItem _selectedItem;
        public TodoItem SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        private readonly ObservableAsPropertyHelper<int> _itemsLeft;
        public int ItemsLeft => _itemsLeft.Value;

        private Show _filter;
        public Show Filter
        {
            get => _filter;
            set => this.RaiseAndSetIfChanged(ref _filter, value);
        }


        private string _todo;
        public string Todo
        {
            get => _todo;
            set => this.RaiseAndSetIfChanged(ref _todo, value);
        }
        
        public ReactiveCommand<Unit, Unit> Refresh { get; }
        public ReactiveCommand<Unit, TodoItem> AddTodo { get; }
        public ReactiveCommand<Unit, Unit> ShowAll { get; }
        public ReactiveCommand<Unit, Unit> ShowActive { get; }
        public ReactiveCommand<Unit, Unit> ShowCompleted { get; }
        public ReactiveCommand<Unit, Unit> ClearCompleted { get; }

        public ViewModel(IStorageProvider storageProvider, IScheduler userInterface, IScheduler background)
        {
            Refresh = ReactiveCommand.CreateFromTask(storageProvider.Refresh, outputScheduler: background);

            _isLoading = Refresh
                .IsExecuting
                .ToProperty(this, x => x.IsLoading, scheduler: userInterface);

            _isReady = Refresh
                .IsExecuting
                .Skip(1)
                .Select(executing => !executing)
                .ToProperty(this, x => x.IsReady, scheduler: userInterface);

            var todos = storageProvider.Read();
            todos.StartWithEmpty()
                .ObserveOn(userInterface)
                .OnItemAdded(x => {
                    Todo = null;
                    SelectedItem = null;
                })
                .OnItemRemoved(x => {
                    SelectedItem = null;
                })
                .Bind(out _todos)
                .Subscribe();

            //_itemsLeft = Todos. 
            //    .ObserveOn(background)
            //    .Log(this, $"Items left")
            //    .ToProperty(this, x => x.ItemsLeft, scheduler: userInterface);

            var canAddTodo = this.WhenAnyValue(vm => vm.Todo, (todo) => !string.IsNullOrWhiteSpace(todo));
            AddTodo = ReactiveCommand.Create(() => new TodoItem(), canAddTodo, outputScheduler: background);
            AddTodo.ThrownExceptions.Subscribe(exception => this.Log().Warn("Error!", exception));
            AddTodo.Subscribe(todoItem =>
            {
                storageProvider.Add(new TodoItem() { Description = Todo });
            });


            ShowAll = ReactiveCommand.Create(() => Unit.Default, outputScheduler: background);
            ShowAll.ThrownExceptions.Subscribe(exception => this.Log().Warn("Error!", exception));
            ShowAll.Subscribe(_ =>
            {
                Filter = Show.All;
            });

            ShowActive = ReactiveCommand.Create(() => Unit.Default, outputScheduler: background);
            ShowActive.ThrownExceptions.Subscribe(exception => this.Log().Warn("Error!", exception));
            ShowActive.Subscribe(_ =>
            {
                Filter = Show.Active;
            });

            ShowCompleted = ReactiveCommand.Create(() => Unit.Default, outputScheduler: background);
            ShowCompleted.ThrownExceptions.Subscribe(exception => this.Log().Warn("Error!", exception));
            ShowCompleted.Subscribe(_ =>
            {
                Filter = Show.Completed;
            });

            var canClearCompleted = this.WhenAnyValue(vm => vm.Todo, (todo) => !string.IsNullOrWhiteSpace(todo));
            ClearCompleted = ReactiveCommand.Create(() => Unit.Default, canClearCompleted, outputScheduler: background);
            ClearCompleted.ThrownExceptions.Subscribe(exception => this.Log().Warn("Error!", exception));
            ClearCompleted.Subscribe(todoItem =>
            {
                foreach (var todo in Todos)
                {
                    if (todo.IsComplete)
                    {
                        storageProvider.Remove(todo.Id);
                    }
                }
            });

            Activator = new ViewModelActivator();
            this.WhenActivated(disposables =>
            {
                Refresh.Execute()
                    .Subscribe()
                    .DisposeWith(disposables);
            });

        }

        public ViewModelActivator Activator { get; }
    }
}
