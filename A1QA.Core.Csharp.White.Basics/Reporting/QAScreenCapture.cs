//  <copyright file="QAScreenCapture.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Media.Imaging;
using A1QA.Core.Csharp.White.Basics.SystemUtilities;
using OpenQA.Selenium;
using TestStack.White.InputDevices;
using TestStack.White.WindowsAPI;
using Point = System.Drawing.Point;

namespace A1QA.Core.Csharp.White.Basics.Reporting
{
    /// <summary>
    ///     Provides methods for capturing images on the screen
    /// </summary>
    public static class QAScreenCapture
    {
        /// <summary>
        ///     Name of images directory
        /// </summary>
        private const string ImagesDirectoryName = "Images";

        /// <summary>
        ///     Gets a value indicating whether screen capture is on or off
        /// </summary>
        private static bool IsEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings["ScreenCaptureIsEnabled"]);

        /// <summary>
        ///     Gets path of directory which will contain the report
        /// </summary>
        private static string ReportPath => ConfigurationManager.AppSettings["ScreenCaptureReportPath"];

        /// <summary>
        ///     Gets prefix which will be used in the image filename
        /// </summary>
        private static string ImagePrefix => ConfigurationManager.AppSettings["ScreenCaptureImagePrefix"];

        /// <summary>
        ///     Gets extension which will be used in the image filename
        /// </summary>
        private static string ImageExtension => ConfigurationManager.AppSettings["ScreenCaptureImageExtension"];

        public static string ScreenshotDirectory
        {
            get
            {
                var curDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return curDir + "\\TestResults\\Images\\";
            }
        }

        /// <summary>
        ///     Capture an image of the entire desktop
        /// </summary>
        /// <param name="isStepImage">Whether or not image is step image</param>
        public static void CaptureDesktop(bool isStepImage = false)
        {
            CaptureImage(AutomationElement.RootElement, isStepImage);
        }

        /// <summary>
        ///     Capture an image of the UIItem
        /// </summary>
        /// <param name="uiItem">UIItem to capture image of</param>
        public static void CaptureImage(object element)
        {
            if (element is IWebElement)
            {
                CaptureChromeScreenshot((IWebElement) element);
            }
            else if (element is AutomationElement)
            {
                CaptureImage((AutomationElement) element);
            }
            else
            {
                Report.Output(Report.Level.Debug, "Unable to take a screenshot of the element");
            }
        }

        /// <summary>
        ///     Capture an image of an AutomationElement
        /// </summary>
        /// <param name="automationElement">AutomationElement to capture image of</param>
        /// <param name="isStepImage">Whether or not image is step image</param>
        public static string CaptureImage(AutomationElement automationElement, bool isStepImage = false)
        {
            var imageName = string.Empty;

            if (IsEnabled)
            {
                var sourcePoint = Point.Empty;
                var destinationPoint = Point.Empty;
                var selectionRectangle = Rectangle.Empty;

                var currentDateTime = string.Format("{0:yyyy-MM-dd_hh-mm-ss-ff}", DateTime.Now);
                imageName = string.Format("{0}{1}{2}", ImagePrefix, currentDateTime, ImageExtension);
                var imageDirectoryPath = string.Format(@"{0}\{1}", ReportPath, ImagesDirectoryName);
                var imagePath = string.Format(@"{0}\{1}", imageDirectoryPath, imageName);

                try
                {
                    // Create images directory if it doesn't already exist
                    if (!Directory.Exists(imageDirectoryPath))
                    {
                        Directory.CreateDirectory(imageDirectoryPath);
                    }

                    var imageDirectoryName = Path.GetFileName(imageDirectoryPath);

                    // Get rectangle containing the AutomationElement
                    // If rectangle is not empty then convert from System.Windows.Rect to System.Drawing.Rectangle
                    // Then use this to capture image to disk and then return the image path
                    var rect = automationElement.Current.BoundingRectangle;

                    if (!rect.IsEmpty)
                    {
                        if (rect.Size.Width != 0 && rect.Size.Height != 0)
                        {
                            selectionRectangle.Height = Convert.ToInt32(rect.Size.Height);
                            selectionRectangle.Width = Convert.ToInt32(rect.Size.Width);
                            selectionRectangle.X = Convert.ToInt32(rect.TopRight.Y);
                            selectionRectangle.Y = Convert.ToInt32(rect.BottomLeft.X);
                            sourcePoint = new Point(Convert.ToInt32(automationElement.Current.BoundingRectangle.X), Convert.ToInt32(automationElement.Current.BoundingRectangle.Y));

                            using (var bitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height))
                            {
                                using (var graphics = Graphics.FromImage(bitmap))
                                {
                                    graphics.CopyFromScreen(sourcePoint, destinationPoint, selectionRectangle.Size);
                                    bitmap.Save(imagePath, ImageFormat.Png);
                                    var reportLevel = Report.GetImageReportLevel(isStepImage);
                                    Report.Output(reportLevel, imageName);
                                    graphics.Dispose();
                                }
                            }
                            return imagePath;
                        }
                        Report.Output(Report.Level.Debug, Properties.Resources.ScreenCaptureEmptyRectangleErrorMsg);
                    }
                    else
                    {
                        Report.Output(Report.Level.Debug, Properties.Resources.ScreenCaptureEmptyRectangleErrorMsg);
                    }
                }
                catch (Exception ex)
                {
                    Report.Output(Report.Level.Debug, Properties.Resources.ScreenCaptureExceptionErrorMsg, ex.Message);
                }
            }

            return null;
        }

        public static string CaptureImageWithPrintScreen(AutomationElement automationElement, bool isStepImage = false)
        {
            var imageName = string.Empty;
            var currentDateTime = string.Format("{0:yyyy-MM-dd_hh-mm-ss-ff}", DateTime.Now);
            imageName = string.Format("{0}{1}{2}", ImagePrefix, currentDateTime, ImageExtension);
            var imageDirectoryPath = string.Format(@"{0}\{1}", ReportPath, ImagesDirectoryName);
            var imagePath = string.Format(@"{0}\{1}", imageDirectoryPath, imageName);

            try
            {
                if (IsEnabled)
                {
                    Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.PRINTSCREEN);

                    if (!Directory.Exists(imageDirectoryPath))
                    {
                        Directory.CreateDirectory(imageDirectoryPath);
                    }

                    Path.GetFileName(imageDirectoryPath);

                    var rect = automationElement.Current.BoundingRectangle;
                    if (!rect.IsEmpty)
                    {
                        if (rect.Size.Width != 0 && rect.Size.Height != 0)
                        {
                            var bitmapSource = Clipboard.GetImage();
                            var croppedBitmap = new CroppedBitmap(bitmapSource, new Int32Rect(Convert.ToInt32(rect.X), Convert.ToInt32(rect.Y), Convert.ToInt32(rect.Width), Convert.ToInt32(rect.Height)));
                            using (var fileStream = new FileStream(imagePath, FileMode.Create))
                            {
                                BitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(croppedBitmap));
                                encoder.Save(fileStream);
                            }
                            var reportLevel = Report.GetImageReportLevel(isStepImage);
                            Report.Output(reportLevel, imageName);

                            return imagePath;
                        }
                        Report.Output(Report.Level.Debug, Properties.Resources.ScreenCaptureEmptyRectangleErrorMsg);
                    }
                }
                else
                {
                    Report.Output(Report.Level.Debug, Properties.Resources.ScreenCaptureEmptyRectangleErrorMsg);
                }
            }
            catch (Exception ex)
            {
                Report.Output(Report.Level.Debug, Properties.Resources.ScreenCaptureExceptionErrorMsg, ex.Message);
            }

            return null;
        }

        /// <summary>
        ///     Capture an image of the Web element
        /// </summary>
        /// <param name="element">Web control to capture image of</param>
        public static void CaptureImageWeb(IWebElement element)
        {
            CaptureChromeScreenshot(element);
        }

        /// <summary>
        ///     Screenshot method using the ChromeAutomationDriver.Driver object
        /// </summary>
        /// <param name="element">IWebElement</param>
        /// <param name="reportLevel">'debug' to include all screenshots. Or 'noscreenshots' to exclude screenshots</param>
        private static void CaptureChromeScreenshot(IWebElement element)
        {
            try
            {
                var ss = ((ITakesScreenshot) Report.Driver).GetScreenshot();

                // Get the real screen location of the element
                const string javascript = "return arguments[0].getBoundingClientRect()";
                var obj = (Dictionary<string, object>) ((IJavaScriptExecutor) Report.Driver).ExecuteScript(javascript, element);
                var rect = new Rectangle((int) double.Parse(obj["left"].ToString()), (int) double.Parse(obj["top"].ToString()), (int) double.Parse(obj["width"].ToString()), (int) double.Parse(obj["height"].ToString()));

                if (rect.Width == 0 || rect.Height == 0)
                {
                    Report.Output(Report.Level.Action, "Unable to take a screenshot of the element. Height or Width is equal zero.");
                    return;
                }

                using (var ms = new MemoryStream(ss.AsByteArray))
                {
                    using (var img = Image.FromStream(ms))
                    {
                        var bmp = new Bitmap(rect.Width, rect.Height);
                        using (var gr = Graphics.FromImage(bmp))
                        {
                            gr.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), rect, GraphicsUnit.Pixel);
                        }

                        var now = string.Format("{0:yyyy-MM-dd_hh-mm-ss-f}", DateTime.Now);
                        var imageName = "Image_" + now + ".png";
                        var fileName = ScreenshotDirectory + imageName;

                        bmp.Save(fileName, ImageFormat.Png);

                        QAWait.Until(() => File.Exists(fileName));
                    }
                }
            }
            catch (Exception e)
            {
                Report.Output(Report.Level.Debug, "Unable to take screenshot. Exception occured: " + e.Message);
            }
        }
    }
}