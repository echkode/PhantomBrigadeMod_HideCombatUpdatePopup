// Copyright (c) 2025 EchKode
// SPDX-License-Identifier: BSD-3-Clause

using System.Collections.Generic;

using HarmonyLib;

using PhantomBrigade.Data;

namespace EchKode.PBMods.HideCombatUpdatePopup
{
    public static class GameSettings
    {
        public static void AddDelegates()
        {
            var implementationsFieldInfo = AccessTools.DeclaredField(typeof(SettingUtility), "implementations");
            if (implementationsFieldInfo == null)
            {
                return;
            }
            var initializedFieldInfo = AccessTools.DeclaredField(typeof(SettingUtility), "initialized");
            if (initializedFieldInfo == null)
            {
                return;
            }
            if (!(bool)initializedFieldInfo.GetValue(null))
            {
                SettingUtility.Initialize();
            }

            var implementations = (Dictionary<string, SettingImplementationDelegate>)implementationsFieldInfo.GetValue(null);
            if (implementations == null)
            {
                return;
            }
            implementations[keyEnableUpdatePopup] = UpdatePopup;
        }

        static void UpdatePopup(DataContainerGameSetting definition, string valueRaw)
        {
            enableUpdatePopup = SettingUtility.TryParseBool(valueRaw);
        }

        public static bool enableUpdatePopup = true;

        // This is the key of the entry in the game settings config database.
        const string keyEnableUpdatePopup = "game_combat_update_popup";
    }
}
