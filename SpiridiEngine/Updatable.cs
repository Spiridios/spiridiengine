namespace Spiridios.SpiridiEngine
{
    /// <summary>
    /// Interface for things that are updatable
    /// </summary>
    public interface Updatable
    {
        void Update(System.TimeSpan elapsedTime);
    }
}
