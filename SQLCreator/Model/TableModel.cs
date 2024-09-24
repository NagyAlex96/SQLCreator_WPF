using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SQLCreator.Model
{
    public class TableModel : ObservableObject
    {
        public TableModel()
        {
            this.FieldInfo = new ObservableCollection<FieldModel>();
        }

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set { SetProperty(ref _tableName, value); }
        }

        private int _priority;
        public int Priority
        {
            get { return _priority; }
            set { SetProperty(ref _priority, value); }
        }

        private ObservableCollection<FieldModel> _fieldInfo;
        public ObservableCollection<FieldModel> FieldInfo
        {
            get { return _fieldInfo; }
            set { SetProperty(ref _fieldInfo, value); }
        }

    }
}
