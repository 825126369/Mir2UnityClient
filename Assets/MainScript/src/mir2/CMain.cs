using System;
using System.Diagnostics;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

namespace Mir2
{
    public partial class CMain : SingleTonMonoBehaviour<CMain>
    {
        public static Text DebugBaseLabel, HintBaseLabel;
        public static Text DebugTextLabel, HintTextLabel, ScreenshotTextLabel;
        public static Graphics Graphics;
        public static Point MPoint;

        public readonly static Stopwatch Timer = Stopwatch.StartNew();
        public readonly static DateTime StartTime = DateTime.UtcNow;
        public static long Time;

        public static bool Shift, Alt, Ctrl, Tilde, SpellTargetLock;

        public static readonly System.Random Random = new System.Random();
    }
}