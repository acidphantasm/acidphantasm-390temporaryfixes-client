﻿using BepInEx;
using acidphantasm_moretagcolours.Patches;

namespace acidphantasm_moretagcolours
{
    [BepInPlugin("phantasm.acid.moretagcolours", "acidphantasm-MoreTagColours", "1.0.0")]
    [BepInDependency("com.SPT.core", "3.9.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("[MoreTagColours] loading...");

            new GetColorPatch().Enable();
            new TagColorsPanelPatch().Enable();
            new ResizeWindowPatch().Enable();

            Logger.LogInfo("[MoreTagColours] loaded!");
        }
    }
}
