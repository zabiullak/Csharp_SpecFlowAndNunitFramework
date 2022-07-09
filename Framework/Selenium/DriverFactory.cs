﻿using Framework.Utilities.Listeners;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace Framework.Selenium
{
    public static class DriverFactory
    {
        static ChromeOptions options = new ChromeOptions();

        public static IWebDriver Build(string browserName, string type)
        {
            FW.Log.Info($"Browser: {browserName}");

            if(type.ToLower() == "local")
            {
                switch (browserName)
                {
                    case "chrome":
                        return BuildChromeDriver();

                    case "firefox":
                        return new FirefoxDriver();

                    default:
                        throw new System.ArgumentException($"{browserName} not supported locally.");
                }
            }
            else if (type.ToLower() == "remote")
            {
                return BuildRemoteDriver(browserName);
            }
            else
            {
                throw new ArgumentException($"{type} is invalid. Choose 'local' or 'remote'.");
            }
            
        }

        private static IWebDriver BuildChromeDriver()
        {
            new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            //return new ChromeDriver(FW.WORKSPACE_DIRECTORY + "_drivers");
            //IWebDriver _driver =  new ChromeDriver(AddChromeOptions());
            return new ChromeDriver(AddChromeOptions());
            //WebListener webListener = new WebListener(_driver);
            //return webListener.Driver;
        }

        private static IWebDriver BuildRemoteDriver(string browserName)
        {
            var DOCKER_GRID_HUB_URI = new Uri("http://localhost:4444/wd/hub");
            RemoteWebDriver driver;

            switch (browserName)
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions
                    {
                        BrowserVersion = "",
                        PlatformName = "LINUX",
                    };

                    chromeOptions.AddArgument("--start-maximized");

                    driver = new RemoteWebDriver(DOCKER_GRID_HUB_URI, chromeOptions.ToCapabilities());
                    break;

                case "firefox":
                    var firefoxOptions = new FirefoxOptions
                    {
                        BrowserVersion = "",
                        PlatformName = "LINUX",
                    };

                    driver = new RemoteWebDriver(DOCKER_GRID_HUB_URI, firefoxOptions.ToCapabilities());
                    break;

                default:
                    throw new ArgumentException($"{browserName} is not supported remotely.");
            }

            return driver;
        }

        public static ChromeOptions AddChromeOptions()
        {
            FW.Log.Info("Launching google chrome with new profile..");
            options.AddArguments("--disable-extensions");
            options.AddArguments("--disable-notifications");
            //options.AddUserProfilePreference("download.default_directory", _autoUtils.GetRepoDownloadFolder());
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("safebrowsing.enabled", true);
            options.AddArgument("no-sandbox");
            options.AddArguments("enable-features=NetworkServiceInProcess");
            options.AddArguments("disable-features=NetworkService");
            return options;
        }

        private static string PlatformDriver
        {
            get
            {
                return Environment.OSVersion.Platform.ToString().Contains("Win")
                    ? "_drivers/windows"
                    : "_drivers/mac";
            }
        }
    }
}
