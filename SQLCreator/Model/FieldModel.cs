using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic.FileIO;
using SQLCreator.Assets;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace SQLCreator.Model
{
    public class FieldModel : ObservableObject
    {

        public FieldModel()
        {
            this.FieldValue = new ObservableCollection<string>();
            this.References = new ObservableCollection<string>();
            this.TypesOfField = new ObservableCollection<string>(FieldTypes.TypeOfFieldsValues.ToList());
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
            set
            {
                SetProperty(ref _typeOfField, value);
            }
        }

        private ObservableCollection<string> _typesOfField;
        public ObservableCollection<string> TypesOfField
        {
            get { return _typesOfField; }
            set { SetProperty(ref _typesOfField, value); }
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
            set
            {
                ;
                SetProperty(ref _referenceTo, value);
            }
        }

        private ObservableCollection<string> _references;
        public ObservableCollection<string> References
        {
            get { return _references; }
            set { SetProperty(ref _references, value); }
        }

        public ObservableCollection<string> FieldValue { get; set; }
    }
}
