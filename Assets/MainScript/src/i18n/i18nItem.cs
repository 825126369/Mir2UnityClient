using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class i18nItem : MonoBehaviour
{
    public string key = "";
    private Text mText;

    private void Start()
    {
        this.mText = this.GetText();
        Do();
    }

    private Text GetText()
    {
        if(this.mText == null)
        {
            this.mText = this.GetComponent<Text>();
        }
        return mText;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        this.Do();
    }
#endif
    
    public void Do()
    {
        this.mText = GetText();
        PrintTool.Assert(mText != null, "mText == null: ", gameObject.GetTreeName());
        if(Application.isPlaying)
        {
            if(i18nController.readOnlyInstance != null)
            {
                this.mText.text = i18nController.readOnlyInstance.getKey(key);
            }
        }
        else
        {
            this.mText.text = i18nController.Instance.getKey(key);
        }
    }

}

