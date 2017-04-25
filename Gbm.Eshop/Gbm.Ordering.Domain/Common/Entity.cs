using System;
using System.Collections.Generic;
using System.Text;


namespace Gbm.Ordering.Domain.Common
{
    public abstract class Entity
    {
        int? requestedHashCode;
        int id;

        public virtual int Id
        {
            get
            {
                return id;
            }
            protected set
            {
                id = value;
            }
        }

        
    }
}
