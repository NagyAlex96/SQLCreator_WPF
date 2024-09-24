using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SQLCreator.Model
{
    public class DataBaseModel : ObservableObject
    {
        public DataBaseModel()
        {
            this.TxtFileName = new ObservableCollection<string>();
            this.TablesInfo = new ObservableCollection<TableModel>();
        }

        private string _outPutDestination;
        public string OutPutDestination
        {
            get { return _outPutDestination; }
            set { SetProperty(ref _outPutDestination, value); }
        }

        private string _pdfFileDestination;
        public string PdfFileDestination
        {
            get { return _pdfFileDestination; }
            set { SetProperty(ref _pdfFileDestination, value); }
        }

        private string _pdfFileName;
        public string PdfFileName
        {
            get { return _pdfFileName; }
            set { SetProperty(ref _pdfFileName, value); }
        }

        private string _pdfPageNum;
        public string PdfPageNum
        {
            get { return _pdfPageNum; }
            set { SetProperty(ref _pdfPageNum, value); }
        }

        private string _pdfLineNum;
        public string PdfLineNum
        {
            get { return _pdfLineNum; }
            set { _pdfLineNum = value; }
        }

        private string _txtFileDestination;
        public string TxtFileDestination
        {
            get { return _txtFileDestination; }
            set { SetProperty(ref _txtFileDestination, value); }
        }

        private string _nameOfDb;
        public string NameOfDb
        {
            get { return _nameOfDb; }
            set { SetProperty(ref _nameOfDb, value); }
        }

        private ObservableCollection<string> _txtFileName;
        public ObservableCollection<string> TxtFileName
        {
            get { return _txtFileName; }
            set { SetProperty(ref _txtFileName, value); }
        }

        private ObservableCollection<TableModel> _tablesInfo;
        public ObservableCollection<TableModel> TablesInfo
        {
            get { return _tablesInfo; }
            set { SetProperty(ref _tablesInfo, value); }
        }          
    }
}
