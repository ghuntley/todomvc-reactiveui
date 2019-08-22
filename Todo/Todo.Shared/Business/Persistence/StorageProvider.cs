using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Todo.Shared.Business.Entities;

namespace Todo.Shared.Business.Persistence
{
    public class StorageProvider : IStorageProvider
    {
        private readonly SourceCache<TodoItem, Guid> _connectable = new SourceCache<TodoItem, Guid>(x => x.Id);

        public Task Add(TodoItem todoItem) => Task.Run(() =>
        {
            _connectable.AddOrUpdate(todoItem);
        });

        public IObservable<IChangeSet<TodoItem, Guid>> Read() => _connectable.Connect();

        public Task Refresh() => Task.Run(() =>
        {
            _connectable.Clear();
            _connectable.AddOrUpdate(new TodoItem() { Description = "Item 1" });
            _connectable.AddOrUpdate(new TodoItem() { Description = "Item 2", IsComplete = true });
            _connectable.AddOrUpdate(new TodoItem() { Description = "Item 3" });
        });


        public Task Remove(Guid todoId) => Task.Run(() =>
        {
            _connectable.Remove(todoId);
        });
    }
}
