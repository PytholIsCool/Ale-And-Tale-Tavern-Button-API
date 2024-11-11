using BepInEx;
using BepInEx.Logging;
using ButtonAPI.MainMenuAPI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PyAPIAAT {
    [BepInPlugin("com.Pythol.PyApiExamples", "Examples", "1.0.0")]
    public class Plugin : BaseUnityPlugin {
        private void Awake() {
            Logger.LogInfo($"PyAPI Examples loaded");
            SceneManager.sceneLoaded += OnSceneWasLoaded;
            Logger.LogInfo($"Test");
        }
        private void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneWasLoaded;
        }
        private void OnSceneWasLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            Logger.LogInfo($"SceneLoaded: {scene.name}");
            if (scene.name == "MM3") {
                StartCoroutine(WaitForScene(scene));
                StartCoroutine(MainMenuAPIBase.Primer(PyAPIExamples.Examples.MainTest.Init));
            }
        }
        private IEnumerator WaitForScene(Scene scene) {
            while (!scene.isLoaded) {
                yield return null;
            }
            new WaitForSeconds(5f);
        }
    }
}
