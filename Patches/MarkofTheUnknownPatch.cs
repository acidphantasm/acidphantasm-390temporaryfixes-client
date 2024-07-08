﻿using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace acidphantasm_390temporaryfixes.Patches
{
    /// <summary>
    /// If Scav war is turned on Botsgroup can be null for some reason if null return early to not softlock player.
    /// </summary>
    public class MarkofTheUnknownPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BotsGroupMarkOfUnknown), nameof(BotsGroupMarkOfUnknown.Dispose));
        }
        [PatchPrefix]
        private static bool PatchPrefix(BotsGroup ____groups)
        {
            if (____groups == null)
            {
                return false;
            }
            return true;
        }
    }
}