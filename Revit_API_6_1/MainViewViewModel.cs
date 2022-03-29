using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical; // Duct
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Revit_API_6_1
{
    public class MainViewViewModel
    {
        private ExternalCommandData _сommandData;
        private UIDocument uidoc;
        private Document doc;

        //public List<DuctType> ProjectDuctTypes { get; set; }
        public List<DuctType> ProjectDuctTypes { get; } = new List<DuctType>();

        //public List<Level> ProjectLevels { get; set; }
        public List<Level> ProjectLevels { get; } = new List<Level>();

        public DuctType SelectedDuctType { get; set; } = null;
        public Level SelectedLevel { get; set; } = null;

        static readonly string intMask = @"^\-?\d+$";
        readonly Regex intRGX = new Regex(intMask);


        public int DuctOffset { get; } = 2500;

        //private int ductOffset;
        //public int DuctOffset
        //{
        //    get => ductOffset;
        //    set
        //    {
        //        if (value != 0 && intRGX.IsMatch(value.ToString()))
        //        {
        //            ductOffset = value;
        //        }
        //    }
        //}

        public DelegateCommand CreateCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _сommandData = commandData;
            doc = commandData.Application.ActiveUIDocument.Document;

            List<DuctType> projectDuctTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(DuctType))
                .Cast<DuctType>()
                .ToList();

            ProjectDuctTypes = projectDuctTypes;

            List<Level> projectLevels = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .ToList();

            ProjectLevels = projectLevels;

            //DuctOffset = 2500; // "Стандартное" значение высоты монтажа воздуховодов, мм

            //CreateCommand = new DelegateCommand(OnCreateCommand);

            //List<Reference> pointRefs = uidoc.Selection.PickObjects(Point);

            //try
            //{
            //    IList<Reference> selectedWallRefList = Uidoc.Selection.PickObjects(ObjectType.Element, new WallFilter(), "Выберите стены:");

            //    List<Wall> selectedWalls = new List<Wall>();

            //    foreach (Reference selectedRef in selectedWallRefList)
            //    {
            //        Wall wall = doc.GetElement(selectedRef) as Wall;
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

        private void OnCreateCommand()
        {
            
            
            if (SelectedDuctType == null || SelectedLevel == null)
            {
                return;
            }

            using (Transaction ts = new Transaction(doc, "Set Wall Type Transaction"))
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

            //Uidoc.Selection.Dispose();
            //Uidoc.RefreshActiveView();
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}