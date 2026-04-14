/*
 * <copyright file="CalcBrutto.cs" company="Lifeprojects.de">
 *     Class: CalcBrutto
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>11.10.2024 19:07:23</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace PlugIn
{
    using Console.PlugInContract;

    public class CalcBrutto : IPlugIn
    {

        public Guid Id
        {
            get { return new Guid("B1F8C9D2-3E4A-4F5B-9A6C-7D8E9F0A1B2C"); }
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
            get { return "Bruttobetrag berechnen"; }
        }

        public string Description
        {
            get { return "Bruttobetrag, auf einen Betrag + einem Prozentsatz berechnen"; }
        }

        public Modulname Modul
        {
            get { return Modulname.Brutto; }
        }

        public decimal Calculate(decimal basisBetrag, decimal prozentSatz)
        {
            decimal steuerBetrag = basisBetrag * (prozentSatz / 100);
            return basisBetrag + steuerBetrag; 
        }
    }
}
