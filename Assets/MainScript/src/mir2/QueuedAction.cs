using System.Collections.Generic;
using UnityEngine;

public class QueuedAction
{
    public MirAction Action;
    public Vector3Int Location;
    public MirDirection Direction;
    public List<object> Params;
}