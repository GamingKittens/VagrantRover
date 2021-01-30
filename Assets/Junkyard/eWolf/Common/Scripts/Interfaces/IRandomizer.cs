/// <summary>
/// Used by Sci-Fi_Objects_Pack1
/// </summary>
namespace eWolf.Common.Interfaces
{
    public interface IRandomizer
    {
        bool IsLocked { get; }

        void Randomize();

        void RandomizeVisual();
    }
}
