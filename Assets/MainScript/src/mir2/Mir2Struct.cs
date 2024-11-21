using UnityEngine;

namespace Mir2
{

    public class Door
    {
        public byte index;
        public DoorState DoorState;
        public byte ImageIndex;
        public long LastTick;
        public Vector3Int Location;
    }
}