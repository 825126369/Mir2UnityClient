

using UnityEngine;

public static class NodeExtension
{
    public static GameObject GetChildByPath(GameObject parent, string name)
    {
        Transform result = parent.transform.Find(name);
        if (result != null)
        {
            return result.gameObject;
        }
        return null;
    }
    

    public static void SetFirstSibling(GameObject node)
    {
        node.transform.SetSiblingIndex(0);
    }

    public static void SetLastSibling(GameObject node)
    {
        int maxIndex = node.transform.parent.childCount - 1;
        node.transform.SetSiblingIndex(maxIndex);
    }
}
