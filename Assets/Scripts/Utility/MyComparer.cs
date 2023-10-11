using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyComparer : IMyComparer
{
    public int Compare(int _x, int _y)
    {
        if (_x.Equals(_y)) return 0;

        return _x < _y ? -1 : 1;
    }
}
