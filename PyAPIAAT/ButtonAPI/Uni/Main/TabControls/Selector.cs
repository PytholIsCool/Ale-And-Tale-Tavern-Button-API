using ButtonAPI.GameAPI;
using ButtonAPI.MainMenuAPI;
using ButtonAPI.Types;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ButtonAPI.Uni.TabControls {
    public class Selector : Root {
        public TextMeshProUGUI TMProSelectorLabel { get; internal set; }
        public Button LeftScroll { get; internal set; }
        public Button RightScroll { get; internal set; }

        private List<Setting> Settings { get; set; }
        private int CurrentIndex { get; set; }
        public Selector(Transform parent, string text) {
            Transform reference = SceneManager.GetActiveScene().name == "MM3" ? MainMenuAPIBase.SelectorTemplate : GameAPIBase.SelectorTemplate;

            (transform = (gameObject = Object.Instantiate(reference, parent).gameObject).transform).name = $"PySelector_{text}";
            Object.Destroy(transform.Find("Text (TMP)").GetComponent<LocalizeStringEvent>());
            (TMProCompnt = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>()).text = text;
            TMProCompnt.richText = true;

            transform.Find("CarouselList").GetComponent<CarouselList>().ClearOptions();

            (TMProSelectorLabel = transform.Find("CarouselList/Label").GetComponent<TextMeshProUGUI>()).text = "Placeholder";
            TMProSelectorLabel.richText = true;
            (LeftScroll = transform.Find("CarouselList/ButtonL").GetComponent<Button>()).onClick.RemoveAllListeners();
            (LeftScroll.onClick = new Button.ButtonClickedEvent()).AddListener(delegate { ScrollLeft(); });
            (RightScroll = transform.Find("CarouselList/ButtonR").GetComponent<Button>()).onClick.RemoveAllListeners();
            (RightScroll.onClick = new Button.ButtonClickedEvent()).AddListener(delegate { ScrollRight(); });

            Settings = new List<Setting>();
            CurrentIndex = 0;
        }

        public Selector AddSetting(string name, UnityAction listener, bool invokeOnInit = true) {
            Settings.Add(new Setting { Name = name, Listener = listener });
            if (Settings.Count == 1) {
                UpdateDisplayedSetting(0);
                TryInvoke(0, invokeOnInit);
            }
            return this;
        }
        public Selector RemoveSetting(string name, bool invokeOnRemove = false) {
            int i = Settings.FindIndex(s => s.Name == name);

            if (i == -1)
                throw new System.NullReferenceException($"PyAPI Error: Setting \"{name}\" Not Found Or Does Not Exist.\nClass: {GetType().Name}");
            TryInvoke(CurrentIndex, invokeOnRemove);
            Settings.RemoveAt(i);

            if (Settings.Count > 0) {
                if (CurrentIndex >= i) {
                    CurrentIndex = Mathf.Clamp(CurrentIndex, 0, Settings.Count - 1);
                }
                UpdateDisplayedSetting(CurrentIndex);
            } else {
                CurrentIndex = 0;
                TMProSelectorLabel.text = "N/A";
            }

            return this;
        }
        public Selector SwitchToSetting(string name, bool invokeOnSwitch = true) {
            int i = Settings.FindIndex(s => s.Name == name);

            if (i == -1)
                throw new System.NullReferenceException($"PyAPI Error: Setting \"{name}\" Not Found Or Does Not Exist.\nClass: {GetType().Name}");
            else {
                CurrentIndex = i;
                UpdateDisplayedSetting(CurrentIndex);
                TryInvoke(CurrentIndex, invokeOnSwitch);
            }
            return this;
        }
        public Selector ClearSettings() {
            Settings.Clear();
            TMProSelectorLabel.text = "N/A";
            return this;
        }
        private void ScrollLeft() {
            if (Settings.Count == 0) 
                return;
            CurrentIndex = (CurrentIndex - 1 + Settings.Count) % Settings.Count;
            UpdateDisplayedSetting(CurrentIndex);
            TryInvoke(CurrentIndex, true);
        }

        private void ScrollRight() {
            if (Settings.Count == 0) return;
            CurrentIndex = (CurrentIndex + 1) % Settings.Count;
            UpdateDisplayedSetting(CurrentIndex);
            TryInvoke(CurrentIndex, true);
        }

        private void UpdateDisplayedSetting(int index) {
            TMProSelectorLabel.text = Settings[index].Name;
        }
        private void TryInvoke(int index, bool i) {
            if (i == true) {
                var setting = Settings[index];
                setting.Listener?.Invoke();
            }
        }

        private class Setting {
            public string Name { get; set; }
            public UnityAction Listener { get; set; }
        }
        public Selector(Tab tab, string name) 
            : this(tab.Container, name) { }
    }
}