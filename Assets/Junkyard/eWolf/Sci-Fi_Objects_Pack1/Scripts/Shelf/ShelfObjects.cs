namespace eWolf.SciFiObjects.Shelf
{
    public class ShelfObjects
    {
        public string Name { get; set; }
        public float Size { get; set; }
        public bool Rotate { get; set; }

        public ShelfObjects(string name, float size)
        {
            Name = name;
            Size = size;
        }

        public ShelfObjects(string name, float size, bool rot)
        {
            Name = name;
            Size = size;
            Rotate = rot;
        }
    }
}