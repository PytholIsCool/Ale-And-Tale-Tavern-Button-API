using ButtonAPI.GameAPI;
using ButtonAPI.Types;
using UnityEngine;
using Object = UnityEngine.Object;
using ButtonAPI.MainMenuAPI;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Localization.Components;
using ButtonAPI.Uni.TabControls;
using ButtonAPI.Uni.Main;
using UnityEngine.SceneManagement;
using PyAPI.ButtonAPI.Uni.TabControls;

namespace ButtonAPI.Uni {
    public class Tab : Root {
        private static Transform currentConRef;
        public Transform Container { get; internal set; }
        public List<Tab> Tabs { get; internal set; }
        public Page ParentPage { get; internal set; }
        public Tab(Page page, string text) {
            ParentPage = page;

            Transform reference = SceneManager.GetActiveScene().name == "MM3" ? RefSetMain() : RefSetGame();

            (transform = (gameObject = Object.Instantiate(reference, page.TabParent).gameObject).transform).name = $"PyTab_{text}";

            (Container = Object.Instantiate(currentConRef, page.Contents)).name = $"PyTabContainer_{text}";
            Container.DestroyChildren();

            Object.Destroy(transform.Find("Text (TMP)").GetComponent<LocalizeStringEvent>());
            (TMProCompnt = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>()).text = text;
            TMProCompnt.richText = true;

            (ButtonCompnt = transform.GetComponent<Button>()).onClick.RemoveAllListeners();
            (ButtonCompnt.onClick = new Button.ButtonClickedEvent()).AddListener(delegate { OpenContents(); });

            (Tabs = page.Tabs).Add(this);
        }

        public Selector AddSelector(string name) => new Selector(this, name);
        public SliderControl AddSlider(string text, UnityAction<float> listener, float defaultValue = 0f, float minValue = 0f, float maxValue = 100f, bool isDecimal = false) => new SliderControl(this, text, listener, defaultValue, minValue, maxValue, isDecimal);
        public ToggleControl AddToggle(string text, UnityAction<bool> listener, bool defaultState = false) => new ToggleControl(this, text, listener, defaultState);
        public ButtonControl AddButton(string text, string buttonText, UnityAction listener) => new ButtonControl(this, text, buttonText, listener);

        public void OpenContents(bool silent = false) {
            if (this == ParentPage.CurrentTab)
                return;
            foreach (Tab tab in Tabs) {
                tab.Container.gameObject.SetActive(false);
                tab.TMProCompnt.color = new Color(0.4745f, 0.4314f, 0.4392f, 1f);
            }
            this.Container.gameObject.SetActive(true);
            this.TMProCompnt.color = new Color(0.9647f, 0.8824f, 0.6118f, 1f);

            if (silent == false)
                FmodManager.Instance.Play(SoundEvent.Page);
            ParentPage.CurrentTab = this;
        }
        private static Transform RefSetMain() {
            currentConRef = MainMenuAPIBase.Settings.Find("BG/Video");
            return MainMenuAPIBase.TabTemplate;
        }
        private static Transform RefSetGame() {
            currentConRef = GameAPIBase.Settings.Find("BG/Video");
            return GameAPIBase.TabTemplate;
        }
    }
}
