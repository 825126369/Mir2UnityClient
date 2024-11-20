using CrystalMir2;

namespace Mir2
{
    public class MapJsonClass
    {
        public int Width, Height;
        public OkCell[,] Cells;
    }

    public class OkCell
    {
        public CellAttribute Attribute;
        public sbyte FishingAttribute = -1;
    }

}
