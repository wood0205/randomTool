using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainCreat : MonoBehaviour
{
    public static MainCreat Instance;

    public Text txt;
    public Text debug;
    public SpecialGroupArray specialGroup;
    public InputField inputSpecial;
    public Image currenImage;

    private List<int> data = new List<int>();

    private string ImageSavePath;


    private void Awake()
    {
        Instance = this;
        ImageSavePath = Application.dataPath + "/Resources/Bg.jpg";
    }

    private void Start()
    {
        LoadTexture();
    }

    public void CreatData()
    {
        data = RollPanel.Instance.CreatData();
        if (data.Count <= 0)//特殊滚轴
        {
            int special = 0;
            try
            {
                special= Convert.ToInt32(inputSpecial.text);
            }
            catch(Exception e)
            {
                txt.text = "输入的特殊滚轴数不是整数";
            }
            data.AddRange(specialGroup.CreatArray(special));
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < data.Count; i++)
        {
            sb.Append(data[i]);
            sb.Append(',');
        }
        sb.Remove(sb.Length - 1, 1);
        txt.text = sb.ToString();
    }

    public void Copy()
    {
        GUIUtility.systemCopyBuffer = txt.text;
    }

    public void ResetData()
    {
        RollPanel.Instance.ResetData();
        specialGroup.ResetData();
    }

    public void OpenFile()
    {
        OpenFileName ofn = new OpenFileName();

        ofn.structSize = Marshal.SizeOf(ofn);
        //可进行修改选择的文件类型
        ofn.filter = "图片文件(*.jpg*.png)\0*.jpg;*.png";
        ofn.file = new string(new char[256]);

        ofn.maxFile = ofn.file.Length;

        ofn.fileTitle = new string(new char[64]);

        ofn.maxFileTitle = ofn.fileTitle.Length;
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        //默认路径
        ofn.initialDir = path;

        ofn.title = "选择需要替换的图片";

        ofn.defExt = "JPG";//显示文件的类型
                           //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        if (WindowDll.GetOpenFileName(ofn))
        {
            string loadPath = "file:///" + ofn.file;
            StartCoroutine(LoadTextrue(loadPath, true));
        }
    }

    IEnumerator LoadTextrue(string path,bool needSave)
    {
        UnityWebRequest unityWebRequest = new UnityWebRequest(path);
        DownloadHandlerTexture handlerTexture = new DownloadHandlerTexture(true);
        unityWebRequest.downloadHandler = handlerTexture;
        yield return unityWebRequest.SendWebRequest();
        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            print(unityWebRequest.error);
        }
        else 
        {
            Texture2D t = handlerTexture.texture;
            //将选择的图片替换上去
            currenImage.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.one);

            if (needSave)
            {
                string[] strs = path.Split('\\');
                string name = strs[strs.Length - 1];
                OutputRt(t, name);
            }
        }
    }

    public void OutputRt(Texture2D png,string name)
    {
        byte[] dataBytes = png.EncodeToPNG();
        if (File.Exists(ImageSavePath))
        {
            File.Delete(ImageSavePath);
        }
        File.WriteAllBytes(ImageSavePath, dataBytes);
        PlayerPrefs.SetInt("IsSetBg",1);
    }

    public void OnOpenRollPanelButton()
    {
        RollPanel.Instance.ShowRollPanel(true);
    }

    private void LoadTexture()
    {
        if (IsSetBg())
        {
            StartCoroutine(LoadTextrue(ImageSavePath, false));
        }
        else
        {
            Texture2D t = Resources.Load<Texture2D>("Bg");
            Sprite sp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.one);
            currenImage.sprite = sp;
        }
    }

    private bool IsSetBg()
    {
        if (PlayerPrefs.GetInt("IsSetBg", 0) == 1)
        {
            return true;
        }
        return false;
    }
}
