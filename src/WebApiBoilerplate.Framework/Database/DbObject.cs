using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Event;

namespace WebApiBoilerplate.Framework.Database
{
    public abstract class DbObject
    {
        public virtual long Id { get; protected set; }

        public ISession Session { get; protected internal set; }

        [Serializable]
        internal class SetSessionToObject : IPostLoadEventListener, IPersistEventListener, ILoadEventListener
        {
            public void OnPostLoad(PostLoadEvent @event)
            {
                if (@event.Entity is DbObject dbObject)
                {
                    dbObject.Session = @event.Session;
                }
            }

            public Task OnPersistAsync(PersistEvent @event, CancellationToken cancellationToken)
            {
                OnPersist(@event);
                return Task.CompletedTask;
            }

            public Task OnPersistAsync(PersistEvent @event, IDictionary createdAlready, CancellationToken cancellationToken)
            {
                OnPersist(@event, createdAlready);
                return Task.CompletedTask;
            }

            public void OnPersist(PersistEvent @event)
            {
                if (@event.Entity is DbObject dbObject)
                {
                    dbObject.Session = @event.Session;
                }
            }

            public void OnPersist(PersistEvent @event, IDictionary createdAlready)
            {
                if (@event.Entity is DbObject dbObject)
                {
                    dbObject.Session = @event.Session;
                }
            }

            public Task OnLoadAsync(LoadEvent @event, LoadType loadType, CancellationToken cancellationToken)
            {
                OnLoad(@event, loadType);
                return Task.CompletedTask;
            }

            public void OnLoad(LoadEvent @event, LoadType loadType)
            {
                if (@event.Result is DbObject dbObject)
                {
                    dbObject.Session = @event.Session;
                }
            }
        }
    }
}
