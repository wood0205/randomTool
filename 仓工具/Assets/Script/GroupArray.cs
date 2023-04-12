using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GroupArray : MonoBehaviour
{
    protected InputField inputMin;
    protected InputField inputMax;
    protected InputField inputNum;
    protected InputField inputNone;
    protected InputField inputSpecify1;
    protected InputField inputInterval1;
    protected InputField inputSpecify2;
    protected InputField inputInterval2;

    protected int min, max, num, specify1,specify2;

    protected int specify1Index, specify2Index, interval1, interval2;
    protected List<int> array = new List<int>();

    private void Awake()
    {
        inputMin = transform.Find("InputField_min").GetComponent<InputField>();
        inputMax = transform.Find("InputField_max").GetComponent<InputField>();
        inputNum = transform.Find("InputField_num").GetComponent<InputField>();
        inputNone = transform.Find("InputField_none").GetComponent<InputField>();
        inputSpecify1 = transform.Find("InputField_specify1").GetComponent<InputField>();
        inputInterval1 = transform.Find("InputField_interval1").GetComponent<InputField>();
        inputSpecify2 = transform.Find("InputField_specify2").GetComponent<InputField>();
        inputInterval2 = transform.Find("InputField_interval2").GetComponent<InputField>();
    }

    public virtual List<int> CreatArray()
    {
        array.Clear();
        if (string.IsNullOrEmpty(inputMin.text) || string.IsNullOrEmpty(inputMax.text) || string.IsNullOrEmpty(inputNum.text)) return array;
        min = Convert.ToInt32(inputMin.text);
        max = Convert.ToInt32(inputMax.text);
        num = Convert.ToInt32(inputNum.text);
        Getspecify();
        List<int> notNeed = new List<int>();

        if (!string.IsNullOrEmpty(inputNone.text))
        {
            string[] notNeedStr = inputNone.text.Split(',');
            for (int i = 0; i < notNeedStr.Length; i++)
            {
                notNeed.Add(Convert.ToInt32(notNeedStr[i]));
            }
        }

        int index = 0;
        while (index < num)
        {
            int value = UnityEngine.Random.Range(min, max + 1);
            if (notNeed.Contains(value))
            {
                continue;
            }
            if (specify1 == value)
            {
                if (specify1Index > interval1)
                {
                    specify1Index = 0;
                }
                else
                {
                    continue;
                }
            }
            if (specify2 == value)
            {
                if (specify2Index > interval2)
                {
                    specify2Index = 0;
                }
                else
                {
                    continue;
                }
            }
            array.Add(value);
            index++;
            specify1Index++;
            specify2Index++;
        }
        return array;
    }

    protected void Getspecify()
    {
        specify1 = int.MinValue;
        specify2 = int.MinValue;
        specify1Index = 0;
        specify2Index = 0;
        interval1 = 0;
        interval2 = 0;
        try
        {
            if (!string.IsNullOrEmpty(inputSpecify1.text))
            {
                specify1 = Convert.ToInt32(inputSpecify1.text);
            }
            if (!string.IsNullOrEmpty(inputSpecify2.text))
            {
                specify2 = Convert.ToInt32(inputSpecify2.text);
            }
            if (!string.IsNullOrEmpty(inputInterval1.text))
            {
                interval1 = Convert.ToInt32(inputInterval1.text);
            }
            if (!string.IsNullOrEmpty(inputInterval2.text))
            {
                interval2 = Convert.ToInt32(inputInterval2.text);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void ResetData()
    {
        inputMin.text = string.Empty;
        inputMax.text = string.Empty;
        inputNum.text = string.Empty;
        inputNone.text = string.Empty;
        inputSpecify1.text = string.Empty;
        inputSpecify2.text = string.Empty;
        inputInterval1.text = string.Empty;
        inputInterval2.text = string.Empty;
    }

}
