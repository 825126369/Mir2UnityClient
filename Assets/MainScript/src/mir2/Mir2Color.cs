namespace Mir2
{
    public static class Mir2Color
    {
        public readonly static UnityEngine.Color Orange = ColorTo(System.Drawing.Color.Orange);
        public readonly static UnityEngine.Color MediumVioletRed = ColorTo(System.Drawing.Color.MediumVioletRed);
        public readonly static UnityEngine.Color Gray = ColorTo(System.Drawing.Color.Gray);
        public readonly static UnityEngine.Color Red = ColorTo(System.Drawing.Color.Red);
        public readonly static UnityEngine.Color Green = ColorTo(System.Drawing.Color.Green);
        public readonly static UnityEngine.Color Blue = ColorTo(System.Drawing.Color.Blue);
        public readonly static UnityEngine.Color Purple = ColorTo(System.Drawing.Color.Purple);
        public readonly static UnityEngine.Color Yellow = ColorTo(System.Drawing.Color.Yellow);
        public readonly static UnityEngine.Color DarkRed = ColorTo(System.Drawing.Color.DarkRed);
        public readonly static UnityEngine.Color White = ColorTo(System.Drawing.Color.White);

        private static UnityEngine.Color ColorTo(System.Drawing.Color ori)
        {
            return new UnityEngine.Color(ori.R / 255f, ori.G / 255f, ori.B / 255f, ori.A / 255f);
        }

    }
}