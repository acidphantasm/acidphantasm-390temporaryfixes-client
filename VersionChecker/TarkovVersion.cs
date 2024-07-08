using SPT.Common.Http;
using SPT.Common.Utils;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using Comfort.Net.Dispatching;

namespace acidphantasm_390temporaryfixes.VersionChecker
{
    internal class TarkovVersion
    {
        private int version;
        public TarkovVersion() : this(0) { }
        public TarkovVersion(int version)
        {
            this.version = version;
        }

        public static int BuildVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(TarkovVersion), false)
                    ?.Cast<TarkovVersion>()?.FirstOrDefault()?.version ?? 30626;
            }
        }
        public static bool CheckEftVersion(ManualLogSource Logger, PluginInfo Info, ConfigFile Config = null)
        {
            int currentVersion = FileVersionInfo.GetVersionInfo(BepInEx.Paths.ExecutablePath).FilePrivatePart;
            int buildVersion = BuildVersion;
            if (currentVersion != buildVersion)
            {
                string errorMessage = $"ERROR: This version of {Info.Metadata.Name} v{Info.Metadata.Version} was built for Tarkov {buildVersion}, but you are running {currentVersion}. Please download the correct plugin version.";
                Logger.LogError(errorMessage);

                return false;
            }

            // Because 3.5.7 and 3.5.8 are both 23399, but have different remappings, we need to do an extra check
            // here that the actual SPT version is what we expect. 
            // TODO: Once a new EFT version comes out, we can drop this
            if (currentVersion == 30626)
            {
                Version expectedSptVersion = new Version(3, 9, 0);
                Version sptVersion = GetSptVersion();
                if (sptVersion != expectedSptVersion)
                {
                    string errorMessage = $"ERROR: This version of {Info.Metadata.Name} v{Info.Metadata.Version} was built for SPT {expectedSptVersion}, but you are running {sptVersion}. Please download the correct plugin version.";
                    Logger.LogError(errorMessage);
                    return false;
                }
            }
            // TODO: Delete above when we have a new assembly version

            return true;
        }
        public static Version GetSptVersion()
        {
            var json = RequestHandler.GetJson("/singleplayer/settings/version");
            string version = Json.Deserialize<VersionResponse>(json).Version;
            version = Regex.Match(version, @"(\d+\.?)+").Value;

            Console.WriteLine($"SPT Version: {version}");
            return Version.Parse(version);
        }

        public struct VersionResponse
        {
            public string Version { get; set; }
        }
    }
}
