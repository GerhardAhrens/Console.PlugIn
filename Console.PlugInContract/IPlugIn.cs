namespace Console.PlugInContract
{
    /// <summary>
    /// The contract interface.
    /// Both, main application and modules are working against this interface.
    /// </summary>
    public interface IPlugIn
    {
        Guid Id { get; }

        Version Version { get; }

        Version ModulVersion { get; }

        string Description { get; }

        string ShortDescription { get; }

        Modulname Modul { get; }

        /// <summary>
        /// Calculates a value out of two given decimal values.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns></returns>
        decimal Calculate(decimal value1, decimal value2);
    }

    public enum Modulname
    {
        None = 0,
        Subtraction = 1,
        Addition = 3,
        Brutto = 4
    }
}
