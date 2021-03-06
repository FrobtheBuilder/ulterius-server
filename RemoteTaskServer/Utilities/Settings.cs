﻿using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace RemoteTaskServer.Utilities
{
    class Settings
    {
        string _settingsPath;
        readonly string _programPath = Assembly.GetExecutingAssembly().GetName().Name;
        [DllImport("kernel32")]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string section, string key, string Default, StringBuilder retVal, int size, string filePath);

        public Settings(string iniPath = null)
        {
            _settingsPath = new FileInfo(iniPath ?? _programPath + ".ini").FullName.ToString();
        }

        public string Read(string key, string section = null)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section ?? _programPath, key, "", retVal, 255, _settingsPath);
            return retVal.ToString();
        }
        public void Write(string key, string value, string section = null)
        {
            WritePrivateProfileString(section ?? _programPath, key, value, _settingsPath);
        }

        public void DeleteKey(string key, string section = null)
        {
            Write(key, null, section ?? _programPath);
        }

        public void DeleteSection(string section = null)
        {
            Write(null, null, section ?? _programPath);
        }

        public bool KeyExists(string key, string section = null)
        {
            return Read(key, section).Length > 0;
        }
    }
}
