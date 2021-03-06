﻿using System;
using System.IO;

namespace Simple.Config
{
    public class XmlFileConfigSource<T> :
        XmlConfigSource<T>,
        IXmlFileConfigSource<T>,
        IConfigSource<T, XPathParameter<FileInfo>>
    {
        public XPathParameter<FileInfo> XmlFile { get; set; }
        protected FileSystemWatcher Watcher { get; set; }

        public bool Active { get; protected set; }
        public DateTime LastModification { get; protected set; }

        private object _lock = new object();

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                if (Active)
                {
                    Simply.Do.Log(this).DebugFormat("The watch in file {0} has raised.", XmlFile.Parameter.Name);
                    InvokeReload();
                }
            }
        }

        protected virtual void SetXmlFileInfo(XPathParameter<FileInfo> info)
        {
            XmlFile = info;

            if (Watcher != null) Watcher.Dispose();

            Watcher = new FileSystemWatcher(XmlFile.Parameter.DirectoryName, XmlFile.Parameter.Name);
            Watcher.Changed += Watcher_Changed;
            Watcher.Created += Watcher_Changed;
            Watcher.Deleted += Watcher_Changed;
            Watcher.EnableRaisingEvents = true;

            Active = true;
        }

        public virtual IConfigSource<T> LoadFile(string fileName)
        {
            return LoadFile(fileName, null);
        }

        public virtual IConfigSource<T> LoadFile(string fileName, string xPath)
        {
            return Load(new XPathParameter<FileInfo>(new FileInfo(fileName), xPath));
        }

        public override bool Reload()
        {
            lock (_lock)
            {
                if (XmlFile.Parameter == null)
                    throw new InvalidOperationException("Cannot reload a non-loaded source");

                Simply.Do.Log(this).DebugFormat("Reloading file {0}...", XmlFile.Parameter.Name);

                try
                {
                    Load(XmlFile);
                    return true;
                }
                catch (IOException)
                {
                    return false;
                }
            }
        }

        public override void Dispose()
        {
            Simply.Do.Log(this).DebugFormat("Disposing configurator for {0}...", typeof(T));
            lock (_lock)
                Active = false;

            Watcher.Dispose();
            base.Dispose();
        }

        #region IConfigSource<T,XPathParameter<FileInfo>> Members

        public IConfigSource<T> Load(XPathParameter<FileInfo> input)
        {
            lock (_lock)
            {
                Simply.Do.Log(this).DebugFormat("Loading XMLConfig for class {0}...", typeof(T).Name);

                SetXmlFileInfo(input.Parameter);

                using (Stream s = XmlFile.Parameter.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var ret = Load(XmlFile.ToOther(s));
                    s.Close();
                    return ret;
                }
            }
        }

        #endregion
    }
}
