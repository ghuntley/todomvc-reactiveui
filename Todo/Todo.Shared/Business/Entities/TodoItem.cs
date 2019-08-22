using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Todo.Shared.Business.Entities
{
    public class TodoItem : ReactiveObject
    {
        public Guid Id => Guid.NewGuid();

        private string _description;
        public string Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        private bool _isComplete;
        public bool IsComplete
        {
            get => _isComplete;
            set => this.RaiseAndSetIfChanged(ref _isComplete, value);
        }
    }
}