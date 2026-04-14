namespace ExampleCalculatorMinus
{
    using Console.PlugInContract;

    public abstract class PlugInInfo : IPlugIn
    {
        public Guid Id
        {
            get { return new Guid("{C8F18FCF-BAE4-4721-9F13-54721D168243}"); }
        }

        public Version Version
        {
            get
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                if (string.IsNullOrWhiteSpace(fvi.FileVersion))
                {
                    return assembly.GetName().Version ?? new Version(1, 0, DateTime.Now.Year, 0);
                }

                if (Version.TryParse(fvi.FileVersion, out var parsed))
                {
                    return parsed;
                }

                return assembly.GetName().Version ?? new Version(1, 0, DateTime.Now.Year, 0);
            }
        }

        public Version ModulVersion
        {
            get { return new Version(1, 1, 0); }
        }

        public string ShortDescription
        {
            get { return "Subtraction"; }
        }

        public string Description
        {
            get { return "Zwei Werte von einander subtrahieren"; }
        }

        public Modulname Modul
        {
            get { return Modulname.Subtraction; }
        }

        public abstract decimal Calculate(decimal value1, decimal value2);
    }

    public class CalcMinus : PlugInInfo
    {
        public override decimal Calculate(decimal value1, decimal value2)
        {
            return value1 - value2;
        }
    }
}
