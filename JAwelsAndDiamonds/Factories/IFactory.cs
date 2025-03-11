namespace JAwelsAndDiamonds.Factories
{
    /// <summary>
    /// Generic factory interface that all factories should implement
    /// </summary>
    /// <typeparam name="T">Type of object to be created</typeparam>
    public interface IFactory<T>
    {
        /// <summary>
        /// Creates a new instance of type T
        /// </summary>
        /// <returns>A new instance of type T</returns>
        T Create();
    }
}