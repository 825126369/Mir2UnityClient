using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class InitSceneDialogBox : MonoBehaviour
{
    public Text textTitle;
    public Text textMessage;
    public Button mYesBtn;
    public Button mCancelBtn;
    public Button mSureBtn;

    Action mYesEvent = null;
    Action mNoEvent = null;
    Action mSureEvent = null;
    private void Awake()
    {
        mYesBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            mYesEvent?.Invoke();
        });

        mCancelBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            mNoEvent?.Invoke();
        });

        mSureBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            mSureEvent?.Invoke();
        });
    }

    public void ShowYesNo(string message, string title = null, Action mYesEvent = null, Action mNoEvent = null)
    {
        if (title == null)
        {
            title = "提示";
        }

        mYesBtn.gameObject.SetActive(false);
        mCancelBtn.gameObject.SetActive(false);
        mSureBtn.gameObject.SetActive(false);

        mYesBtn.gameObject.SetActive(true);
        mCancelBtn.gameObject.SetActive(true);

        this.mYesEvent = mYesEvent;
        this.mNoEvent = mNoEvent;

        gameObject.SetActive(true);
        textMessage.text = message;
        textTitle.text = title;

        transform.SetAsLastSibling();
        gameObject.SetActive(true);
    }

    public void ShowSure(string message, string title = null, Action mSureEvent = null)
    {
        if (title == null)
        {
            title = "提示";
        }

        mYesBtn.gameObject.SetActive(false);
        mCancelBtn.gameObject.SetActive(false);
        mSureBtn.gameObject.SetActive(false);

        mSureBtn.gameObject.SetActive(true);

        this.mSureEvent = mSureEvent;

        gameObject.SetActive(true);
        textMessage.text = message;
        textTitle.text = title;

        transform.SetAsLastSibling();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
