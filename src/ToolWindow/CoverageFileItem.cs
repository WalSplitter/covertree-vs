using System.Collections.ObjectModel;
using System.ComponentModel;
using CoverTree.VS.Coverage;

namespace CoverTree.VS.ToolWindow
{
    public class CoverageFileItem : INotifyPropertyChanged
    {
        private double _pct;
        private CoverageStatus _status;

        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsFolder { get; set; }
        public FileCoverage CoverageData { get; set; }
        public ObservableCollection<CoverageFileItem> Children { get; } = new ObservableCollection<CoverageFileItem>();

        public double Pct
        {
            get => _pct;
            set { _pct = value; OnPropertyChanged(nameof(Pct)); OnPropertyChanged(nameof(PctDisplay)); }
        }

        public CoverageStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); OnPropertyChanged(nameof(StatusColor)); }
        }

        public string PctDisplay => $"{_pct:F1}%";

        public string StatusColor
        {
            get
            {
                switch (_status)
                {
                    case CoverageStatus.Passing: return "#4EC94E";
                    case CoverageStatus.Warning: return "#CCA700";
                    case CoverageStatus.Failing: return "#F14C4C";
                    default: return "#808080";
                }
            }
        }

        public string Tooltip
        {
            get
            {
                if (CoverageData == null) return Name;
                return $"{Name}\nLines: {CoverageData.Lines?.Pct:F1}%\n" +
                       $"Functions: {CoverageData.Functions?.Pct:F1}%\n" +
                       $"Statements: {CoverageData.Statements?.Pct:F1}%\n" +
                       $"Branches: {CoverageData.Branches?.Pct:F1}%";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string n) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
