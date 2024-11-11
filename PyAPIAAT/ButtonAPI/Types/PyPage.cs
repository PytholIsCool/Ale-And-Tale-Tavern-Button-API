using ButtonAPI.Uni;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ButtonAPI.Types {
    public class PyPage {
        public Transform transform { get; internal set; }
        public GameObject gameObject { get; internal set; }
        public Transform TabParent { get; internal set; }
        public Transform Contents { get; internal set; }
        public Button Back { get; internal set; }
        public List<Tab> Tabs { get; internal set; }
    }
}
