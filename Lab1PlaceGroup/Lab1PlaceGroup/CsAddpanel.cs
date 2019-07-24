using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
namespace Lab1PlaceGroup
{

    class CsAddpanel : Autodesk.Revit.UI.IExternalApplication
    {
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("FireSafety");
            Uri uriImage = new Uri(@"C:\Users\TJU_J\Pictures\button.png");
            BitmapImage lagrgeIamge = new BitmapImage(uriImage);

            RibbonPanel ribbonPanel_1 = application.CreateRibbonPanel("FireSafety","距离约束");
            PushButton pushButton1 = ribbonPanel_1.AddItem(new PushButtonData("避难间", "避难间", @"D:\Code\C#\Lab1PlaceGroup\Lab1PlaceGroup\bin\Debug\Lab1placeGroup.dll", "Lab1PlaceGroup.Class1")) as PushButton;            
            pushButton1.LargeImage = lagrgeIamge;

            RibbonPanel ribbonPanel_2 = application.CreateRibbonPanel("FireSafety", "面积检查");
            PushButton pushButton2 = ribbonPanel_2.AddItem(new PushButtonData("密集场所", "密集场所", @"D:\Code\C#\Lab1PlaceGroup\Lab1PlaceGroup\bin\Debug\Lab1placeGroup.dll", "Lab1PlaceGroup.Class2")) as PushButton;
            pushButton2.LargeImage = lagrgeIamge;
            return Result.Succeeded;

        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
