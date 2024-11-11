using ButtonAPI.Uni.Main;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ButtonAPI.Uni {
    public class Patches {
        public static bool arePatchesApplied;
        public static void InitializePatches() {
            HarmonyLib.Tools.Logger.ChannelFilter = HarmonyLib.Tools.Logger.LogChannel.All;
            HarmonyLib.Tools.HarmonyFileLog.Enabled = true;
            var harmony = new HarmonyLib.Harmony("com.Pythol.AATPyAPI");

            var settingsOnUpdate = AccessTools.Method(typeof(AppSettingsMenu), "Update");
            var aviOnUpdate = AccessTools.Method(typeof(AvatarUI), "Update");
            var gameOnUpdate = AccessTools.Method(typeof(GameMenu), "Update");

            harmony.Patch(settingsOnUpdate, new HarmonyMethod(typeof(Patches), nameof(SettingsOnUpdate)));
            harmony.Patch(aviOnUpdate, new HarmonyMethod(typeof(Patches), nameof(AviOnUpdate)));
            harmony.Patch(gameOnUpdate, new HarmonyMethod(typeof(Patches), nameof(GameOnUpdate)));

            arePatchesApplied = true;
        }
        private static bool EscapeCheck() {
            if (Popup.isPopupActive)
                return false;
            if (Page.isPageOpen == true && Input.GetKeyDown(KeyCode.Escape)) {
                if (SceneManager.GetActiveScene().name == "MM3")
                    Page.CurrentPage.OnBack();
                else
                    Page.CurrentPage.OnBack(true);
                return false;
            }
            return true;
        }
        //Main
        private static bool SettingsOnUpdate(AppSettingsMenu __instance) {
            return EscapeCheck();
        }
        private static bool AviOnUpdate(AvatarUI __instance) {
            return EscapeCheck();
        }
        //Game
        private static bool GameOnUpdate(GameMenu __instance) {
            return EscapeCheck();
        }
    }
}
