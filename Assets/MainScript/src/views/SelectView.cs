using UnityEngine;

public class SelectView : MonoBehaviour
{
    private bool bInit = false;
    private void Init()
    {
        if (bInit) return;
        bInit = true;   
    }

    public void Show()
    {
        Init();
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
