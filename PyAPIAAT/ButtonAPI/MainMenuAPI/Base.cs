using ButtonAPI.Uni;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ButtonAPI.MainMenuAPI {
    public class MainMenuAPIBase {
        public static MainMenu MainMenuCompnt;

        public static Transform MainMenu, Home, Settings;

        public static Transform PanelButtonTemplate, TabTemplate, SelectorTemplate, SliderTemplate, ToggleTemplate, ButtonTemplate;
        public static bool IsReady() {
            if (SceneManager.GetActiveScene().name != "MM3")
                return false;

            if ((MainMenu = (MainMenuCompnt = Object.FindObjectOfType<MainMenu>()).transform) == null) throw new NullReferenceException("PyAPI Fatal Error: \"MainMenu\" reference is null \nClass: MainMenuAPIBase"); //It's honestly stupid that melonlaoder doesnt let you use exceptions
            if ((Home = MainMenu?.Find("Screen Home")) == null) throw new NullReferenceException("PyAPI Error: \"Home\" reference is null \nClass: MainMenuAPIBase");
            if ((Settings = MainMenu?.Find("AppSettingsMenu")) == null) throw new NullReferenceException("PyAPI Error: \"Settings\" reference is null \nClass: MainMenuAPIBase");
            //Controls
            if ((PanelButtonTemplate = Home?.Find("Panel/Button Settings")) == null) throw new NullReferenceException("PyAPI Error: \"HomeButtonTemplate\" reference is null \nClass: MainMenuAPIBase");
            if ((TabTemplate = Settings?.Find("BG/Tab_Panel/Button Video")) == null) throw new NullReferenceException("PyAPI Error: \"TabTemplate\" reference is null \nClass: MainMenuAPIBase");
            if ((SelectorTemplate = Settings?.Find("BG/Video/Res")) == null) throw new NullReferenceException("PyAPI Error: \"SelectorTemplate\" reference is null \nClass: MainMenuAPIBase");
            if ((SliderTemplate = Settings?.Find("BG/Video/FOV")) == null) throw new NullReferenceException("PyAPI Error: \"SliderTemplate\" reference is null \nClass: MainMenuAPIBase");
            if ((ToggleTemplate = Settings?.Find("BG/Video/VSync")) == null) throw new NullReferenceException("PyAPI Error: \"ToggleTemplate\" reference is null \nClass: MainMenuAPIBase");

            return true;
        }
        /// <summary>
        /// Checks to make sure all references are available.
        /// </summary>
        /// <returns></returns>
        public static IEnumerator Primer(Action onComplete = null) {
            while (!IsReady())
                yield return null;
            if (!Patches.arePatchesApplied)
                Patches.InitializePatches();

            onComplete?.Invoke();
        }
    }
}