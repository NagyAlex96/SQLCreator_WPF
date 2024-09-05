using CommunityToolkit.Mvvm.ComponentModel;

namespace SQLCreator.Model
{
    public class FileModel : ObservableObject
    {
        //elérési útvonal --> betöltésnél, kimenetnél
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

        private string _txtFileDestination;
        public string TxtFileDestination
        {
            get { return _txtFileDestination; }
            set { SetProperty(ref _txtFileDestination, value); }
        }

        private string _txtFileName;
        public string TxtFileName
        {
            get { return _txtFileName; }
            set { SetProperty(ref _txtFileName, value); }
        }
    }
}
