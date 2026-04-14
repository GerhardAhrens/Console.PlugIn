namespace ExampleCalculatorPlus
{
    using Console.PlugInContract;

    public abstract class PlugInInfo : IPlugIn
    {
        public Guid Id
        {
            get { return new Guid("155ACEC2-A782-49AD-B246-3E62442B62DA"); }
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
            get { return "Addition"; }
        }

        public string Description
        {
            get { return "Zwei Werte addieren"; }
        }

        public Modulname Modul
        {
            get { return Modulname.Addition; }
        }

        public abstract decimal Calculate(decimal value1, decimal value2);
    }

    public class CalcPlus : PlugInInfo
    {
        public override decimal Calculate(decimal value1, decimal value2)
        {
            return value1 + value2;
        }
    }
}
