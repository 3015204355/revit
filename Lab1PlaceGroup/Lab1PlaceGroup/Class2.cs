using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Lab1PlaceGroup
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Class2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData revit, ref string messages, ElementSet elements)
        {


            try
            {
                UIDocument uidoc = revit.Application.ActiveUIDocument;
                Document doc = uidoc.Document;
                FilteredElementCollector filteredElements = new FilteredElementCollector(doc);
                ElementCategoryFilter roomTagFilter = new ElementCategoryFilter(BuiltInCategory.OST_Rooms);

                filteredElements = filteredElements.WherePasses(roomTagFilter);
                foreach (Room room in filteredElements)
                {
                    // TaskDialog.Show("RoomName", room.Name);
                    if (room.Name.IndexOf("观众厅") >= 0 || room.Name.IndexOf("会议厅") >= 0 || room.Name.IndexOf("多功能厅") >= 0)
                    {
                        String Roomlevel = room.Level.Name;
                        if (Roomlevel == "Level 1" || Roomlevel == "Level 2" || Roomlevel == "Level 3")
                        {
                            TaskDialog.Show("高层建筑人员密集场所检查", "人员密集场所:" + room.Name + " 检查合格");
                        }
                        else
                        {

                            ParameterSet parameters = room.Parameters;
                            double area = 0.0;
                            foreach (Parameter pa in parameters)
                            {

                                if (pa.Definition.Name == "面积")
                                {
                                    int index1 = pa.AsValueString().IndexOf(" ");//要对面积进行标注 m^2
                                    string substring1 = pa.AsValueString().Substring(0, index1 + 1);
                                    TaskDialog.Show("面积", pa.AsValueString() + " " + substring1);
                                    area = Double.Parse(substring1);
                                    break;
                                }
                            }
                            if (area <= 400.0)
                            {
                                TaskDialog.Show("高层建筑人员密集场所检查", "人员密集场所:" + room.Name + " 检查合格");
                            }
                            else
                            {
                                TaskDialog.Show("高层建筑人员密集场所检查", "人员密集场所:" + room.Name + " 布置在其他楼层且面积大于400平方米！");
                            }
                        }

                    }
                    if (room.Name.IndexOf("防火隔间") >= 0)
                    {
                        ParameterSet parameters = room.Parameters;
                        double area = 0.0;
                        foreach (Parameter pa in parameters)
                        {

                            if (pa.Definition.Name == "面积")
                            {
                                int index1 = pa.AsValueString().IndexOf(" ");
                                string substring1 = pa.AsValueString().Substring(0, index1 + 1);
                                area = Double.Parse(substring1);
                                break;
                            }
                        }
                        if (area >= 6.0)
                        {
                            TaskDialog.Show("防火隔间检查", "防火隔间合格");
                        }
                        else
                        {

                            TaskDialog.Show("防火隔间检查", "防火隔间面积小于6平发米");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                messages = e.Message;
                return Result.Failed;
            }
            return Result.Succeeded;
        }

    }
}


