using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Mir2
{
    public sealed class MapControl:Singleton<MapControl>
    {
        public static UserObject User
        {
            get { return MapObject.User; }
            set { MapObject.User = value; }
        }

        private static long nextActionTime;
        public static long NextActionTime
        {
            get { return nextActionTime; }
            set
            {
                if (GameScene.Observing) return;
                nextActionTime = value;
            }
        }
        public static long InputDelay;

        public static List<Effect> Effects = new List<Effect>();

        public const int CellWidth = 48;
        public const int CellHeight = 32;

        public static int OffSetX;
        public static int OffSetY;

        public static int ViewRangeX;
        public static int ViewRangeY;
        public static Dictionary<uint, MapObject> Objects = new Dictionary<uint, MapObject>();
        public static List<MapObject> ObjectsList = new List<MapObject>();

        public CellInfo[,] M2CellInfo;
        public List<Door> Doors = new List<Door>();
        public int Width, Height;

        public static Vector3Int MapLocation
        {
            get { return GameScene.User == null ? Vector3Int.zero : new Vector3Int(MouseLocation.x / CellWidth - OffSetX, MouseLocation.y / CellHeight - OffSetY) + GameScene.User.CurrentLocation; }
        }

        public static Vector3 ToMouseLocation(Vector3Int p)
        {
            return new Vector3((p.x - MapObject.User.Movement.x + OffSetX) * CellWidth, (p.y - MapObject.User.Movement.y + OffSetY) * CellHeight) + MapObject.User.OffSetMove;
        }

        public static Vector3Int MouseLocation;
        public bool FloorValid, LightsValid;
        protected internal bool TextureValid;

        public static int Direction16(Vector3Int source, Vector3Int destination)
        {
            Vector3 c = new Vector3(source.x, source.y);
            Vector3 a = new Vector3(c.x, 0);
            Vector3 b = new Vector3(destination.x, destination.y);
            float bc = (float)Vector3.Distance(c, b);
            float ac = bc;
            b.y -= c.y;
            c.y += bc;
            b.y += bc;
            float ab = (float)Vector3.Distance(b, a);
            double x = (ac * ac + bc * bc - ab * ab) / (2 * ac * bc);
            double angle = Math.Acos(x);

            angle *= 180 / Math.PI;
            if (destination.x < c.x)
            {
                angle = 360 - angle;
                angle += 11.25F;
            }
            if (angle > 360)
            {
                angle -= 360;
            }
            return (int)(angle / 22.5F);
        }
        
        public void RemoveObject(MapObject ob)
        {
            M2CellInfo[ob.MapLocation.x, ob.MapLocation.y].RemoveObject(ob);
        }
        public void AddObject(MapObject ob)
        {
            M2CellInfo[ob.MapLocation.x, ob.MapLocation.y].AddObject(ob);
        }
        public MapObject FindObject(uint ObjectID, int x, int y)
        {
            return M2CellInfo[x, y].FindObject(ObjectID);
        }
        public void SortObject(MapObject ob)
        {
            M2CellInfo[ob.MapLocation.x, ob.MapLocation.y].Sort();
        }
        public static MapObject GetObject(uint targetID)
        {
            Objects.TryGetValue(targetID, out var ob);
            return ob;
        }

        public bool HasTarget(Vector3Int p)
        {
            foreach (var ob in Objects.Values)
            {
                if (ob.CurrentLocation == p && ob.Blocking)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanHalfMoon(Vector3Int p, MirDirection d)
        {
            d = Functions.PreviousDir(d);
            for (int i = 0; i < 4; i++)
            {
                if (HasTarget(Functions.PointMove(p, d, 1))) return true;
                d = Functions.NextDir(d);
            }
            return false;
        }
        public bool CanCrossHalfMoon(Vector3Int p)
        {
            MirDirection dir = MirDirection.Up;
            for (int i = 0; i < 8; i++)
            {
                if (HasTarget(Functions.PointMove(p, dir, 1))) return true;
                dir = Functions.NextDir(dir);
            }
            return false;
        }

        public bool ValidPoint(Vector3Int p)
        {
            //GameScene.Scene.ChatDialog.ReceiveChat(string.Format("cell: {0}", (M2CellInfo[p.X, p.Y].BackImage & 0x20000000)), ChatType.Hint);
            return (M2CellInfo[p.x, p.y].BackImage & 0x20000000) == 0;
        }



    }
}