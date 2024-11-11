using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ButtonAPI.Types {
    public class Root {
        public Transform transform { get; internal set; }
        public GameObject gameObject { get; internal set; }
        public TextMeshProUGUI TMProCompnt { get; internal set; }
        private string _text { get; set; }
        public string Text {
            get => _text; set {
                _text = value;
                if (TMProCompnt != null)
                    TMProCompnt.text = _text;
            }
        }
        public Button ButtonCompnt { get; internal set; }
        public UnityAction Listener { get; internal set; }
    }
}
