using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollPanel : MonoBehaviour
{
    public static RollPanel Instance;

    public InputField inputField_Num;
    public Color color;
    public ScrollRect scrollRect;
    public GameObject rollPre;

    private List<GroupArray> totalGroupList = new List<GroupArray>();
    private List<int> creatData = new List<int>();
    private int groupNum;
    private void Awake()
    {
        ShowRollPanel(false);
        Instance = this;
        inputField_Num.onEndEdit.AddListener(NumChangeEnd);
    }

    public void ShowRollPanel(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

    private void NumChangeEnd(string str)
    {
        try
        {
            groupNum = int.Parse(str);
            if (groupNum <= 0)
            {
                MainCreat.Instance.txt.text = "请输入32位以内正整数的轴数";
                return;
            }
            for (int i = 0; i < totalGroupList.Count; i++)
            {
                if(i< groupNum)
                {
                    totalGroupList[i].gameObject.SetActive(true);
                }
                else
                {
                    totalGroupList[i].gameObject.SetActive(false);
                }
            }
            int needAdd = groupNum - totalGroupList.Count;
            if (needAdd > 0)
            {
                int num = totalGroupList.Count;
                for (int i = 0; i < needAdd; i++)
                {
                    GameObject go = GameObject.Instantiate(rollPre);
                    go.transform.SetParent(scrollRect.content);
                    var item = go.GetComponent<GroupArray>();
                    item.InitName(num + i);
                    totalGroupList.Add(item);
                }
            }
        }
        catch
        {
            MainCreat.Instance.txt.text = "请输入32位以内正整数的轴数";
        }
    }

    public List<int> CreatData()
    {
        creatData.Clear();
        for (int i = 0; i < groupNum; i++)
        {
            creatData.AddRange(totalGroupList[i].CreatArray());
        }
        return creatData;
    }

    public void ResetData()
    {
        for (int i = 0; i < totalGroupList.Count; i++)
        {
            totalGroupList[i].ResetData();
        }
    }

    public void OnCloseButton()
    {
        ShowRollPanel(false);
    }

    public void OnCreatDataButton()
    {
        MainCreat.Instance.CreatData();
        gameObject.SetActive(false);
    }

    //public void OnSetColorButton()
    //{

    //}
}
