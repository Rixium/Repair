namespace Repair.Input
{
    public interface IInput
    {
        string Name { get; set; }
        void Update(float delta);
    }
}