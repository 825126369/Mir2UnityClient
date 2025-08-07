using UnityEngine;
using UnityEngine.UI;

namespace Mir2
{
    public class HeroObject : PlayerObject
    {

        public override ObjectType Race
        {
            get { return ObjectType.Hero; }
        }

        public string OwnerName;
        public Text OwnerLabel;

        public override bool ShouldDrawHealth()
        {
            //if (GroupDialog.GroupList.Contains(OwnerName) || OwnerName == User.Name)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        public HeroObject(uint objectID) : base(objectID)
        {
        }

        //public void Load(S.ObjectHero info)
        //{
        //    Load((S.ObjectPlayer)info);
        //    OwnerName = info.OwnerName;

        //    if (info.ObjectID == Hero?.ObjectID)
        //        Hero.CurrentLocation = info.Location;
        //}

        public override void CreateLabel()
        {
            base.CreateLabel();

            OwnerLabel = null;
            string ownerText = $"{OwnerName}'s Hero";

            for (int i = 0; i < LabelList.Count; i++)
            {
                if (LabelList[i].text != ownerText || LabelList[i].color != NameColour) continue;
                OwnerLabel = LabelList[i];
                break;
            }

            if (OwnerLabel != null && !OwnerLabel.isActiveAndEnabled) return;
            

                //OwnerLabel. AutoSize = true,
                //BackColour = Color.Transparent,
                //ForeColour = NameColour,
                //OutLine = true,
                //OutLineColour = Color.Black,
                //Text = ownerText,
            
            //OwnerLabel.Disposing += (o, e) => LabelList.Remove(OwnerLabel);
            LabelList.Add(OwnerLabel);
        }

        public override void DrawName()
        {
            CreateLabel();
            if (NameLabel == null || OwnerLabel == null) return;
            NameLabel.transform.position = new Vector3(DisplayRectangle.x + (50 - NameLabel.rectTransform.sizeDelta.x) / 2, DisplayRectangle.y - (42 - NameLabel.rectTransform.sizeDelta.y / 2) + (Dead ? 35 : 8));
            OwnerLabel.transform.position = new Vector3(DisplayRectangle.x + (50 - OwnerLabel.rectTransform.sizeDelta.x) / 2, NameLabel.transform.position.y + NameLabel.rectTransform.sizeDelta.y - 1);
        }

    }
}
