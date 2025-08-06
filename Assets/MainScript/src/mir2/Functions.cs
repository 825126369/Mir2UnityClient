using System;
using System.Collections.Generic;
using UnityEngine;
namespace Mir2
{
    public static class Functions
    {
        public static bool CompareBytes(byte[] a, byte[] b)
        {
            if (a == b) return true;

            if (a == null || b == null || a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) return false;

            return true;
        }

        public static string ConvertByteSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }

        public static bool TryParse(string s, out Vector3Int temp)
        {
            temp = Vector3Int.zero;

            if (String.IsNullOrWhiteSpace(s)) return false;

            string[] data = s.Split(',');
            if (data.Length <= 1) return false;

            if (!Int32.TryParse(data[0], out int tempX))
                return false;

            if (!Int32.TryParse(data[1], out int tempY))
                return false;

            temp = new Vector3Int(tempX, tempY);
            return true;
        }

        public static bool InRange(Vector3Int a, Vector3Int b, int i)
        {
            return Math.Abs(a.x - b.x) <= i && Math.Abs(a.y - b.y) <= i;
        }

        public static bool FacingEachOther(MirDirection dirA, Vector3Int pointA, MirDirection dirB, Vector3Int pointB)
        {
            if (dirA == DirectionFromPoint(pointA, pointB) && dirB == DirectionFromPoint(pointB, pointA))
            {
                return true;
            }

            return false;
        }

        public static string PrintTimeSpanFromSeconds(double secs, bool accurate = true)
        {
            TimeSpan t = TimeSpan.FromSeconds(secs);
            string answer;
            if (t.TotalMinutes < 1.0)
            {
                answer = string.Format("{0}s", t.Seconds);
            }
            else if (t.TotalHours < 1.0)
            {
                answer = accurate ? string.Format("{0}m {1:D2}s", t.Minutes, t.Seconds) : string.Format("{0}m", t.Minutes);
            }
            else if (t.TotalDays < 1.0)
            {
                answer = accurate ? string.Format("{0}h {1:D2}m {2:D2}s", (int)t.Hours, t.Minutes, t.Seconds) : string.Format("{0}h {1:D2}m", (int)t.TotalHours, t.Minutes);
            }
            else // more than 1 day
            {
                answer = accurate ? string.Format("{0}d {1:D2}h {2:D2}m {3:D2}s", (int)t.Days, (int)t.Hours, t.Minutes, t.Seconds) : string.Format("{0}d {1}h {2:D2}m", (int)t.TotalDays, (int)t.Hours, t.Minutes);
            }

            return answer;
        }

        public static string PrintTimeSpanFromMilliSeconds(double milliSeconds)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(milliSeconds);
            string answer;
            if (t.TotalMinutes < 1.0)
            {
                answer = string.Format("{0}.{1}s", t.Seconds, (decimal)(t.Milliseconds / 100));
            }
            else if (t.TotalHours < 1.0)
            {
                answer = string.Format("{0}m {1:D2}s", t.TotalMinutes, t.Seconds);
            }
            else if (t.TotalDays < 1.0)
            {
                answer = string.Format("{0}h {1:D2}m {2:D2}s", (int)t.TotalHours, t.Minutes, t.Seconds);
            }
            else
            {
                answer = string.Format("{0}d {1}h {2:D2}m {3:D2}s", (int)t.Days, (int)t.Hours, t.Minutes, t.Seconds);
            }

            return answer;
        }

        public static MirDirection PreviousDir(MirDirection d)
        {
            switch (d)
            {
                case MirDirection.Up:
                    return MirDirection.UpLeft;
                case MirDirection.UpRight:
                    return MirDirection.Up;
                case MirDirection.Right:
                    return MirDirection.UpRight;
                case MirDirection.DownRight:
                    return MirDirection.Right;
                case MirDirection.Down:
                    return MirDirection.DownRight;
                case MirDirection.DownLeft:
                    return MirDirection.Down;
                case MirDirection.Left:
                    return MirDirection.DownLeft;
                case MirDirection.UpLeft:
                    return MirDirection.Left;
                default: return d;
            }
        }
        public static MirDirection NextDir(MirDirection d)
        {
            switch (d)
            {
                case MirDirection.Up:
                    return MirDirection.UpRight;
                case MirDirection.UpRight:
                    return MirDirection.Right;
                case MirDirection.Right:
                    return MirDirection.DownRight;
                case MirDirection.DownRight:
                    return MirDirection.Down;
                case MirDirection.Down:
                    return MirDirection.DownLeft;
                case MirDirection.DownLeft:
                    return MirDirection.Left;
                case MirDirection.Left:
                    return MirDirection.UpLeft;
                case MirDirection.UpLeft:
                    return MirDirection.Up;
                default: return d;
            }
        }

        public static MirDirection DirectionFromPoint(Vector3Int source, Vector3Int dest)
        {
            if (source.x < dest.x)
            {
                if (source.y < dest.y)
                    return MirDirection.DownRight;
                if (source.y > dest.y)
                    return MirDirection.UpRight;
                return MirDirection.Right;
            }

            if (source.x > dest.x)
            {
                if (source.y < dest.y)
                    return MirDirection.DownLeft;
                if (source.y > dest.y)
                    return MirDirection.UpLeft;
                return MirDirection.Left;
            }

            return source.y < dest.y ? MirDirection.Down : MirDirection.Up;
        }

        public static MirDirection ShiftDirection(MirDirection dir, int i)
        {
            return (MirDirection)(((int)dir + i + 8) % 8);
        }

        public static Vector3Int PointMove(Vector3Int p, MirDirection d, int i)
        {
            switch (d)
            {
                case MirDirection.Up:
                    p += new Vector3Int(0, -i);
                    break;
                case MirDirection.UpRight:
                    p += new Vector3Int(i, -i);
                    break;
                case MirDirection.Right:
                    p += new Vector3Int(i, 0);
                    break;
                case MirDirection.DownRight:
                    p += new Vector3Int(i, i);
                    break;
                case MirDirection.Down:
                    p += new Vector3Int(0, i);
                    break;
                case MirDirection.DownLeft:
                    p += new Vector3Int(-i, i);
                    break;
                case MirDirection.Left:
                    p += new Vector3Int(-i, 0);
                    break;
                case MirDirection.UpLeft:
                    p += new Vector3Int(-i, -i);
                    break;
            }
            return p;
        }

        public static int MaxDistance(Vector3Int p1, Vector3Int p2)
        {
            return Math.Max(Math.Abs(p1.x - p2.x), Math.Abs(p1.y - p2.y));
        }

        public static MirDirection ReverseDirection(MirDirection dir)
        {
            switch (dir)
            {
                case MirDirection.Up:
                    return MirDirection.Down;
                case MirDirection.UpRight:
                    return MirDirection.DownLeft;
                case MirDirection.Right:
                    return MirDirection.Left;
                case MirDirection.DownRight:
                    return MirDirection.UpLeft;
                case MirDirection.Down:
                    return MirDirection.Up;
                case MirDirection.DownLeft:
                    return MirDirection.UpRight;
                case MirDirection.Left:
                    return MirDirection.Right;
                case MirDirection.UpLeft:
                    return MirDirection.DownRight;
                default:
                    return dir;
            }
        }

        public static ItemInfoCFG GetRealItem(ItemInfoCFG Origin, ushort Level, MirClass job, List<ItemInfoCFG> ItemList)
        {
            if (Origin.ClassBased && Origin.LevelBased)
                return GetClassAndLevelBasedItem(Origin, job, Level, ItemList);
            if (Origin.ClassBased)
                return GetClassBasedItem(Origin, job, ItemList);
            if (Origin.LevelBased)
                return GetLevelBasedItem(Origin, Level, ItemList);
            return Origin;
        }

        public static ItemInfoCFG GetLevelBasedItem(ItemInfoCFG Origin, ushort level, List<ItemInfoCFG> ItemList)
        {
            ItemInfoCFG output = Origin;
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemInfoCFG info = ItemList[i];
                if (info.ItemName.StartsWith(Origin.ItemName))
                    if ((info.ItemRequiredType == RequiredType.Level) && 
                        (info.ItemRequiredAmount <= level) && 
                        (output.ItemRequiredAmount < info.ItemRequiredAmount) && 
                        (Origin.ItemRequiredGender == info.ItemRequiredGender))
                        output = info;
            }
            return output;
        }
        public static ItemInfoCFG GetClassBasedItem(ItemInfoCFG Origin, MirClass job, List<ItemInfoCFG> ItemList)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemInfoCFG info = ItemList[i];
                if (info.ItemName.StartsWith(Origin.ItemName))
                    if (((byte)info.ItemRequiredClass == (1 << (byte)job)) && 
                        (Origin.ItemRequiredGender == info.ItemRequiredGender))
                        return info;
            }
            return Origin;
        }

        public static ItemInfoCFG GetClassAndLevelBasedItem(ItemInfoCFG Origin, MirClass job, ushort level, List<ItemInfoCFG> ItemList)
        {
            ItemInfoCFG output = Origin;
            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemInfoCFG info = ItemList[i];
                if (info.ItemName.StartsWith(Origin.ItemName))
                    if ((byte)info.ItemRequiredClass == (1 << (byte)job))
                        if ((info.ItemRequiredType == RequiredType.Level) &&
                            (info.ItemRequiredAmount <= level) &&
                            (output.ItemRequiredAmount <= info.ItemRequiredAmount) &&
                            (Origin.ItemRequiredGender == info.ItemRequiredGender))
                            output = info;
            }
            return output;
        }

        //    public static string StringOverLines(string line, int maxWordsPerLine, int maxLettersPerLine)
        //    {
        //        string newString = string.Empty;

        //        string[] words = line.Split(' ');

        //        int lineLength = 0;

        //        for (int i = 0; i < words.Length; i++)
        //        {
        //            lineLength += words[i].Length + 1;

        //            newString += words[i] + " ";
        //            if (i > 0 && i % maxWordsPerLine == 0 && lineLength > maxLettersPerLine)
        //            {
        //                lineLength = 0;
        //                newString += "\r\n";
        //            }
        //        }

        //        return newString;
        //    }

        //    public static IEnumerable<byte[]> SplitArray(byte[] value, int bufferLength)
        //    {
        //        int countOfArray = value.Length / bufferLength;
        //        if (value.Length % bufferLength > 0)
        //            countOfArray++;
        //        for (int i = 0; i < countOfArray; i++)
        //        {
        //            yield return value.Skip(i * bufferLength).Take(bufferLength).ToArray();
        //        }
        //    }

        //    public static byte[] CombineArray(List<byte[]> arrays)
        //    {
        //        byte[] rv = new byte[arrays.Sum(x => x.Length)];
        //        int offset = 0;
        //        foreach (byte[] array in arrays)
        //        {
        //            System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
        //            offset += array.Length;
        //        }
        //        return rv;
        //    }
        //    public static byte[] SerializeToBytes<T>(T item)
        //    {
        //#pragma warning disable SYSLIB0011
        //        var formatter = new BinaryFormatter();
        //        using (var stream = new MemoryStream())
        //        {
        //            formatter.Serialize(stream, item);
        //            stream.Seek(0, SeekOrigin.Begin);
        //#pragma warning restore SYSLIB0011
        //            return stream.ToArray();
        //        }
        //    }
        //    public static object DeserializeFromBytes(byte[] bytes)
        //    {
        //#pragma warning disable SYSLIB0011
        //        var formatter = new BinaryFormatter();
        //        using (var stream = new MemoryStream(bytes))
        //        {
        //            var deserialized = formatter.Deserialize(stream);
        //#pragma warning restore SYSLIB0011
        //            return deserialized;
        //        }
        //    }

        //    /// <summary>
        //    /// Chop a List into chunks
        //    /// </summary>
        //    /// <param name="width">The amount of Chunks desired</param>
        //    /// <param name="originalList">The list to Chop into Chunks</param>
        //    /// <returns>Original List in Chunks within a List</returns>
        //    public static List<List<T>> SplitList<T>(int width, List<T> originalList)
        //    {
        //        var _tempChunks = new List<List<T>>();

        //        if (width == 0)
        //            _tempChunks.Add(originalList);
        //        else
        //        {
        //            // Determine how many lists are required 
        //            var numberOfLists = (originalList.Count / width);

        //            for (var i = 0; i <= numberOfLists; i++)
        //            {
        //                var newChunk = originalList.Skip(i * width).Take(width).ToList();
        //                _tempChunks.Add(newChunk);
        //            }
        //        }

        //        return _tempChunks;
        //    }
    }
}