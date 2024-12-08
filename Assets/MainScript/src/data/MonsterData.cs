using System.Collections.Generic;
using UnityEngine;

public class MonsterData
{
    public uint nId;
    public string Name;

    //Location
    public uint CurrentMapIndex; //��ǰ��ͼIndex
    public Vector3Int CurrentLocation;
    public uint Direction;

    public uint HP;
    public uint MP;

    public uint nAttack = 100;
}