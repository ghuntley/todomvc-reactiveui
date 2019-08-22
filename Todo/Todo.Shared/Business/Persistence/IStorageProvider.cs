using DynamicData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Todo.Shared.Business.Entities;

namespace Todo.Shared.Business.Persistence
{
    public interface IStorageProvider
    {
        IObservable<IChangeSet<TodoItem, Guid>> Read();
        Task Add(TodoItem todoItem);
        Task Remove(Guid todoId);
        Task Refresh();
    }
}
