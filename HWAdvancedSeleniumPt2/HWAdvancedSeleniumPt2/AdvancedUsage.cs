using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace HWAdvancedSeleniumPt2
{
    public class AdvancedUsage
    {
        [TestFixture]
        class BrowserOptions
        {
            IWebDriver driver;
            IJavaScriptExecutor jexec;
          
            [OneTimeSetUp]
            public void OneTimeSetUp()
            {
                var options = new ChromeOptions();
                options.AddArguments
                    (
                        "--start-fullscreen",
                        "--start-maximized",
                        "--disable-infobars"
                    );
                
                options.AddUserProfilePreference("download.default_directory", @"C:\test");

                driver = new ChromeDriver(options);
                driver.Url = "https://unsplash.com/search/photos/test";

                jexec = (IJavaScriptExecutor)driver;
            }

            [Test]
            public void DownloadLastItem()
            {
                while (true)
                {
                    var prevScrollY = Convert.ToInt64(jexec.ExecuteScript("return window.scrollY"));

                    jexec.ExecuteScript("window.scrollBy(0, 400)");
                    Thread.Sleep(500);

                    var ScrollY = Convert.ToInt64(jexec.ExecuteScript("return window.scrollY"));

                    if (prevScrollY == ScrollY)
                        break;
                }

                var lastImage = driver.FindElements(By.XPath("//*[@id='gridMulti']/div[1]//a/div/img")).Last();
                new Actions(driver).MoveToElement(lastImage).Perform();
                lastImage.Click();
                var download = driver.FindElement(By.XPath("//span[text()='Download free']"));
                
                jexec.ExecuteScript($"arguments[0].click()", download);
                
                Assert.That(driver.FindElement(By.ClassName("_3Gbbu")).Text, Is.EqualTo("Say thanks"));              
            }
            
            [OneTimeTearDown]
            public void OneTimeTearDown()
            {
                driver.Quit();
            }
        }
        }
}
