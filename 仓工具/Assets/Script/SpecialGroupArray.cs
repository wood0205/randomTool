using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialGroupArray : GroupArray
{
    public List<int> CreatArray(int special)
    {
        var array = base.CreatArray();
        List<int> res = new List<int>();
        for (int i = 0; i < array.Count; i++)
        {
            res.Add(array[i]);
            if(i!=array.Count-1)
            {
                res.Add(special);
            }
        }
        return res;
    }
}

