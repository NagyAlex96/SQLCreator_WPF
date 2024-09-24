using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLCreator.Interfaces;
using SQLCreator.Logic;
using SQLCreator.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace SQLCreator.ViewModel
{
    class MainWindowVM : ObservableRecipient
    {
        private IFileLogic _fileLogic = new FileLogic();
        private IDbCreatorLogic _dbCreatorLogic = new DBCreatorLogic();
        private IFileWriterLogic _fileWriterLogic;

        public MainWindowVM()
        {
            AddedFiles = new ObservableCollection<DataBaseModel>();
            ProcessedFiles = new ObservableCollection<DataBaseModel>();

            AddFilesCommand = new RelayCommand(AddFiles);
            ModifyFileCommand = new RelayCommand(ModifyFile);
            RemoveFileCommand = new RelayCommand(RemoveFile);
            ProcessFileCommand = new RelayCommand(ProcessFile);
            ProcessAllFileCommmand = new RelayCommand(ProcessAllFile);
        }


        public IRelayCommand AddFilesCommand { get; private set; }
        private void AddFiles()
        {
            _fileLogic.AddFile(AddedFiles);
        }

        public IRelayCommand ModifyFileCommand { get; private set; }
        private void ModifyFile()
        {
            //_fileLogic.Modify(SelectedItem);
        }

        public IRelayCommand RemoveFileCommand { get; private set; }
        private void RemoveFile()
        {
            _fileLogic.Delete(AddedFiles, SelectedItem);
        }

        public IRelayCommand ProcessFileCommand { get; private set; }
        private void ProcessFile()
        {
            if(this.SelectedItem == null)
            {
                MessageBox.Show("No file was selected!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this._fileWriterLogic = new FileWriterLogic(this.SelectedItem);
                this._fileWriterLogic.SQLWriter();
                this._fileLogic.MoveFileFromTo(AddedFiles, ProcessedFiles, SelectedItem);
            }
        }

        public IRelayCommand ProcessAllFileCommmand { get; private set; }
        private void ProcessAllFile()
        {
            while(this.AddedFiles.Count>0)
            {
                this._dbCreatorLogic.CreateDataBase(AddedFiles[0]);
                this._fileWriterLogic = new FileWriterLogic(AddedFiles[0]);
                this._fileWriterLogic.SQLWriter();
                this._fileLogic.MoveFileFromTo(AddedFiles, ProcessedFiles, AddedFiles[0]);
            }

            //foreach (var item in AddedFiles)
            //{
            //    this._dbCreatorLogic.CreateDataBase(item);
            //    this._fileWriterLogic = new FileWriterLogic(item);
            //    this._fileWriterLogic.SQLWriter();
            //}
        }

        public ObservableCollection<DataBaseModel> AddedFiles { get; set; }
        public ObservableCollection<DataBaseModel> ProcessedFiles { get; set; }

        private DataBaseModel _selectedItem;
        public DataBaseModel SelectedItem
        {
            get { return _selectedItem; }
            set 
            {
                if(value != null)
                {
                    this._dbCreatorLogic.CreateDataBase(value);
                }
                SetProperty(ref _selectedItem, value); 
            }
        }
    }
}
