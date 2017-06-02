namespace BikeTest
{
    using System;
    using System.Linq;
    using System.Configuration;
    using System.IO;
    using System.Net.Mail;
    using System.Net;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Support.UI;
    using System.Text;
    using Test.Extensions;
    using Framework.PresentationLayer.LoginPage;
    using OpenQA.Selenium.Firefox;

    [TestClass]
    public abstract class BaseTest
    {
        #region Constants
        public TestContext TestContext { get; set; }

        private const string EmailNotificationEnabled = "email.enabled";

        public const string SmtpFrom = "smpt.from";

        public const string SmptFromPassword = "smpt.from.password";

        public const string SmptTo = "smpt.to";

        public const string SmptHost = "smpt.host";

        public const string SmtpPort = "smtp.port";

        public const string SmtpEnableSsl = "smtp.enableSsl";

        public const string DriverName = "DriverName";

        public const string Language = "Language";

        internal const string DriverDirectory = "./WebDrivers/";

        private const string FolderTestResult = "Screenshot";

        private const string TestSuitVersion = "Version";

        private const string ReportFolder = "ReportFolder";

        private static DateTime StartTime = DateTime.Now;

        private static string TestContextDir;

        #endregion

        #region Protected Fields

        protected WebDriverWait webDriverWait;

        #endregion

        #region Private Fields

        protected IWebDriver driver;

        #endregion

        #region Protected Methods Routine

        protected TPage GoToUrl<TPage>(string path)
        {
            string testUri = EnvironmentTest.GetUrl();
            this.driver.Navigate().GoToUrl(string.Format(testUri));

            return (TPage)Activator.CreateInstance(typeof(TPage), new object[] { this.webDriverWait });
        }

        #endregion

        #region Public Methods Routine

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            if (Directory.Exists(BaseTest.FolderTestResult))
            {
                Directory.Delete(BaseTest.FolderTestResult, true);
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestContextDir = this.TestContext.TestDeploymentDir;
            this.CreateWebDriver();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // here is an access to the CurrentTestOutcome but not on stacktrace
            if (TestContext.CurrentTestOutcome == UnitTestOutcome.Failed)
            {
                var taskScreen = this.driver as ITakesScreenshot;
                var screenshot = taskScreen.GetScreenshot();

                var path = BaseTest.FolderTestResult;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var imageName = string.Format("{0}.{1}.png", TestContext.FullyQualifiedTestClassName, TestContext.TestName);
                var imagePath = Path.Combine(path, imageName);

                screenshot.SaveAsFile(imagePath, ScreenshotImageFormat.Png);
            }

            this.driver.Dispose();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            SendEmailTestResult();
        }

        public SupportedWebDriver GetDriverType()
        {
            return (SupportedWebDriver)Enum.Parse(typeof(SupportedWebDriver), ConfigurationManager.AppSettings[BaseTest.DriverName]);
        }

        #endregion

        #region Private Methods Routine

        private void CreateWebDriver()
        {
            this.InitializeDriver();
            this.GoToUrl<UILoginPage>(EnvironmentTest.GetUrl());
            this.webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        private void InitializeDriver()
        {
            var lang = ConfigurationManager.AppSettings[BaseTest.Language];
            BikeTest.Framework.Configuration.SetLanguage(lang);

            switch (this.GetDriverType())
            {
                case SupportedWebDriver.Firefox:
                    this.driver = new FirefoxDriver();
                    this.driver.Manage().Window.Maximize();
                    break;

                case SupportedWebDriver.Chrome:
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--start-maximized");
                    chromeOptions.AddArgument("--disable-extensions");
                    chromeOptions.AddArgument("--disable-infobars");
                    chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                    chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                    chromeOptions.AddArgument(string.Format("--lang={0}", lang));

                    this.driver = new ChromeDriver(BaseTest.DriverDirectory, chromeOptions);
                    break;
            }
        }

        public HttpStatusCode GetPageStatusCode()
         {
            string pageURL = this.driver.Url;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(pageURL);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            return httpWebResponse.StatusCode;
        }
    
        private static void SendEmailTestResult()
        {
            string hostName = EnvironmentTest.GetHost();
            var time = Math.Round((DateTime.Now - StartTime).TotalSeconds / 60.0, 2);
            string version = ConfigurationManager.AppSettings[BaseTest.TestSuitVersion];
            string enabledString = ConfigurationManager.AppSettings[BaseTest.EmailNotificationEnabled];
            string lang = ConfigurationManager.AppSettings[BaseTest.Language];
            bool enabled;

            if (!bool.TryParse(enabledString, out enabled) || !enabled)
            {
                return;
            }

            var attachments = new List<string>();

            if (Directory.Exists(BaseTest.FolderTestResult))
            {
                attachments.AddRange(Directory.GetFiles(BaseTest.FolderTestResult));
                attachments.Sort();
            }

            string from = ConfigurationManager.AppSettings[BaseTest.SmtpFrom];
            string pass = ConfigurationManager.AppSettings[BaseTest.SmptFromPassword];
            string to = ConfigurationManager.AppSettings[BaseTest.SmptTo];
            string host = ConfigurationManager.AppSettings[BaseTest.SmptHost];
            int port = int.Parse(ConfigurationManager.AppSettings[BaseTest.SmtpPort]);
            bool sslEnabled = bool.Parse(ConfigurationManager.AppSettings[BaseTest.SmtpEnableSsl]);

            var fromAddress = new MailAddress(from, "Test");
            var toAddress = new MailAddress(to);
            string fromPassword = pass;

            string subject = string.Empty;
            var driverName = ConfigurationManager.AppSettings[BaseTest.DriverName];
            if (!attachments.Any())
            {
                subject = string.Format("Test Status: PASSED, {0}, {1}", driverName, Environment.MachineName);
            }
            else
            {
                subject = string.Format("Test Statust: {0} FAILED, {1}, {2}", attachments.Count, driverName, Environment.MachineName);
            }

            using (var message = new MailMessage(fromAddress.Address, toAddress.Address, subject, string.Empty))
            {
                var reportPath = Path.Combine(ConfigurationManager.AppSettings[BaseTest.ReportFolder], StringExtension.GetCurrentDateTimeWithSeconds());
                Directory.CreateDirectory(reportPath);
                var functionalBuilder = new StringBuilder();
                foreach (var attach in attachments)
                {

                    File.Copy(attach, Path.Combine(reportPath, Path.GetFileName(attach)));

                    var testName = Path.GetFileNameWithoutExtension(attach);
                }

                var uiBuilder = new StringBuilder();

                reportPath = Path.Combine(reportPath, "index.html");
                using (StreamWriter sw = new StreamWriter(File.Create(reportPath)))
                {
                    sw.Write(CreateMainEmailBlock(hostName, time.ToString(), version, lang, driverName, functionalBuilder.ToString(), uiBuilder.ToString()));
                }

                var testNamesBuilder = new StringBuilder();
                foreach (var attach in attachments)
                {
                    var testName = Path.GetFileNameWithoutExtension(attach);
                    testNamesBuilder.AppendFormat("{0}<br/>", testName);
                }

                message.Body = CreateBody(hostName, time.ToString(), version, lang, driverName, reportPath, testNamesBuilder.ToString());
                attachments.ForEach(path => message.Attachments.Add(new Attachment(path)));

                message.IsBodyHtml = true;

                var smtp = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = sslEnabled,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                smtp.Send(message);
            }
        }

        private static string CreateBody(string host, string time, string version, string lang, string browser, string testNames, string reportPath)
        {
            var template = @"
                <html>
	                <body>
		                <div>
			                <span>Host: </span><span>{0}</span>
		                </div>
		                <div>
			                <span>Time: </span><span>{1}</span>
		                </div>
		                <div>
			                <span>Version: </span><span>{2}</span>
		                </div>
		                <div>
			                <span>Language: </span><span>{3}</span>
		                </div>
		                <div>
			                <span>Browser: </span><span>{4}</span>
		                </div>
                        <div>
			                <span>FAILED TESTS:</span>
		                </div>
                        <div>
			                <span>{5}</span>
		                </div>
		                <div>
			                <span>Report: </span><span><a href=""file://{6}"">Open</a></span>
                        </div>
                        
                      </body>
                </html>";

            return string.Format(template, host, time, version, lang, browser, testNames, reportPath);
        }

        private static string CreateMainEmailBlock(string host, string time, string version, string lang, string browser, string functional, string ui)
        {
            var template = @"<htm>
                        <head>
                        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                         <style type=""text/css"">
                         summary::-webkit-details-marker {{
                         color: #00ACF3;
                         font-size: 125%;
                         margin-right: 2px;
                        }}
                        summary:focus {{
	                        outline-style: none;
                        }}
                        article > details > summary {{
	                        font-size: 28px;
	                        margin-top: 16px;
                        }}
                        details > p {{
	                        margin-left: 24px;
                        }}
                        details details {{
	                        margin-left: 36px;
                        }}
                        details details summary {{
	                        font-size: 16px;
                        }}

                        .autoResizeImage {{
                            max-width: 100%;
                            height: auto;
                            width: 100%;
                        }}
                    </style>
                    <title>Acceptance Test Result</title>
                    </head>
	                <body>
	                    <div>
		                    <span>Host: </span><span>{0}</span>
	                    </div>
	                    <div>
		                    <span>Time: </span><span>{1} sec</span>
	                    </div>
	                    <div>
		                    <span>Version: </span><span>{2}</span>
	                    </div>
	                    <div>
		                    <span>Language: </span><span>{3}</span>
	                    </div>
                        <div>
		                    <span>Browser: </span><span>{4}</span>
	                    </div>
	                    <br/>
	                </body>
                </html>";

            return string.Format(template, host, time, version, lang, browser, functional, ui);
        }

        #endregion

    }
}