using BepInEx.Logging;
using ButtonAPI.Uni.Main;
using PyAPIAAT;

namespace PyAPIExamples.Examples {
    public class MainTest {
        public static void Init() {
            var Pg = new Page();
            var Pg2 = new Page();
            var Pg3 = new Page();

            new PanelButton("Examples", () => {
                Pg.OpenMenu();
            });

            var Tab = Pg.AddTab("Tab 1");
            Tab.AddButton("Nested Menu 1", "Open", () => {
                Pg2.OpenMenu();
            });

            Tab.AddSlider("TestSlider", (val) => {
                //val represents the value of the slider
            });

            var sel = Tab.AddSelector("TestSelector");
            sel.AddSetting("Setting 1", () => {

            });
            sel.AddSetting("Setting 2", () => {

            });
            sel.AddSetting("Setting 3", () => {

            });
            Tab.AddToggle("TestToggle", (val) => {
                //val represents the value of the toggle
            });

            var Tab2 = Pg2.AddTab("Tab 1");
            Tab2.AddButton("Nested Menu 2", "Open", () => {
                Pg3.OpenMenu();
            });

            var Tab3 = Pg3.AddTab("Tab 1");
            Tab3.AddButton("Test", "Test", () => {
                Popup.StandardPrompt("TestTitle", () => {

                });
            });

            var Tab4 = Pg3.AddTab("Tab 2");
            Tab4.AddToggle("Test Toggle", (val) => {

            });

            var Tab5 = Pg3.AddTab("Tab 3");
            Tab5.AddSlider("TestSlider", (val) => {

            });
        }
    }
}
