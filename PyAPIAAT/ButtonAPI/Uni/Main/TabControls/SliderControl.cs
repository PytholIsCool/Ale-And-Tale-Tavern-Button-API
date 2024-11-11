using ButtonAPI.GameAPI;
using ButtonAPI.MainMenuAPI;
using ButtonAPI.Types;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ButtonAPI.Uni.TabControls {
    public class SliderControl : Root {
        public TextMeshProUGUI ValDisplay { get; private set; }
        public Slider Slider { get; private set; }
        public UnityAction<float> SListener { get; internal set; }
        public float DefaultVal { get; internal set; }
        public SliderControl(Transform parent, string text, UnityAction<float> listener, float defaultValue = 0f, float minValue = 0f, float maxValue = 100f, bool isDecimal = false) {
            Transform reference = SceneManager.GetActiveScene().name == "MM3" ? MainMenuAPIBase.SliderTemplate : GameAPIBase.SliderTemplate;

            DefaultVal = defaultValue;
            var figures = "0";

            (transform = (gameObject = Object.Instantiate(reference, parent).gameObject).transform).name = $"PySlider_{text}";

            (TMProCompnt = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>()).text = text;
            TMProCompnt.richText = true;
            ValDisplay = transform.Find("Slider_HandleType/FillArea/Fill/Handle/Text (TMP)").GetComponent<TextMeshProUGUI>();

            (Slider = transform.Find("Slider_HandleType").GetComponent<Slider>()).onValueChanged.RemoveAllListeners();
            (Slider.onValueChanged = new Slider.SliderEvent()).AddListener(SListener = listener);

            Slider.minValue = minValue;
            Slider.maxValue = maxValue;
            Slider.value = defaultValue;

            if (isDecimal != false)
                figures = "0.0";

            Slider.onValueChanged.AddListener(new UnityAction<float>((val) => ValDisplay.text = val.ToString(figures)));
            ValDisplay.text = DefaultVal.ToString(figures);
        }
        public SliderControl(Tab tab, string text, UnityAction<float> listener, float defaultValue = 0f, float minValue = 0f, float maxValue = 100f, bool isDecimal = false)
            : this(tab.Container, text, listener, defaultValue, minValue, maxValue, isDecimal) { }
    }
}
