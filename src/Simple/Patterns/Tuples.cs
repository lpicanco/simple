﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Simple.Patterns
{
    [Serializable]
    public class Tuple<T>
    {
        public ReadOnlyCollection<T> Values { get; private set; }

        public int Count { get { return Values.Count; } }

        public Tuple(params T[] values)
        {
            Values = new ReadOnlyCollection<T>(new List<T>(values));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tuple<T>)) return false;

            return Values.SequenceEqual((obj as Tuple<T>).Values);
        }

        public override int GetHashCode()
        {
            int res = 0;
            foreach (T value in Values)
                res ^= EqualityComparer<T>.Default.GetHashCode(value);

            return res;
        }

        public override string ToString()
        {
            return Values.StringJoin(", ");
        }

    }

    public static class Tuples
    {
        public static Pair<T> Get<T>(T first, T second)
        {
            return new Pair<T>(first, second);
        }

        public static Pair<T, Q> Get<T, Q>(T first, Q second)
        {
            return new Pair<T, Q>(first, second);
        }

        public static Triplet<T> Get<T>(T first, T second, T third)
        {
            return new Triplet<T>(first, second, third);
        }

        public static Triplet<T, Q, P> Get<T, Q, P>(T first, Q second, P third)
        {
            return new Triplet<T, Q, P>(first, second, third);
        }
    }

    [Serializable]
    public class Pair<T, Q>
    {
        public T First { get; protected set; }
        public Q Second { get; protected set; }

        public Pair(T first, Q second)
        {
            First = first;
            Second = second;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Pair<T, Q>)) return false;
            var obj2 = obj as Pair<T, Q>;

            return
                EqualityComparer<T>.Default.Equals(First, obj2.First) &&
                EqualityComparer<Q>.Default.Equals(Second, obj2.Second);

        }

        public override int GetHashCode()
        {
            return
               EqualityComparer<T>.Default.GetHashCode(First) ^
               EqualityComparer<Q>.Default.GetHashCode(Second);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", First, Second);
        }
    }

    [Serializable]
    public class Pair<T> : Pair<T, T>
    {
        public Pair(T first, T second) : base(first, second) { }
    }

    [Serializable]
    public class Triplet<T, Q, P>
    {
        public T First { get; protected set; }
        public Q Second { get; protected set; }
        public P Third { get; protected set; }

        public Triplet(T first, Q second, P third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Triplet<T, Q, P>)) return false;
            var obj2 = obj as Triplet<T, Q, P>;

            return
                EqualityComparer<T>.Default.Equals(First, obj2.First) &&
                EqualityComparer<Q>.Default.Equals(Second, obj2.Second) &&
                EqualityComparer<P>.Default.Equals(Third, obj2.Third);
            ;
        }

        public override int GetHashCode()
        {
            return
                EqualityComparer<T>.Default.GetHashCode(First) ^
                EqualityComparer<Q>.Default.GetHashCode(Second) ^
                EqualityComparer<P>.Default.GetHashCode(Third);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", First, Second, Third);
        }
    }

    [Serializable]
    public class Triplet<T> : Triplet<T, T, T>
    {
        public Triplet(T first, T second, T third) : base(first, second, third) { }
    }

}
