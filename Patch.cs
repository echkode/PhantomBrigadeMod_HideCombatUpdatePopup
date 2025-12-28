// Copyright (c) 2025 EchKode
// SPDX-License-Identifier: BSD-3-Clause

using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using PhantomBrigade.Combat.Systems;

namespace EchKode.PBMods.HideCombatUpdatePopup
{
    [HarmonyPatch]
    static class Patch
    {
        [HarmonyPatch(typeof(CombatBootstrap), nameof(CombatBootstrap.Enable))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Cb_EnableTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            // Allow entry for log view based on setting.
            var loadInstanceMatch = new CodeMatch(CodeInstruction.LoadField(typeof(CIViewOverworldLog), nameof(CIViewOverworldLog.ins)));
            var dup = new CodeInstruction(OpCodes.Dup);
            var loadSetting = CodeInstruction.LoadField(typeof(GameSettings), nameof(GameSettings.enableUpdatePopup));
            var store = CodeInstruction.StoreField(typeof(CIViewOverworldLog), nameof(CIViewOverworldLog.entryAllowed));

            var cm = new CodeMatcher(instructions, generator);
            cm.Start();
            cm.MatchEndForward(loadInstanceMatch)
                .Advance(1)
                .InsertAndAdvance(dup)
                .InsertAndAdvance(loadSetting)
                .InsertAndAdvance(store);

            return cm.InstructionEnumeration();
        }

        [HarmonyPatch(typeof(TeardownCombatSystem), nameof(TeardownCombatSystem.TearDown))]
        [HarmonyPostfix]
        static void Tcs_TearDownPostfix()
        {
            CIViewOverworldLog.ins.entryAllowed = true;
        }
    }
}
