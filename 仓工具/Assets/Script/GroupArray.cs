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
    protected Text txt_Title;

    protected int min, max, num, specify1,specify2;

    protected int specify1Index, specify2Index, interval1, interval2;
    protected List<int> array = new List<int>();

    private List<int> notNeed = new List<int>();
    private bool isInputMin, isInputMax, isInputNum, isInputNone, isInputSpecify1, isInputSpecify2, isInputInterval1, isInputInterval2;

    private const string normalStr = "R{0}";
    private const string spStr = "Sp";

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
        txt_Title = transform.Find("Title/txt_Title").GetComponent<Text>();

        //添加修改文本回调
        inputMin.onEndEdit.AddListener(IpMinOver);
        inputMax.onEndEdit.AddListener(IpMaxOver);
        inputNum.onEndEdit.AddListener(IpNumOver);
        inputNone.onEndEdit.AddListener(IpNoneOver);
        inputSpecify1.onEndEdit.AddListener(IpSpecify1Over);
        inputInterval1.onEndEdit.AddListener(IpInterval1Over);
        inputSpecify2.onEndEdit.AddListener(IpSpecify2Over);
        inputInterval2.onEndEdit.AddListener(IpInterval2Over);
    }

    public virtual void InitName(int idx)
    {
        if (idx >= 0)
        {
            txt_Title.text = string.Format(normalStr, idx + 1);
        }
        else
        {
            txt_Title.text = spStr;
        }
        ResetText();
        if(isInputMin)
        {
            inputMin.text = min.ToString();
        }
        if(isInputMax)
        {
            inputMax.text = max.ToString();
        }
        if(isInputNum)
        {
            inputNum.text = num.ToString();
        }
        if(isInputNone)
        {
            if (notNeed.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < notNeed.Count; i++)
                {
                    sb.Append(notNeed[i]);
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                inputNone.text = sb.ToString();
            }
        }
        if (isInputSpecify1)
        {
            inputSpecify1.text = specify1.ToString();
        }
        if (isInputSpecify2)
        {
            inputSpecify2.text = specify2.ToString();
        }
        if (isInputInterval1)
        {
            inputInterval1.text = interval1.ToString();
        }
        if (isInputInterval2)
        {
            inputInterval2.text = interval2.ToString();
        }
    }

    public virtual List<int> CreatArray()
    {
        array.Clear();
        if (string.IsNullOrEmpty(inputMin.text) || string.IsNullOrEmpty(inputMax.text) || string.IsNullOrEmpty(inputNum.text)) return array;
        min = Convert.ToInt32(inputMin.text);
        max = Convert.ToInt32(inputMax.text);
        num = Convert.ToInt32(inputNum.text);
        Getspecify();



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

    #region 输入回调

    private void IpMinOver(string str)
    {
        isInputMin = !string.IsNullOrEmpty(str);
        if (isInputMin)
        {
            min = Convert.ToInt32(str);
        }
    }

    private void IpMaxOver(string str)
    {
        isInputMax = !string.IsNullOrEmpty(str);
        if (isInputMax)
        {
            max = Convert.ToInt32(str);
        }
    }

    private void IpNumOver(string str)
    {
        isInputNum = !string.IsNullOrEmpty(str);
        if (isInputNum)
        {
            num = Convert.ToInt32(str);
        }
    }

    private void IpNoneOver(string str)
    {
        isInputNone = !string.IsNullOrEmpty(str);
        if (isInputNone)
        {
            string[] notNeedStr = inputNone.text.Split(',');
            for (int i = 0; i < notNeedStr.Length; i++)
            {
                notNeed.Add(Convert.ToInt32(notNeedStr[i]));
            }
            if (notNeed.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < notNeed.Count; i++)
                {
                    sb.Append(notNeed[i]);
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                inputNone.text = sb.ToString();
            }
        }
    }

    private void IpSpecify1Over(string str)
    {
        isInputSpecify1 = !string.IsNullOrEmpty(str);
        if (isInputSpecify1)
        {
            specify1 = Convert.ToInt32(str);
        }
    }

    private void IpInterval1Over(string str)
    {
        isInputInterval1 = !string.IsNullOrEmpty(str);
        if (isInputInterval1)
        {
            interval1 = Convert.ToInt32(str);
        }
    }

    private void IpSpecify2Over(string str)
    {
        isInputSpecify2 = !string.IsNullOrEmpty(str);
        if (isInputSpecify2)
        {
            specify2 = Convert.ToInt32(str);
        }
    }

    private void IpInterval2Over(string str)
    {
        isInputInterval2 = !string.IsNullOrEmpty(str);
        if (isInputInterval2)
        {
            interval2 = Convert.ToInt32(str);
        }
    }

    #endregion
    private void ResetText()
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

    public void ResetData()
    {
        ResetText();
        isInputMin = isInputMax = isInputNum = isInputNone = isInputSpecify1 = isInputSpecify2 = isInputInterval1 = isInputInterval2 = false;
    }

}
