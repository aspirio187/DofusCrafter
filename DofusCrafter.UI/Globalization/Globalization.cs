﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DofusCrafter.UI.Globalization
{
    public static class Globalization
    {
        private static ResourceManager? _rm;

        static Globalization()
        {
            Initialize(typeof(Globalization).Namespace + ".strings");
        }

        private static void Initialize(string resourceFileName)
        {
            _rm = new ResourceManager(resourceFileName, Assembly.GetExecutingAssembly());
        }

        public static string? GetString(string name)
        {
            return _rm?.GetString(name);
        }

        public static void ChangeLanguage(string language)
        {
            var cultureInfo = new CultureInfo(language);

            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }
    }
}