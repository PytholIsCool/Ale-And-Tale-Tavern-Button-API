using ButtonAPI.GameAPI;
using ButtonAPI.MainMenuAPI;
using ButtonAPI.Types;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ButtonAPI.Uni.Main {
    public class PanelButton : Root {
        public PanelButton(Transform parent, string text, UnityAction listener) {
            Transform reference = SceneManager.GetActiveScene().name == "MM3" ? MainMenuAPIBase.PanelButtonTemplate : GameAPIBase.PanelButtonTemplate;

            (TMProCompnt = (transform = (gameObject = Object.Instantiate(reference, parent).gameObject).transform).Find("Text (TMP)").GetComponent<TextMeshProUGUI>()).text = text;
            TMProCompnt.richText = true;
            Object.Destroy(TMProCompnt.gameObject.GetComponent<LocalizeStringEvent>());

            (ButtonCompnt = transform.GetComponent<Button>()).onClick.RemoveAllListeners();
            (ButtonCompnt.onClick = new Button.ButtonClickedEvent()).AddListener(delegate {
                (Listener = listener).Invoke();
                FmodManager.Instance.Play(SceneManager.GetActiveScene().name == "MM3" ? SoundEvent.UIButton : SoundEvent.Page);
            });
        }
        public PanelButton(string text, UnityAction listener)
            : this((SceneManager.GetActiveScene().name == "MM3" ? MainMenuAPIBase.PanelButtonTemplate : GameAPIBase.PanelButtonTemplate).parent, text, listener) { }
    }
}
