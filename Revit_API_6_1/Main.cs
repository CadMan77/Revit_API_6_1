// Создайте WPF-приложение, которое создаёт воздуховод по двум введённым точкам.
// Тип воздуховода и уровень расположения должны выбираться из выпадающего списка.
// Смещение воздуховода задаётся с помощью ввода значения в окне.

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_6_1
{
    [Transaction(TransactionMode.Manual)]

    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {           
            MainView mainView = new MainView(commandData);
            mainView.ShowDialog();
            return Result.Succeeded;
        }
    }
}
