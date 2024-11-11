using ButtonAPI.GameAPI;
using ButtonAPI.MainMenuAPI;
using ButtonAPI.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using UnityEngine.Events;

namespace ButtonAPI.Uni.Main {
    public class Page : PyPage {
        private static Transform currentParent;
        private static Transform currentHome;
        public Tab CurrentTab { get; set; }
        private int OpenCount { get; set; }
        private static List<Page> pageHistory = new List<Page>();
        public static bool isPageOpen;
        public static Page CurrentPage;
        private static bool scenePatched;
        public Page(UnityAction onEscape = null) {
            Transform currentRef = SceneManager.GetActiveScene().name == "MM3" ? RefSetMain() : RefSetGame();

            (transform = (gameObject = Object.Instantiate(currentRef, currentParent).gameObject).transform).name = $"PyPage_{Guid.NewGuid()}";

            Contents = transform.Find("BG");
            transform.DestroyChildrenExcept(Contents);
            (TabParent = Contents.Find("Tab_Panel")).DestroyChildren();

            Contents.DestroyChildrenExcept(new List<Transform> {
                Contents.Find("gpHints_Info"),
                //Contents.Find("Button Apply"),
                Contents.Find("Button Back"),
                Contents.Find("Tab_Panel"),
                Contents.Find("Tab_Line"),
                Contents.Find("gp")
            });
            Contents.Find("gpHints_Info").gameObject.SetActive(false);
            Contents.Find("gp").gameObject.SetActive(false);

            (Back = Contents.Find("Button Back").GetComponent<Button>()).onClick.RemoveAllListeners();
            (Back.onClick = new Button.ButtonClickedEvent()).AddListener(delegate { OnBack(); });

            Tabs = new List<Tab>();

            if (!scenePatched)
                SceneManager.sceneLoaded += delegate {
                    if (SceneManager.GetActiveScene().name == "MM3" || SceneManager.GetActiveScene().name == "Playtest") {
                        pageHistory.Clear();
                        isPageOpen = false;
                        CurrentPage = null;
                    }
                    scenePatched = true;
                };
        }
        public void OpenMenu() {
            OpenCount = 0;

            foreach (var page in pageHistory) {
                page.gameObject.SetActive(false);
            }

            this.gameObject.SetActive(true);
            currentHome.gameObject.SetActive(false);
            FmodManager.Instance.Play(SoundEvent.Page);

            if (Tabs.Count > 0 && OpenCount == 0) {
                Tabs[0].OpenContents(true);
                OpenCount++;
            }
            if (!pageHistory.Contains(this))
                pageHistory.Add(this);

            isPageOpen = true;
            CurrentPage = this;
        }

        public void OnBack(bool silent = false, UnityAction onEscape = null) {
            this.gameObject.SetActive(false);

            if (pageHistory.Contains(this))
                pageHistory.Remove(this);
            if (pageHistory.Count > 0)
                pageHistory[pageHistory.Count - 1].OpenMenu();
            else {
                if (SceneManager.GetActiveScene().name == "MM3") {
                    MainMenuAPIBase.MainMenuCompnt.SetState(MainMenu.State.Home);
                } else {
                    GameAPIBase.GameMenuCompnt.SetState(GameMenu.State.MenuHome);
                }
                currentHome.gameObject.SetActive(true);
                isPageOpen = false;
            }

            if (!silent)
                FmodManager.Instance.Play(SoundEvent.UIButton);

            onEscape?.Invoke();
        }
        public Tab AddTab(string text) => new Tab(this, text);
        private Transform RefSetMain() {
            currentParent = MainMenuAPIBase.MainMenu;
            currentHome = MainMenuAPIBase.Home;
            return MainMenuAPIBase.Settings;
        }
        private Transform RefSetGame() {
            currentParent = GameAPIBase.Settings.parent;
            currentHome = GameAPIBase.Home;
            return GameAPIBase.Settings;
        }
    }
}