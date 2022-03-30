using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical; // Duct
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
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

        public List<DuctType> ProjectDuctTypes { get; } = new List<DuctType>();

        public List<Level> ProjectLevels { get; } = new List<Level>();

        public DuctType SelectedDuctType { get; set; } = null;
        public Level SelectedLevel { get; set; } = null;

        static readonly string intMask = @"^\-?\d+$";
        readonly Regex intRGX = new Regex(intMask);

        //public int DuctOffset { get; } // ?? "unrecovarable error"
        //public int DuctOffset { get; set; } = 2500; // простовато

        private int ductOffset;
        public int DuctOffset
        {
            get => ductOffset;
            set
            {
                if (value != 0 && intRGX.IsMatch(value.ToString()))
                {
                    ductOffset = value;
                }
            }
        }

        XYZ point1 = null, point2 = null;

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

            DuctOffset = 2600; // "Стандартное" значение отметки середины воздуховода, мм

            CreateCommand = new DelegateCommand(OnCreateCommand);

            try
            {
                point1 = new XYZ(100, 0, 0); // осторожно - футы!
                point2 = new XYZ(0, 100, 0);
                //point1 = uidoc.Selection.PickPoint("Выберите первую точку:");
                //point2 = uidoc.Selection.PickPoint("Выберите вторую точку:");

            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return;
            }
            //catch (Exception ex) // ?? прервать дальнейшее выполнение команды!!
            //{
            //    TaskDialog.Show("Ошибка", $"{ex.Message}");
            //    return;
            //}

        }

        private void OnCreateCommand()
        {
            if ((point1==null || point2 == null) || SelectedDuctType == null || SelectedLevel == null)
            {
                return;
            }

            //Curve curve = Line.CreateBound(point1, point2);

            //TaskDialog.Show("SupplyAirTypeId", $"{mepSystemType.Id}"); // 712974

            using (Transaction ts = new Transaction(doc, "Create Duct Transaction"))
            {
                ts.Start();

                //ElementId ei = new ElementId(712974);
                //Duct duct = Duct.Create(doc, doc.GetElement(ei).Id, SelectedDuctType.Id, SelectedLevel.Id, point1, point2);

                MEPSystemType mepSystemType = new FilteredElementCollector(doc)
                    .OfClass(typeof(MEPSystemType))
                    .Cast<MEPSystemType>()
                    .FirstOrDefault(m => m.SystemClassification == MEPSystemClassification.SupplyAir);
                Duct duct = Duct.Create(doc, mepSystemType.Id, SelectedDuctType.Id, SelectedLevel.Id, point1, point2);

                double ductOffsetInFeet = UnitUtils.ConvertToInternalUnits(ductOffset, UnitTypeId.Millimeters);

                duct.get_Parameter(BuiltInParameter.RBS_OFFSET_PARAM).Set(ductOffsetInFeet); // "Отметка посередине"

                ts.Commit();
            }

            RaiseCloseRequest();

            //TaskDialog.Show("Выполнено", "+");
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}