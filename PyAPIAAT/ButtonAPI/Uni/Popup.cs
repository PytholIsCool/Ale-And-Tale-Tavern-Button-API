using ButtonAPI.GameAPI;
using ButtonAPI.MainMenuAPI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ButtonAPI.Uni {
    public class Popup {
        private static Transform Base;
        private static Transform Dimmer;
        private static Transform PopupObj;
        private static Transform TextObj;

        private static TextMeshProUGUI StandardTitle;
        private static TextMeshProUGUI TextTitle;
        private static TMP_InputField TextInput;

        public static bool isPopupActive;
        public static void StandardPrompt(string title, UnityAction onApply, UnityAction onBack = null, string backText = "Cancel", string applyText = "Confirm") {
            Transform reference = SceneManager.GetActiveScene().name == "MM3" ? RefSetMain() : RefSetGame();
            if (reference.Find("PopupManager") == null)
                CreatePopupMan(reference);

            Dimmer.gameObject.SetActive(true);
            PopupObj.gameObject.SetActive(true);
            StandardTitle.text = title;

            PopupObj.Find("Button Back/Text (TMP)").GetComponent<TextMeshProUGUI>().text = backText;
            PopupObj.Find("Button Apply/Text (TMP)").GetComponent<TextMeshProUGUI>().text = applyText;

            var back = PopupObj.Find("Button Back").GetComponent<Button>();
            back.onClick.RemoveAllListeners();
            (back.onClick = new Button.ButtonClickedEvent()).AddListener(delegate {
                Dimmer.gameObject.SetActive(false);
                PopupObj.gameObject.SetActive(false);
                onBack?.Invoke();
                FmodManager.Instance.Play(SoundEvent.UIButton);

                isPopupActive = false;
            });
            var apply = PopupObj.Find("Button Apply").GetComponent<Button>();
            apply.onClick.RemoveAllListeners();
            (apply.onClick = new Button.ButtonClickedEvent()).AddListener(delegate {
                Dimmer.gameObject.SetActive(false);
                PopupObj.gameObject.SetActive(false);
                onApply.Invoke();
                FmodManager.Instance.Play(SoundEvent.UIButton);

                isPopupActive = false;
            });

            isPopupActive = true;
        }

        public static void TextPrompt(string title, UnityAction<string> onApply, UnityAction onBack = null, string backText = "Cancel", string applyText = "Confirm") {
            Transform reference = SceneManager.GetActiveScene().name == "MM3" ? RefSetMain() : RefSetGame();
            if (reference.Find("PopupManager") == null)
                CreatePopupMan(reference);

            Dimmer.gameObject.SetActive(true);
            TextObj.gameObject.SetActive(true);
            TextTitle.text = title;

            TextObj.Find("Button Back/Text (TMP)").GetComponent<TextMeshProUGUI>().text = backText;
            TextObj.Find("Button Apply/Text (TMP)").GetComponent<TextMeshProUGUI>().text = applyText;

            var back = TextObj.Find("Button Back").GetComponent<Button>();
            back.onClick.RemoveAllListeners();
            (back.onClick = new Button.ButtonClickedEvent()).AddListener(delegate {
                Dimmer.gameObject.SetActive(false);
                TextObj.gameObject.SetActive(false);
                TextInput.text = null;
                onBack?.Invoke();
                FmodManager.Instance.Play(SoundEvent.UIButton);

                isPopupActive = false;
            });
            var apply = TextObj.Find("Button Apply").GetComponent<Button>();
            apply.onClick.RemoveAllListeners();
            (apply.onClick = new Button.ButtonClickedEvent()).AddListener(delegate {
                Dimmer.gameObject.SetActive(false);
                TextObj.gameObject.SetActive(false);
                onApply.Invoke(TextInput.text);
                TextInput.text = null;
                FmodManager.Instance.Play(SoundEvent.UIButton);

                isPopupActive = false;
            });

            isPopupActive = true;
        }

        private static void CreatePopupMan(Transform p) {
            var Manager = Object.Instantiate(Base.Find("VideoTestPanel"), p);
            Manager.name = "PopupManager";

            Dimmer = Manager.Find("ScreenDimmed");

            PopupObj = Manager.Find("Popup");
            (TextObj = Object.Instantiate(PopupObj, Manager)).name = "Text";

            PopupObj.Find("Slider_HealthBar_Default").gameObject.SetActive(false);
            TextObj.Find("Slider_HealthBar_Default/Fill").GetComponent<Image>().enabled = false;
            TextObj.Find("gp").gameObject.SetActive(false);

            StandardTitle = PopupObj.Find("Text_Info").GetComponent<TextMeshProUGUI>();
            Object.Destroy(StandardTitle.transform.GetComponent<LocalizeStringEvent>());
            TextTitle = TextObj.Find("Text_Info").GetComponent<TextMeshProUGUI>();
            Object.Destroy(TextTitle.transform.GetComponent<LocalizeStringEvent>());

            var Temp = new GameObject() { name = "TextInput" };
            Temp.transform.SetParent(TextObj.Find("Slider_HealthBar_Default/Fill"));
            Temp.transform.parent.SetAsLastSibling();

            TextInput = Temp.AddComponent<TMP_InputField>();
            var textComponent = Temp.AddComponent<TextMeshProUGUI>();
            TextInput.textComponent = textComponent;
            TextInput.textComponent.overflowMode = TextOverflowModes.Ellipsis;
            textComponent.fontSize = 18;
            textComponent.alignment = TextAlignmentOptions.Left;

            RectTransform textInputRect = Temp.GetComponent<RectTransform>();
            textInputRect.sizeDelta = new Vector2(420f, 30f);

            var placeholderObj = new GameObject() { name = "Placeholder" };
            placeholderObj.transform.SetParent(Temp.transform);
            var placeholderText = placeholderObj.AddComponent<TextMeshProUGUI>();
            placeholderText.text = "Type here...";
            placeholderText.fontSize = 18;
            placeholderText.alignment = TextAlignmentOptions.Left;
            placeholderText.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            TextInput.placeholder = placeholderText;

            RectTransform placeholderRect = placeholderObj.GetComponent<RectTransform>();
            placeholderRect.sizeDelta = textInputRect.sizeDelta;

            TextInput.transform.localPosition = new Vector3(155f, 0f, 0f);
            placeholderObj.transform.localPosition = new Vector3(0f, 0f, 0f);

            Object.Destroy(PopupObj.Find("Button Apply/Text (TMP)").GetComponent<LocalizeStringEvent>());
            Object.Destroy(PopupObj.Find("Button Back/Text (TMP)").GetComponent<LocalizeStringEvent>());
            Object.Destroy(TextObj.Find("Button Apply/Text (TMP)").GetComponent<LocalizeStringEvent>());
            Object.Destroy(TextObj.Find("Button Back/Text (TMP)").GetComponent<LocalizeStringEvent>());

            Manager.gameObject.SetActive(true);
            Dimmer.gameObject.SetActive(false);
            PopupObj.gameObject.SetActive(false);
            TextObj.gameObject.SetActive(false);
        }

        private static Transform RefSetMain() {
            Base = MainMenuAPIBase.Settings;
            return MainMenuAPIBase.MainMenu;
        }

        private static Transform RefSetGame() {
            Base = GameAPIBase.Settings;
            return GameAPIBase.GameMenu;
        }
    }
}