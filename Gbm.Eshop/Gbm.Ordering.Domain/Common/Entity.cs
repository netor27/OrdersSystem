using MediatR;
using System;
using System.Collections.Generic;
using System.Text;


namespace Gbm.Ordering.Domain.Common
{
    public abstract class Entity
    {
        int? requestedHashCode;
        int id;
        private List<IAsyncNotification> domainEvents;


        public virtual int Id
        {
            get { return id; }
            protected set { id = value; }
        }

        public List<IAsyncNotification> DomainEvents => domainEvents;

        public void AddDomainEvents(IAsyncNotification eventItem)
        {
            domainEvents = domainEvents ?? new List<IAsyncNotification>();
            domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IAsyncNotification eventItem)
        {
            if(domainEvents is null)
            {
                return;
            }

            domainEvents.Remove(eventItem);            
        }
        
        public bool IsTransient()
        {
            return this.Id == default(Int32);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!requestedHashCode.HasValue)
                    requestedHashCode = this.Id.GetHashCode() ^ 31;
                return requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (object.Equals(left, null))
                return object.Equals(right, null) ? true : false;
            else

                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
