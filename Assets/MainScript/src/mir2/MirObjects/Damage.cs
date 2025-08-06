using UnityEngine;
using UnityEngine.UI;

namespace Mir2
{
    public class Damage:MonoBehaviour
    {
        public string Text;
        public Color Colour;
        public int Distance;
        public long ExpireTime;
        public double Factor;
        public int Offset;
        public Text DamageLabel;

        public Damage(string text, int duration, Color colour, int distance = 50)
        {
            ExpireTime = (long)(CMain.Time + duration);
            Text = text;
            Distance = distance;
            Factor = duration / this.Distance;
            Colour = colour;
        }

        public void Draw(Vector3Int displayLocation)
        {
            long timeRemaining = ExpireTime - CMain.Time;
            if (DamageLabel == null)
            {
                DamageLabel = GetComponent<Text>();
                DamageLabel.SetNativeSize();
                //DamageLabel.BackColour = Color.Transparent,
                //    ForeColour = Colour,
                //    OutLine = true,
                //    OutLineColour = Color.Black,
                //    Text = Text,
                //    Font = new Font(Settings.FontName, 8F, FontStyle.Bold);
                DamageLabel.text = Text;
            }

            displayLocation += new Vector3Int((int)(15 - (Text.Length * 3)), (int)(((int)((double)timeRemaining / Factor)) - Distance) - 75 - Offset);
            transform.position = displayLocation;
        }
    }

}
