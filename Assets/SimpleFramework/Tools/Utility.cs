using System;

public static class DelegateUtility
{
    public static bool CheckFunIsExist<T>(Action<T> mEvent, Action<T> fun)
    {
        if (mEvent == null)
        {
            return false;
        }
        Delegate[] mList = mEvent.GetInvocationList();
        return Array.Exists<Delegate>(mList, (x) => x.Equals(fun));
    }
}