using ButtonAPI.GameAPI;
using ButtonAPI.MainMenuAPI;
using ButtonAPI.Types;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ButtonAPI.Uni.TabControls {
    public class ToggleControl : Root {
        public Toggle ToggleCompnt { get; private set; }
        public UnityAction<bool> TListener { get; internal set; }

        private bool shouldInvoke = true;
        public ToggleControl(Transform parent, string text, UnityAction<bool> listener, bool defaultState = false) {
            Transform reference = SceneManager.GetActiveScene().name == "MM3" ? MainMenuAPIBase.ToggleTemplate : GameAPIBase.ToggleTemplate;

            (transform = (gameObject = Object.Instantiate(reference, parent).gameObject).transform).name = $"PyToggle_{text}";
            
            (TMProCompnt = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>()).text = text;

            (ToggleCompnt = transform.Find("Toggle").GetComponent<Toggle>()).onValueChanged.RemoveAllListeners();
            (ToggleCompnt.onValueChanged = new Toggle.ToggleEvent()).AddListener(new UnityAction<bool>((val) => {
                if (shouldInvoke)
                    (TListener = listener).Invoke(val);
            }));
            ToggleCompnt.isOn = defaultState;
        }
        public void SoftSetState(bool state) {
            shouldInvoke = false;
            ToggleCompnt.isOn = state;
            shouldInvoke = true;
        }

        public ToggleControl(Tab tab, string text, UnityAction<bool> listener, bool defaultState = false)
            : this(tab.Container, text, listener, defaultState) { }
    }
}
