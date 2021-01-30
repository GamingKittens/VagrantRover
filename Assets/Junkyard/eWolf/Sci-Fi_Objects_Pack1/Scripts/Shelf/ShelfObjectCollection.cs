using System.Collections.Generic;

namespace eWolf.SciFiObjects.Shelf
{
    [System.Serializable]
    public class ShelfObjectCollections
    {
        public bool Boxes = true;
        public bool Controllers = true;
        public bool Jars = true;
        public bool Junk = true;
        public bool Tools = true;

        private static List<ShelfObjects> _controllers = new List<ShelfObjects>()
        {
            new ShelfObjects("Controller_01_pf",0.2f),
            new ShelfObjects("Controller_02_pf",0.2f),
            new ShelfObjects("Controller_03_pf",0.3f),
            new ShelfObjects("Controller_04_pf",0.2f),
            new ShelfObjects("Controller_05_pf",0.2f),
            new ShelfObjects("Controller_06_pf",0.2f)
        };

        private static List<ShelfObjects> _jars = new List<ShelfObjects>()
        {
            new ShelfObjects("Specimen_Jar_01_pf",0.2f),
            new ShelfObjects("Specimen_Jar_02_pf",0.2f)
        };

        private static List<ShelfObjects> _junk = new List<ShelfObjects>()
        {
            new ShelfObjects("Metal_obj01_pf",0.2f),
            new ShelfObjects("Metal_Obj02_pf",0.3f),
            new ShelfObjects("Disk_pf",0.2f),
            new ShelfObjects("DiskGears_pf",0.25f),
            new ShelfObjects("Pipe_01_pf",0.2f),
            new ShelfObjects("Pipe_02_pf",0.15f),
            new ShelfObjects("Valve_01_pf", 0.35f),
            new ShelfObjects("Valve_02_pf", 0.35f),
            new ShelfObjects("Belt_01_pf", 0.35f)
        };

        private static List<ShelfObjects> _tools = new List<ShelfObjects>()
        {
            new ShelfObjects("Tool_01_pf",0.3f),
            new ShelfObjects("Tool_02_pf",0.3f),
            new ShelfObjects("Tool_03_pf",0.3f),
            new ShelfObjects("Binoculars_01_pf",0.2f),
            new ShelfObjects("Binoculars_02_pf",0.2f)
        };

        private List<ShelfObjects> _boxes = new List<ShelfObjects>()
        {
            new ShelfObjects("Object_01_pf",0.2f),
            new ShelfObjects("Object_02_pf",0.2f),
            new ShelfObjects("Object_03_pf",0.3f),
            new ShelfObjects("Object_04_pf",0.3f),
            new ShelfObjects("Object_05_pf",0.2f),
            new ShelfObjects("Object_06_pf",0.25f),
            new ShelfObjects("Object_07_pf",0.25f),
            new ShelfObjects("SmallBox_01_pf",0.3f),
            new ShelfObjects("SmallBox_02_pf",0.3f),
            new ShelfObjects("SmallPowerBox_pf",0.2f),
        };

        public List<ShelfObjects> GetObjects()
        {
            List<ShelfObjects> items = new List<ShelfObjects>();
            if (Jars)
                items.AddRange(_jars);

            if (Controllers)
                items.AddRange(_controllers);

            if (Junk)
                items.AddRange(_junk);

            if (Boxes)
                items.AddRange(_boxes);

            if (Tools)
                items.AddRange(_tools);

            return items;
        }
    }
}
