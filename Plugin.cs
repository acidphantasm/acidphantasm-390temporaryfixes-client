using BepInEx;
using acidphantasm_390temporaryfixes.Patches;
using acidphantasm_390temporaryfixes.VersionChecker;
using BepInEx.Bootstrap;
using System;

namespace acidphantasm_390temporaryfixes
{
    [BepInPlugin("phantasm.acid.390temporaryfixes", "acidphantasm-390temporaryfixes", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("[390 Temporary Fixes] loading...");

            if (!TarkovVersion.CheckEftVersion(Logger, Info, Config))
            {
                // TODO: Remove when a new EFT version comes out. For now, force remove
                //       ourselves from the list of plugins, so other plugins can know
                //       that we are not loaded
                Chainloader.PluginInfos.Remove(Info.Metadata.GUID);
                // End TODO

                throw new Exception($"Invalid EFT Version");
            }

            try
            {
                new MarkofTheUnknownPatch().Enable();
            }
            catch (Exception ex)
            {
                Logger.LogError($"{GetType().Name}: {ex}");
                throw;
            }

            Logger.LogInfo("[390 Temporary Fixes] loaded!");
        }
    }
}
