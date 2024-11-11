using ButtonAPI.GameAPI;
using ButtonAPI.MainMenuAPI;
using UnityEngine.SceneManagement;
using UnityEngine;
using ButtonAPI.Types;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using ButtonAPI.Uni;
using ButtonAPI;

namespace PyAPI.ButtonAPI.Uni.TabControls {
    public class ButtonControl : Root {
        private static Transform ButtonRef;
        private string _buttonText { get; set; }
        public string ButtonText { get => _buttonText; set { 
                _buttonText = value;
                if (ButtonTMProCompnt != null)
                    ButtonTMProCompnt.text = _buttonText;
            } 
        }
        public TextMeshProUGUI ButtonTMProCompnt { get; internal set; }
        public GameObject ButtonParent { get; private set; }
        public ButtonControl(Transform parent, string text, string buttonText, UnityAction listener) {
            Transform reference = SceneManager.GetActiveScene().name == "MM3" ? SetRefMain() : SetRefGame();

            (transform = (gameObject = Object.Instantiate(reference, parent).gameObject).transform).name = $"PyButton_{text}";

            transform.DestroyChildrenExcept(transform.Find("Text (TMP)"));
            (ButtonParent = new GameObject { name = "GridLayoutGroup" }).transform.SetParent(transform);
            SetLayout();

            (TMProCompnt = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>()).text = text;

            var ButtonObj = Object.Instantiate(ButtonRef, ButtonParent.transform);
            ButtonObj.name = $"Button_{buttonText}";

            Object.Destroy(ButtonObj.Find("Text (TMP)").GetComponent<LocalizeStringEvent>());
            (ButtonTMProCompnt = ButtonObj.Find("Text (TMP)").GetComponent<TextMeshProUGUI>()).text = buttonText;

            (ButtonCompnt = ButtonObj.GetComponent<Button>()).onClick.RemoveAllListeners();
            (ButtonCompnt.onClick = new Button.ButtonClickedEvent()).AddListener(delegate {
                (Listener = listener).Invoke();
                FmodManager.Instance.Play(SoundEvent.UIButton);
            });
        }
        /// <summary>
        /// Adds another button to the same column. MAX 3 BUTTONS.
        /// </summary>
        public ButtonControl AddButton(string buttonText, UnityAction listener) {
            Transform ButtonObj;
            TextMeshProUGUI TMP;
            Button B_Compnt;

            ButtonObj = Object.Instantiate(ButtonRef, ButtonParent.transform);
            ButtonObj.name = $"Button_{buttonText}";

            Object.Destroy(ButtonObj.Find("Text (TMP)").GetComponent<LocalizeStringEvent>());
            (TMP = ButtonObj.Find("Text (TMP)").GetComponent<TextMeshProUGUI>()).text = buttonText;

            (B_Compnt = ButtonObj.GetComponent<Button>()).onClick.RemoveAllListeners();
            (B_Compnt.onClick = new Button.ButtonClickedEvent()).AddListener(delegate {
                listener.Invoke();
                FmodManager.Instance.Play(SoundEvent.UIButton);
            });
            return this;
        }
        private void SetLayout() {
            ButtonParent.transform.localPosition = new Vector3(400f, -35f, 0f);
            var GLG = ButtonParent.gameObject.AddComponent<GridLayoutGroup>();
            GLG.cellSize = new Vector2(150f, 45f);
            GLG.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            GLG.constraintCount = 3;
            GLG.spacing = new Vector2(20f, 0f);
            GLG.childAlignment = TextAnchor.UpperRight;
        }
        private static Transform SetRefMain() {
            ButtonRef = MainMenuAPIBase.Settings.Find("BG/Button Back");
            return MainMenuAPIBase.ToggleTemplate;
        }
        private static Transform SetRefGame() {
            ButtonRef = GameAPIBase.Settings.Find("BG/Button Back");
            return GameAPIBase.ToggleTemplate;
        }
        public ButtonControl(Tab tab, string text, string buttonText, UnityAction listener)
            : this(tab.Container, text, buttonText, listener) { }
    }
}
