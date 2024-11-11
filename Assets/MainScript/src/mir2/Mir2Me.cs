using NetProtocols.Game;
using System;
using UnityEngine;

namespace Mir2
{
    public class Mir2Me : SingleTonMonoBehaviour<Mir2Me>
    {
        public float fSpeed = 100f;
        TimeOutGenerator mTimeOut_MouseDown = null;

        public const int CellWidth = 48;
        public const int CellHeight = 32;

        public static int OffSetX;
        public static int OffSetY;

        public static int ViewRangeX;
        public static int ViewRangeY;

        bool bAtuoRun = false;

        public Vector3Int MapLocation;
        public MirDirection Direction;
        public MirAction CurrentAction;

        [NonSerialized]
        public Vector3Int CurrentLocation;

        public MirGender Gender;
        public MirClass Class;
        public byte Hair;
        public ushort Level;
        public int FrameIndex;

        private bool bInit = false;
        public void Init()
        {
            if (bInit) return;
            bInit = true;
            mTimeOut_MouseDown = TimeOutGenerator.New(0.5f);
            CurrentLocation = new Vector3Int(MapLocation.x * TileMapMgr.CellWidth, MapLocation.y * TileMapMgr.CellHeight, 0);
            Direction = MirDirection.UpRight;
            transform.position = CurrentLocation;
        }


        private void Awake()
        {
            Init();
        }

        private void OnDrawGizmos()
        {
            
        }

        private void Update()
        {
            bool bClickRight = false;
            if (Input.GetMouseButton(0))
            {
                bClickRight = false;
            }
            else if (Input.GetMouseButton(1))
            {
                bClickRight = true;
            }

            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                bool bClickMap = false;
                float distance = 1000f;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, distance))
                {
                    PrintTool.Log("Ray: " + hit.collider.gameObject.name);
                    BoxCollider mHitCollider = hit.collider.GetComponent<BoxCollider>();
                    if (mHitCollider != null && mHitCollider.gameObject.name == "MapClickBoxCollider")
                    {
                        PrintTool.Log("µã»÷µØÍ¼£º" + mHitCollider.transform.position);
                        bClickMap = true;
                    }
                }
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
                
                if (bClickMap)
                {
                    bAtuoRun = false;
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 dir = Vector3.Normalize(pos - transform.position);
                    Direction = GetDirection(dir);

                    if (bClickRight)
                    {
                        SendRunMsg();
                    }
                    else
                    {
                        SendWalkMsg();
                    }

                    if (TileMapMgr.readOnlyInstance != null)
                    {
                        TileMapMgr.readOnlyInstance.UpdateMap();
                    }
                }
            }
        }

        private MirDirection GetDirection(Vector3 dir)
        {
            if (dir.x > 0)
            {
                if (dir.y < 0)
                    return MirDirection.DownRight;
                if (dir.y > 0)
                    return MirDirection.UpRight;
                return MirDirection.Right;
            }

            if (dir.x < 0)
            {
                if (dir.y < 0)
                    return MirDirection.DownLeft;
                if (dir.y > 0)
                    return MirDirection.UpLeft;
                return MirDirection.Left;
            }
            return dir.y < 0 ? MirDirection.Down : MirDirection.Up;
        }

        private Vector3Int GetDirOffset(MirDirection Direction)
        {
            Vector3Int targetPos = Vector3Int.zero;
            if (Direction == MirDirection.Left)
            {
                targetPos = new Vector3Int(-1, 0, 0);
            }
            else if (Direction == MirDirection.Right)
            {
                targetPos = new Vector3Int(1, 0, 0);
            }
            else if (Direction == MirDirection.Up)
            {
                targetPos = new Vector3Int(0, 1, 0);
            }
            else if (Direction == MirDirection.Down)
            {
                targetPos = new Vector3Int(0, -1, 0);
            }
            else if (Direction == MirDirection.UpRight)
            {
                targetPos = new Vector3Int(1, 1, 0);
            }
            else if (Direction == MirDirection.DownRight)
            {
                targetPos = new Vector3Int(1, -1, 0);
            }
            else if (Direction == MirDirection.DownLeft)
            {
                targetPos = new Vector3Int(-1, -1, 0);
            }
            else if (Direction == MirDirection.UpLeft)
            {
                targetPos = new Vector3Int(-1, 1, 0);
            }
            else
            {
                PrintTool.Assert(false);
            }

            return targetPos;
        }

        private void OnSimpleMove()
        {
            Vector3Int targetPos = CurrentLocation + GetDirOffset(Direction);
            CurrentLocation = targetPos;
            MapLocation = new Vector3Int(CurrentLocation.x / TileMapMgr.CellWidth, CurrentLocation.y / TileMapMgr.CellHeight, 0);
            transform.position = CurrentLocation;
        }

        private void UpdateLocation(Vector3Int Location, MirDirection dir)
        {
            CurrentLocation = Location;
            MapLocation = new Vector3Int(CurrentLocation.x / TileMapMgr.CellWidth, CurrentLocation.y / TileMapMgr.CellHeight, 0);
            transform.position = CurrentLocation;

            Direction = dir;
        }

        private void UpdateFrame()
        {
            FrameIndex++;
            if (FrameIndex > 2)
            {
                FrameIndex = 0;
            }
        }

        private void SendWalkMsg()
        {
            var mSendMsg = new packet_cs_request_Walk();
            mSendMsg.Direction = (uint)Direction;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_WALK, mSendMsg);
        }

        private void SendRunMsg()
        {
            var mSendMsg = new packet_cs_request_Run();
            mSendMsg.Direction = (uint)Direction;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_RUN, mSendMsg);
        }

        public void HandleServerLocation(Vector3Int Location, MirDirection dir)
        {
            UpdateLocation(Location, dir);
        }

    }
}

