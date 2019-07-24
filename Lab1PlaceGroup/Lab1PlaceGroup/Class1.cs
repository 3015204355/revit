using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Architecture;
using System.Linq;

namespace Lab1PlaceGroup
{
    //获取当前选择集中包含的对象
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string messages, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            //定义变量
            Dictionary<string, double> floor = new Dictionary<string, double>();
            //  Stack<string> refuge = new Stack<string>();//用来存放避难间的标高
            SortedSet<double> refuge_height = new SortedSet<double>();
            double height = 0.0;
            double minHeight = 0.0;
            bool ifHigher = true;//用来判断是否小于50000mm==50m,true表示小于50m
            Transaction ts = new Transaction(uiDoc.Document, "level");
            ts.Start();
            FilteredElementCollector collector = new FilteredElementCollector(uiDoc.Document);
            ICollection<Element> collection = collector.OfClass(typeof(Level)).ToElements();
            foreach (Element e in collection)
            {
                //  TaskDialog.Show("标高", e.Name);
                ParameterSet parameterSet = e.Parameters;
                foreach (Parameter pa in parameterSet)
                {
                    if (pa.Definition.Name == "立面")
                    {
                        double levelheight = Double.Parse(pa.AsValueString());
                        floor.Add(e.Name, levelheight);
                        if (height < levelheight)
                        {
                            height = levelheight;
                        }
                        //      TaskDialog.Show("立面高度：", pa.AsValueString());
                        break;
                    }
                }
            }
            ts.Commit();
            //TaskDialog.Show("建筑高度：", height.ToString());
            foreach (string key in floor.Keys)
            {
                TaskDialog.Show("Floor", key + " :\n " + floor[key].ToString());

            }
            if (height >= 100000)//如果建筑高度大于100米
            {
                FilteredElementCollector filteredElements = new FilteredElementCollector(doc);
                ElementCategoryFilter roomTagFilter = new ElementCategoryFilter(BuiltInCategory.OST_Rooms);
                filteredElements = filteredElements.WherePasses(roomTagFilter);

                foreach (Room room in filteredElements)
                {
                    if (room.Name.IndexOf("避难间") >= 0)
                    {
                        refuge_height.Add(floor[room.Level.Name]);
                    }
                }
                double lastHeight = 0.0;
                foreach (double rheight in refuge_height)
                {
                    //TaskDialog.Show("避难间高度", rheight.ToString());
                    if (minHeight > rheight)
                    {
                        minHeight = rheight;
                    }
                    if (ifHigher && rheight - lastHeight <= 50000)//不大于50m
                    {
                        lastHeight = rheight;
                    }
                    else
                    {
                        ifHigher = false;
                    }
                }
                if (minHeight <= 50000 && ifHigher)
                {
                    TaskDialog.Show("避难间检查结果：", "避难间设计合格");
                }
                else if (minHeight <= 50000 && !ifHigher)
                {
                    TaskDialog.Show("避难间检查结果：", "避难间设计不合格:\n+两个避难间之间的高度大于50m!");
                }
                else if (minHeight > 50000 && ifHigher)
                {
                    TaskDialog.Show("避难间检查结果：", "避难间设计不合格:\n+第一个避难层间的楼地面至灭火救援场地地面的高度大于50m!");
                }
                else
                {
                    TaskDialog.Show("避难间检查结果：", "避难间设计不合格:\n+第一个避难层间的楼地面至灭火救援场地地面的高度大于50m且两个避难间之间的高度大于50m!");
                }
            }
            return Result.Succeeded;
        }
    }
}
