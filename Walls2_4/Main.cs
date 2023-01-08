using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Задание 2.4 Количество стен по этажам

namespace Walls2_4
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication uiapp = commandData.Application;                       
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);       //1й этаж
            FilteredElementCollector collector2 = new FilteredElementCollector(doc);      //2й этаж
            ICollection<Element> levels = collector
                .OfClass(typeof(Level))
                .ToElements();
            ICollection<Element> levels2 = collector2
                .OfClass(typeof(Level))
                .ToElements();
            var query = from element in collector where element.Name == "Level 1" select element;
            var query2 = from element in collector2 where element.Name == "Level 2" select element;

            List<Element> level1 = query.ToList<Element>();
            List<Element> level2 = query2.ToList<Element>();
            ElementId levelId = level1[0].Id;
            ElementId levelId2 = level2[0].Id;

            ElementLevelFilter levelFilter = new ElementLevelFilter(levelId);
            ElementLevelFilter levelFilter2 = new ElementLevelFilter(levelId2);
            collector = new FilteredElementCollector(doc);
            collector2 = new FilteredElementCollector(doc);
            ICollection<Element> allWallsOnLevel1 = collector
                .OfClass(typeof(Wall))
                .WherePasses(levelFilter)
                .ToElements();

            ICollection<Element> allWallsOnLevel2 = collector2
                .OfClass(typeof(Wall))
                .WherePasses(levelFilter2)
                .ToElements();

            TaskDialog.Show("1й и 2й", $"Кол-во стен на 1-ом этаже {allWallsOnLevel1.Count.ToString()}\n" +
                $"Кол-во стен на 2-ом этаже {allWallsOnLevel2.Count.ToString()}");

            return Result.Succeeded;
        }
    }
}
