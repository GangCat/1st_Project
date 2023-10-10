using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MyComparer<T> : IMyComparer<T>
{
    //protected MyComparer();
    public abstract int Compare(T x, T y);

    public static Comparer<T> Default { get; }

}
