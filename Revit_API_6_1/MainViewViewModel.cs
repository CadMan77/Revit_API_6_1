using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical; // Duct
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_6_1
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private UIDocument _uidoc;
        private Document _doc;

        //public List<Wall> SelectedWalls { get; set; }
        public List<DuctType> ProjectDuctTypes { get; set; }
        public DuctType SelectedDuctType { get; set; }
        public DelegateCommand SetDuctTypeCommand { get; }

        public List<Level> ProjectLevels { get; set; }
        public Level SelectedLevel { get; set; }
        public DelegateCommand SetLevelCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _uidoc = _commandData.Application.ActiveUIDocument;
            _doc = _uidoc.Document;

            List<DuctType> projectDuctTypes = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_DuctCurves)
                .WhereElementIsElementType()
                .Cast<DuctType>()
                .ToList();

            ProjectDuctTypes = projectDuctTypes;

            List<Level> projectLevels = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .ToList();

            ProjectLevels = projectLevels;

            //try
            //{
            //    IList<Reference> selectedWallRefList = _uidoc.Selection.PickObjects(ObjectType.Element, new WallFilter(), "Выберите стены:");

            //    List<Wall> selectedWalls = new List<Wall>();

            //    foreach (Reference selectedRef in selectedWallRefList)
            //    {
            //        Wall wall = _doc.GetElement(selectedRef) as Wall;
            //        selectedWalls.Add(wall);
            //    }

            //    SelectedWalls = selectedWalls;

            //    SetWallTypeCommand = new DelegateCommand(OnSetWallTypeCommand);
            //}
            //catch (Autodesk.Revit.Exceptions.OperationCanceledException) // ?? прервать дальнейшее выполнение команды!!
            //{
            //    //TaskDialog.Show("Отмена", "Команда прервана пользователем.");
            //    return;
            //}
            //catch (Exception ex) // ?? прервать дальнейшее выполнение команды!!
            //{
            //    TaskDialog.Show("Ошибка", $"{ex.Message}");
            //    return;
            //}
        }

        private void OnSetDuctTypeCommand()
        {
            //if (SelectedWalls.Count == 0)
            //{
            //    return;
            //}

            using (Transaction ts = new Transaction(_doc, "Set Wall Type Transaction"))
            {
                ts.Start();

                //foreach (Wall wall in SelectedWalls)
                //{
                //    wall.WallType = SelectedWallType;
                //}

                ts.Commit();
            }

            RaiseCloseRequest();

            //TaskDialog.Show("Выполнено", $"Тип выбранных стен ({SelectedWalls.Count} шт.) успешно изменен на \"{SelectedWallType.Name}\".");

            //_uidoc.Selection.Dispose();
            //_uidoc.RefreshActiveView();
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}