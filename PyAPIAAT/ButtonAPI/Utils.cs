using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ButtonAPI {
    public static class Utils {
        public static void DestroyChildren(this Transform parent) {
            foreach (Transform child in parent)
                Object.Destroy(child.gameObject);
        }
        public static void DestroyChildrenExcept(this Transform parent, List<Transform> excludes) {
            foreach (Transform child in parent)
                if (!excludes.Contains(child))
                    Object.Destroy(child.gameObject);
        }
        public static void DestroyChildrenExcept(this Transform parent, Transform exclude) {
            foreach(Transform child in parent)
                if (child != exclude)
                    Object.Destroy(child.gameObject);
        }
    }
}
