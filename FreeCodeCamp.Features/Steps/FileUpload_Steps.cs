﻿using FreeCodeCamp.WebPages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace FreeCodeCamp.Features.Steps
{
    [Binding]
    public class FileUpload_Steps
    {


        [Then(@"Click on Choose File and Upload a New File")]
        public void ThenClickOnChooseFileAndUploadANewFile()
        {
            //Pages.HerokuApp.ClickOnFileUpload();
            Pages.HerokuApp.SendFile("Info.txt");
        }

        [Then(@"Verify FileUploaded Successfully")]
        public void ThenVerifyFileUploadedSuccessfully()
        {
            Pages.HerokuApp.ClickOnFileUpload();
            string Filename = Pages.HerokuApp.GetFileName();
            Assert.That(Filename, Is.EqualTo("Info.txt"));
        }

    }
}
