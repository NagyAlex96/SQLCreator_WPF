using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLCreator.Logic;
using SQLCreator.Model;
using System.Collections.ObjectModel;

namespace SQLCreator.ViewModel
{
    class FileVM : ObservableRecipient
    {
        FileLogic _fileLogic = new FileLogic();
        public FileVM()
        {
            AddedFiles = new ObservableCollection<FileModel>();
            ProcessedFiles = new ObservableCollection<FileModel>();

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
            _fileLogic.Modify(SelectedItem);
        }

        public IRelayCommand RemoveFileCommand { get; private set; }
        private void RemoveFile()
        {
            _fileLogic.Delete(AddedFiles, SelectedItem);
        }       
              
        public IRelayCommand ProcessFileCommand { get; private set; }
        private void ProcessFile()
        {
            //TODO: error
            _fileLogic.ProcessFile(SelectedItem);
            _fileLogic.MoveFileFromTo(AddedFiles, ProcessedFiles, SelectedItem);
        }

        public IRelayCommand ProcessAllFileCommmand { get; private set; }
        private void ProcessAllFile()
        {
            _fileLogic.ProcessFile(AddedFiles);
        }

        public ObservableCollection<FileModel> AddedFiles { get; set; }
        public ObservableCollection<FileModel> ProcessedFiles { get; set; }

        private FileModel _selectedItem;
        public FileModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

    }
}
