using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gbm.Ordering.Domain.Common
{
    public abstract class Enumeration<T> : IComparable
        where T:Enumeration<T>
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        protected Enumeration() { }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration<T>;
            if(otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static IEnumerable<T> GetAll() 
        {
            var type = typeof(T);
            var fields = type.GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var info in fields)
            {
                if(info.GetValue(null) is T locatedValue)
                {
                    yield return locatedValue;
                }
            }
        }

        public int CompareTo(object obj)
        {
            if(obj == null)
            {
                return 1;
            }

            if(obj is Enumeration<T> otherEnumeration)
            {
                return Id.CompareTo(otherEnumeration.Id);
            }
            else
            {
                throw new ArgumentException($"Obj it's not of type {typeof(T).Name}");
            }
        }

        public static int AbsoluteDifference(Enumeration<T> firstValue, Enumeration<T> secondValue)
        {
            return Math.Abs(firstValue.Id - secondValue.Id);
        }

        public static T Parse<K>(K value, string description, Func<T,bool> predicate) 
        {
            var matchingItem = GetAll().FirstOrDefault(predicate);
            if(matchingItem == null)
            {
                var message = $"{value} is not valid {description} in {typeof(T)}";
                throw new InvalidOperationException(message);
            }

            return matchingItem;
        }

        public static T FromValue(int value)
        {
            return Parse(value, nameof(value), item => item.Id == value);
        }

        public static T FromDisplayName(string displayName) 
        {
            return Parse(displayName, nameof(displayName), 
                item => item.Name.Equals(displayName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
