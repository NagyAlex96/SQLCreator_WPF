using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SQLCreator.Model
{
    public class FieldModel : ObservableObject
    {
        public FieldModel()
        {
            this.FieldValue = new ObservableCollection<string>();
            this.References = new ObservableCollection<string>();
        }

        private string _fieldName;
        public string FieldName
        {
            get { return _fieldName; }
            set { SetProperty(ref _fieldName, value); }
        }

        private string _typeOfField;
        public string TypeOfField
        {
            get { return _typeOfField; }
            set { SetProperty(ref _typeOfField, value); }
        }

        private bool _isPrimaryKey;
        public bool IsPrimaryKey
        {
            get { return _isPrimaryKey; }
            set { SetProperty(ref _isPrimaryKey, value); }
        }

        private bool _isForeignKey;
        public bool IsForeignKey
        {
            get { return _isForeignKey; }
            set { SetProperty(ref _isForeignKey, value); }
        }

        private string _referenceTo;
        public string ReferenceTo
        {
            get { return _referenceTo; }
            set { SetProperty(ref _referenceTo, value); }
        }

        private ObservableCollection<string> _references;
        public ObservableCollection<string> References
        {
            get { return _references; }
            set { SetProperty(ref _references, value); }
        }

        private ObservableCollection<string> _fieldValue;
        public ObservableCollection<string> FieldValue
        {
            get { return _fieldValue; }
            set { SetProperty(ref _fieldValue, value); }
        }
    }
}
