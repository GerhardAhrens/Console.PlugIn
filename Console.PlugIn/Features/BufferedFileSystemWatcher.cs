//-----------------------------------------------------------------------
// <copyright file="BufferedFileSystemWatcher.cs" company="Lifeprojects.de">
//     Class: BufferedFileSystemWatcher
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>09.06.2023</date>
//
// <summary>
// Class with BufferedFileSystemWatcher Definition
// </summary>
// <Remarks>
// Überarbeitung auf NET 10
// https://decatec.de/programmierung/filesystemwatcher-events-werden-mehrfach-ausgeloest-loesungsansaetze/
// </Remarks>
//-----------------------------------------------------------------------

namespace System.IO
{
    using System.Collections.Generic;
    using System.Timers;

    /// <summary>
    /// Erfasst Benachrichtigungen über Änderungen am Dateisystem und löst Ereignisse aus, wenn sich ein Verzeichnis oder eine Datei in einem Verzeichnis ändert.
    /// Die Ereignisse werden nicht sofort ausgelöst, sondern zwischengespeichert, sodass Ereignisse, die vom integrierten FileSystemWatcher zweimal ausgelöst werden, nur einmal ausgelöst werden.
    /// </summary>
    public class BufferedFileSystemWatcher : FileSystemWatcher
    {
        private const string StandardFilter = "*.*";
        private const double StandardBufferDelay = 400;

        private HashSet<string> changedList;
        private HashSet<string> createdList;
        private HashSet<string> deletedList;
        private HashSet<string> renamedList;

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse „BufferedFileSystemWatcher“.
        /// </summary>
        public BufferedFileSystemWatcher() : this(string.Empty, StandardFilter, StandardBufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse „BufferedFileSystemWatcher“.
        /// </summary>
        /// <param name="bufferedChangeTypes">Die „BufferedChangeTypes“, die festlegen, welche Ereignisse zwischengespeichert werden sollen.</param>
        public BufferedFileSystemWatcher(BufferedChangeTypes bufferedChangeTypes) : this(string.Empty, StandardFilter, StandardBufferDelay, bufferedChangeTypes)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse „BufferedFileSystemWatcher“ mit der angegebenen Pufferverzögerungszeit.
        /// </summary>
        /// <param name="bufferDelay">Die Zeit in Millisekunden, für die die ausgelösten Ereignisse zwischengespeichert werden sollen.</param>
        public BufferedFileSystemWatcher(long bufferDelay) : this(string.Empty, StandardFilter, bufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse „BufferedFileSystemWatcher“ mit der angegebenen Pufferverzögerungszeit.
        /// </summary>
        /// <param name="bufferDelay">Die Zeit in Millisekunden, für die die ausgelösten Ereignisse zwischengespeichert werden sollen.</param>
        /// <param name="bufferedChangeTypes">The “BufferedChangeTypes,” which determine which events should be buffered.</param>
        public BufferedFileSystemWatcher(long bufferDelay, BufferedChangeTypes bufferedChangeTypes) : this(string.Empty, StandardFilter, bufferDelay, bufferedChangeTypes)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse „BufferedFileSystemWatcher“ unter Angabe des zu überwachenden Verzeichnisses.
        /// </summary>
        /// <param name="path">Das zu überwachende Verzeichnis, in Standard- oder UNC-Notation (Universal Naming Convention).</param>
        public BufferedFileSystemWatcher(string path) : this(path, StandardFilter, StandardBufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse „BufferedFileSystemWatcher“ unter Angabe des zu überwachenden Verzeichnisses.
        /// </summary>
        /// <param name="path">Das zu überwachende Verzeichnis, in Standard- oder UNC-Notation (Universal Naming Convention).</param>
        /// <param name="bufferedChangeTypes">The BufferedChangeTypes specifying which events should be buffered.</param>
        public BufferedFileSystemWatcher(string path, BufferedChangeTypes bufferedChangeTypes) : this(path, StandardFilter, StandardBufferDelay, bufferedChangeTypes)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse „BufferedFileSystemWatcher“ unter Angabe des zu überwachenden Verzeichnisses.
        /// </summary>
        /// <param name="path">Das zu überwachende Verzeichnis, in Standard- oder UNC-Notation (Universal Naming Convention).</param>
        /// <param name="bufferDelay">Die Zeit in Millisekunden, für die die ausgelösten Ereignisse zwischengespeichert werden sollen.</param>
        public BufferedFileSystemWatcher(string path, long bufferDelay) : this(path, StandardFilter, bufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specified directory and type of files to monitor.
        /// </summary>
        /// <param name="path">Das zu überwachende Verzeichnis, in Standard- oder UNC-Notation (Universal Naming Convention).</param>
        /// <param name="filter">Die Dateitypen, die überwacht werden sollen. Beispielsweise überwacht „*.txt“ alle Textdateien auf Änderungen.</param>
        public BufferedFileSystemWatcher(string path, string filter) : this(path, filter, StandardBufferDelay, BufferedChangeTypes.All)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BufferedFileSystemWatcher class, given the specified directory and type of files to monitor.
        /// </summary>
        /// <param name="path">Das zu überwachende Verzeichnis, in Standard- oder UNC-Notation (Universal Naming Convention).</param>
        /// <param name="filter">Die Dateitypen, die überwacht werden sollen. Beispielsweise überwacht „*.txt“ alle Textdateien auf Änderungen.</param>
        /// <param name="bufferedChangeTypes">Die „BufferedChangeTypes“ legen fest, welche Ereignisse zwischengespeichert werden sollen.</param>
        public BufferedFileSystemWatcher(string path, string filter, BufferedChangeTypes bufferedChangeTypes) : this(path, filter, StandardBufferDelay, bufferedChangeTypes)
        {
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Klasse „BufferedFileSystemWatcher“ unter Angabe des angegebenen Verzeichnisses, 
        /// des zu überwachenden Dateityps, der Pufferverzögerungszeit und der Ereignistypen, die verzögert verarbeitet werden sollen.
        /// </summary>
        /// <param name="path">Das zu überwachende Verzeichnis, in Standard- oder UNC-Notation (Universal Naming Convention).</param>
        /// <param name="filter">Die Dateitypen, die überwacht werden sollen. Beispielsweise überwacht „*.txt“ alle Textdateien auf Änderungen.</param>
        /// <param name="bufferDelay">Die Zeit in Millisekunden, für die die ausgelösten Ereignisse zwischengespeichert werden sollen.</param>
        /// <param name="bufferedChangeTypes">Die „BufferedChangeTypes“ legen fest, welche Ereignisse zwischengespeichert werden sollen.</param>
        public BufferedFileSystemWatcher(string path, string filter, double bufferDelay, BufferedChangeTypes bufferedChangeTypes) : base(path, filter)
        {
            this.changedList = [];
            this.createdList = [];
            this.deletedList = [];
            this.renamedList = [];

            this.Path = path;
            this.Filter = filter;
            this.BufferDelay = bufferDelay;
            this.BufferedChangeTypes = bufferedChangeTypes;

            base.Changed += this.BufferedFileSystemWatcherChanged;
            base.Created += this.BufferedFileSystemWatcherCreated;
            base.Deleted += this.BufferedFileSystemWatcherDeleted;
            base.Renamed += this.BufferedFileSystemWatcherRenamed;
        }

        ~BufferedFileSystemWatcher()
        {
            this.changedList = null;
            this.createdList = null;
            this.deletedList = null;
            this.renamedList = null;

            base.Changed -= this.BufferedFileSystemWatcherChanged;
            base.Created -= this.BufferedFileSystemWatcherCreated;
            base.Deleted -= this.BufferedFileSystemWatcherDeleted;
            base.Renamed -= this.BufferedFileSystemWatcherRenamed;
        }

        /// <summary>
        /// Occurs when a file or directory in the specified Path is changed.
        /// </summary>
        public new event FileSystemEventHandler Changed;

        /// <summary>
        /// Tritt auf, wenn eine Datei oder ein Verzeichnis im angegebenen Pfad erstellt wird.
        /// </summary>
        public new event FileSystemEventHandler Created;

        /// <summary>
        /// Tritt auf, wenn eine Datei oder ein Verzeichnis im angegebenen Pfad gelöscht wird.
        /// </summary>
        public new event FileSystemEventHandler Deleted;

        /// <summary>
        /// Tritt auf, wenn eine Datei oder ein Verzeichnis im angegebenen Pfad umbenannt wird.
        /// </summary>
        public new event FileSystemEventHandler Renamed;

        /// <summary>
        /// Ruft die Pufferverzögerungszeit in Millisekunden ab oder legt sie fest, d. h. die Zeit, für die die ausgelösten Ereignisse gepuffert werden sollen.
        /// </summary>
        public void SetBufferDelay(double bufferDelay = StandardBufferDelay)
        {
            this.BufferDelay = bufferDelay;
        }

        private double BufferDelay { get; set; }

        /// <summary>
        /// Ruft die „BufferedChangeTypes“ ab oder legt sie fest, d. h. die Arten von Ereignissen, die vom „BufferedFileSystemWatcher“ zwischengespeichert werden sollen.
        /// Wenn diese Eigenschaft auf „DelayedChangeTypes.None“ gesetzt ist, verhält sich der „BufferedFileSystemWatcher“ wie ein normaler „FileSystemWatcher“.
        /// </summary>
        public void SetBufferedChangeTypes(BufferedChangeTypes bufferedChangeTypes)
        {
            this.BufferedChangeTypes = bufferedChangeTypes;
        }

        private BufferedChangeTypes BufferedChangeTypes { get; set; }

        public void StopFileSystemWatcher()
        {
            if (this.EnableRaisingEvents == true)
            {
                this.EnableRaisingEvents = false;
            }
        }

        public void StartFileSystemWatcher()
        {
            if (this.EnableRaisingEvents == false)
            {
                this.EnableRaisingEvents = true;
            }
        }

        public (string,double,bool) GetFileSystemWatcherInfo()
        {
            return ($"{this.BufferedChangeTypes}", this.BufferDelay, this.EnableRaisingEvents);
        }

        /// <summary>
        /// Wird aufgerufen, sobald eine Datei oder ein Verzeichnis im angegebenen Pfad geändert wird (zwischengespeichert).
        /// </summary>
        /// <param name="e">The FileSystemEventArgs that contains the event data.</param>
        protected virtual void OnBufferedChanged(FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;
            Timer timer = new Timer(this.BufferDelay) { AutoReset = false };

            try
            {
                lock (this.changedList)
                {
                    if (this.changedList.Contains(fullPath) == false)
                    {
                        this.changedList.Add(fullPath);
                    }
                    else
                    {
                        return;
                    }
                }

                timer.Elapsed += (object elapsedSender, ElapsedEventArgs elapsedArgs) =>
                {
                    FileSystemEventHandler tmp = this.Changed;

                    if (this.changedList.Contains(fullPath) == true)
                    {
                        if (tmp != null)
                        {
                            tmp(this, e);
                        }
                    }

                    lock (this.changedList)
                    {
                        if (this.changedList.Contains(fullPath) == true)
                        {
                            this.changedList.Remove(fullPath);
                        }
                    }
                };

                timer.Start();
            }
            catch (IOException ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        /// <summary>
        ///  Wird immer dann aufgerufen, wenn eine Datei oder ein Verzeichnis im angegebenen Pfad geändert wird (nicht zwischengespeichert).
        /// </summary>
        /// <param name="e">Das `FileSystemEventArgs`-Objekt, das die Ereignisdaten enthält.</param>
        protected virtual new void OnChanged(FileSystemEventArgs e)
        {
            FileSystemEventHandler tmp = this.Changed;

            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        /// <summary>
        /// Wird ausgelöst, sobald eine Datei oder ein Verzeichnis im angegebenen Pfad erstellt wird (gepuffert).
        /// </summary>
        /// <param name="e">Das `FileSystemEventArgs`-Objekt, das die Ereignisdaten enthält.</param>
        protected virtual void OnBufferedCreated(FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;
            Timer timer = new Timer(this.BufferDelay) { AutoReset = false };

            try
            {
                lock (this.createdList)
                {
                    if (this.createdList.Contains(fullPath) == false)
                    {
                        this.createdList.Add(fullPath);
                    }
                }

                timer.Elapsed += (object elapsedSender, ElapsedEventArgs elapsedArgs) =>
                {
                    FileSystemEventHandler tmp = this.Created;

                    if (tmp != null)
                    {
                        tmp(this, e);
                    }

                    lock (this.createdList)
                    {
                        if (this.createdList.Contains(fullPath) == true)
                        {
                            this.createdList.Remove(fullPath);
                        }
                    }
                };

                timer.Start();
            }
            catch (IOException ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Wird ausgelöst, sobald eine Datei oder ein Verzeichnis im angegebenen Pfad erstellt wird (nicht gepuffert).
        /// </summary>
        /// <param name="e">Das `FileSystemEventArgs`-Objekt, das die Ereignisdaten enthält.</param>
        protected virtual new void OnCreated(FileSystemEventArgs e)
        {
            FileSystemEventHandler tmp = this.Created;

            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn eine Datei oder ein Verzeichnis im angegebenen Pfad gelöscht wird (gepuffert).
        /// </summary>
        /// <param name="e">Das `FileSystemEventArgs`-Objekt, das die Ereignisdaten enthält.</param>
        protected virtual void OnBufferedDeleted(FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;
            var timer = new Timer(this.BufferDelay) { AutoReset = false };

            try
            {
                lock (this.deletedList)
                {
                    if (this.deletedList.Contains(fullPath) == false)
                    {
                        this.deletedList.Add(fullPath);
                    }
                }

                timer.Elapsed += (object elapsedSender, ElapsedEventArgs elapsedArgs) =>
                {
                    FileSystemEventHandler tmp = this.Deleted;

                    if (tmp != null)
                    {
                        tmp(this, e);
                    }

                    lock (this.deletedList)
                    {
                        if (this.deletedList.Contains(fullPath) == true)
                        {
                            this.deletedList.Remove(fullPath);
                        }
                    }
                };

                timer.Start();
            }
            catch (IOException ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Wird immer dann aufgerufen, wenn eine Datei oder ein Verzeichnis im angegebenen Pfad gelöscht wird (nicht zwischengespeichert).
        /// </summary>
        /// <param name="e">Das `FileSystemEventArgs`-Objekt, das die Ereignisdaten enthält.</param>
        protected virtual new void OnDeleted(FileSystemEventArgs e)
        {
            FileSystemEventHandler tmp = this.Deleted;

            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        /// <summary>
        /// Wird aufgerufen, sobald eine Datei oder ein Verzeichnis im angegebenen Pfad umbenannt wird (zwischengespeichert).
        /// </summary>
        /// <param name="e">Das `FileSystemEventArgs`-Objekt, das die Ereignisdaten enthält.</param>
        protected virtual void OnBufferedRenamed(RenamedEventArgs e)
        {
            string fullPath = e.FullPath;
            Timer timer = new Timer(this.BufferDelay) { AutoReset = false };

            try
            {
                lock (this.renamedList)
                {
                    if (this.renamedList.Contains(fullPath) == false)
                    {
                        this.renamedList.Add(fullPath);
                    }
                }

                timer.Elapsed += (object elapsedSender, ElapsedEventArgs elapsedArgs) =>
                {
                    FileSystemEventHandler tmp = this.Renamed;

                    if (tmp != null)
                    {
                        tmp(this, e);
                    }

                    lock (this.renamedList)
                    {
                        if (this.renamedList.Contains(fullPath) == true)
                        {
                            this.renamedList.Remove(fullPath);
                        }
                    }
                };

                timer.Start();
            }
            catch (IOException ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Wird aufgerufen, sobald eine Datei oder ein Verzeichnis im angegebenen Pfad umbenannt wird (nicht zwischengespeichert).
        /// </summary>
        /// <param name="e">Das `FileSystemEventArgs`-Objekt, das die Ereignisdaten enthält.</param>
        protected virtual new void OnRenamed(RenamedEventArgs e)
        {
            FileSystemEventHandler tmp = this.Renamed;

            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        private void BufferedFileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            if ((this.BufferedChangeTypes & BufferedChangeTypes.Changed) == BufferedChangeTypes.Changed)
            {
                this.OnBufferedChanged(e);
            }
            else
            {
                if (this.BufferedChangeTypes == BufferedChangeTypes.None)
                {
                    this.OnChanged(e);
                }
            }
        }

        private void BufferedFileSystemWatcherCreated(object sender, FileSystemEventArgs e)
        {
            if ((this.BufferedChangeTypes & BufferedChangeTypes.Created) == BufferedChangeTypes.Created)
            {
                this.OnBufferedCreated(e);
            }
            else
            {
                if (this.BufferedChangeTypes == BufferedChangeTypes.None)
                {
                    this.OnCreated(e);
                }
            }
        }

        private void BufferedFileSystemWatcherDeleted(object sender, FileSystemEventArgs e)
        {
            if ((this.BufferedChangeTypes & BufferedChangeTypes.Deleted) == BufferedChangeTypes.Deleted)
            {
                this.OnBufferedDeleted(e);
            }
            else
            {
                if (this.BufferedChangeTypes == BufferedChangeTypes.None)
                {
                    this.OnDeleted(e);
                }
            }
        }

        private void BufferedFileSystemWatcherRenamed(object sender, RenamedEventArgs e)
        {
            if ((this.BufferedChangeTypes & BufferedChangeTypes.Renamed) == BufferedChangeTypes.Renamed)
            {
                this.OnBufferedRenamed(e);
            }
            else
            {
                if (this.BufferedChangeTypes == BufferedChangeTypes.None)
                {
                    this.OnRenamed(e);
                }
            }
        }
    }
}
