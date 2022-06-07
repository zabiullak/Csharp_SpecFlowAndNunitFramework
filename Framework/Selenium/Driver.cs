﻿using Framework.Utilities.Listeners;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Selenium
{
    public static class Driver
    {
        [ThreadStatic] private static IWebDriver _driver;

        [ThreadStatic] public static Wait Wait;

        [ThreadStatic] public static Window Window;

        [ThreadStatic] public static WebListener webListener;

        public static void Init()
        {
            _driver = DriverFactory.Build(FW.Config.Driver.Browser, FW.Config.Driver.Type);
            Wait = new Wait(FW.Config.Driver.WaitSeconds);
            Window = new Window();
            Window.Maximize();
            webListener = new WebListener(_driver);
            _driver = webListener.Driver;
        }

        public static IWebDriver Current => _driver ?? throw new NullReferenceException("_driver is null.");

        /// <summary>
        /// Finds the element based on the locator provided.
        /// </summary>
        /// <param name="by">FindBy mechanism.</param>
        /// <param name="elementName">Name of element for logging purposes.</param>
        public static Element FindElement(By by, [Optional]string eleName)
        {
            var element = Wait.Until(drvr => drvr.FindElement(by));
            return new Element(element, eleName)
            {
                FoundBy = by
            };
        }

        /// <summary>
        /// Finds the elements based on the locator provided.
        /// </summary>
        /// <param name="by">FindBy mechanism.</param>
        public static Elements FindElements(By by)
        {
            return new Elements(Current.FindElements(by))
            {
                FoundBy = by
            };
        }

        public static void Goto(string url)
        {
            if (!url.StartsWith("http"))
            {
                url = $"http://{url}";
            }
            //FW.Log.Info(url);
            Current.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Drags an element from it's current position to the location of another element.
        /// </summary>
        /// <param name="dragElement">Drag element.</param>
        /// <param name="dropOnElement">Drop element.</param>
        public static void DragAndDrop(Element dragElement, Element dropOnElement)
        {
            var actions = new Actions(Current);
            actions.DragAndDrop(dragElement.Current, dropOnElement.Current);
            actions.Build().Perform();
        }

        public static void Quit()
        {
            FW.Log.Info("Close Browser");
            Current.Quit();
        }

        /// <summary>
        /// Refreshes the current page.
        /// </summary>
        public static void Refresh()
        {
            Current.Navigate().Refresh();
        }

        /// <summary>
        /// Selects the dropdown option. This only works for 'select' elements.
        /// </summary>
        /// <param name="by">Select option by: INDEX | TEXT | VALUE.</param>
        /// <param name="element">The Select element.</param>
        /// <param name="value">Value to select the option.</param>
        public static void SelectDropdownOption(DropdownBy by, Element element, dynamic value)
        {
            var dropdown = new SelectElement(element.Current);

            switch (by)
            {
                case DropdownBy.INDEX:
                    dropdown.SelectByIndex((int)value);
                    break;
                case DropdownBy.TEXT:
                    dropdown.SelectByText((string)value);
                    break;
                case DropdownBy.VALUE:
                    dropdown.SelectByValue((string)value);
                    break;
            }
        }

        /// <summary>
        /// Takes a screenshot of the current page as a .png file.
        /// <para>Example: Driver.TakeScreenshot("~/pics/ss", "example")</para>
        /// <para>This saves the screenshot as ~/pics/ss/example.png</para>
        /// </summary>
        /// <param name="directory">Directory to save the file to.</param>
        /// <param name="imgName">Image name without .png extension.</param>
        public static void TakeScreenshot(string directory, string imgName)
        {
            var ss = ((ITakesScreenshot)Current).GetScreenshot();
            var ssFileName = Path.Combine(directory, imgName);
            ss.SaveAsFile($"{ssFileName}.png", ScreenshotImageFormat.Png);
        }

        public static void TakeScreenShot(string imageName)
        {
            var ss = ((ITakesScreenshot)Current).GetScreenshot();
            var ssFileName = Path.Combine(FW.CurrentTestDirectory.FullName, imageName);
            ss.SaveAsFile($"{ssFileName}.png", ScreenshotImageFormat.Png);
        }

        public enum DropdownBy
        {
            INDEX = 0,
            TEXT = 1,
            VALUE = 2
        }
    }
}
