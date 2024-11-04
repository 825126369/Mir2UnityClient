using System;

public class UpdateMgr:SingleTonMonoBehaviour<UpdateMgr>
{
    event Action mapUpdateFunc;
    
    private void Update()
    {
        if (mapUpdateFunc != null)
        {
            mapUpdateFunc();
        }
    }

    public void AddListener(Action func)
    {
        if (CheckFunIsExist(func))
        {
            PrintTool.LogError("AddListener Errir");
        }
        else
        {
            mapUpdateFunc += func;
        }
    }

    public void RemoveListener(Action func)
    {
        this.mapUpdateFunc -= func;
    }

    private bool CheckFunIsExist(Action fun)
    {
        if (mapUpdateFunc != null) return false;
        Delegate[] mList = mapUpdateFunc.GetInvocationList();
        return Array.Exists<Delegate>(mList, (x) => x.Equals(fun));
    }
}
