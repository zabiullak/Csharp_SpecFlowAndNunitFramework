﻿using Framework;
using Framework.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCodeCamp.WebPages
{
    public class Pages
    {
        [ThreadStatic]
        public static HomePage Home;

        [ThreadStatic]
        public static HRMHomePage HRMHomePage;

        [ThreadStatic]
        public static HRMLoginPage HRMLoginPage;

        [ThreadStatic]
        public static NewsPage NewsPage;

        
        public static void Init()
        {
            Home = new HomePage();
            HRMLoginPage = new HRMLoginPage();
            HRMHomePage = new HRMHomePage();
            NewsPage = new NewsPage();
        }

        public static HRMLoginPage GoToOrangeHrmApplication()
        {
            Driver.Goto(FW.Config.Test.url_orangeHRM);
            FW.Log.Info($"Landed on Url -> {FW.Config.Test.url_orangeHRM}");
            return new HRMLoginPage();
        }

        public static HomePage GotoFreeCodeCamp()
        {
            Driver.Goto(FW.Config.Test.url_freeCodeCamp);
            FW.Log.Info($"Landed on Url -> {FW.Config.Test.url_freeCodeCamp}");
            return new HomePage();
        }

    }
}
