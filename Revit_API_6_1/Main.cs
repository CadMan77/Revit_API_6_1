// Разработать WPF-приложение, для создания воздуховода по двум введённым точкам.
// Тип воздуховода и базовый уровень его расположения должны выбираться из выпадающего списка.
// Смещение воздуховода (значение параметра "Отметка посередине") задаётся с помощью ввода значения в окне.
// PS: Значения прочих параметров - произвольные
// Например, для параметра "Тип системы" можно взять значение "Supply Air" (Id=712974)
// ElementId ei = new ElementId(712974)

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
