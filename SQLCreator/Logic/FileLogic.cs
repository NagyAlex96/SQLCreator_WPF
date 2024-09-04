using SQLCreator.Assets;
using SQLCreator.Interfaces;
using SQLCreator.Model;

namespace SQLCreator.Logic
{
    public class FileLogic : IFileLogic
    {
        private IFileReader _fileManager;
        private IDbCreatorLogic _dbCreatorLogic;
        private IFileWriter _fWriter;

        public FileLogic()
        {
            this._fileManager = new FileReader();
            this._dbCreatorLogic = new DBCreatorLogic();
        }

        public void AddFile(IList<FileModel> fileModels)
        {
            foreach (var item in _fileManager.FileOpener())
            {
                fileModels.Add(item);
            }
        }

        public void Modify(FileModel fModel)
        {
            //ModifyPage Modify = new ModifyPage(fModel, true);
            //Modify.ShowDialog();
        }

        public void Delete(IList<FileModel> removeFrom, FileModel removeWhat)
        {
            removeFrom.Remove(removeWhat);
        }

        public void ProcessFile(IList<FileModel> fileModels)
        {
            //foreach (var item in fileModels)
            //{
            //    Task _task = new Task(() => { ProcessFile(item); }, TaskCreationOptions.LongRunning);
            //    _task.Start();
            //}
        }

        public void ProcessFile(FileModel fileModel)
        {
            this._dbCreatorLogic.CreateDataBase(this._fileManager.PdfReader(fileModel), this._fileManager.TxtReader(fileModel), fileModel.TxtFileName);
            this._fWriter = new FileWriter(this._dbCreatorLogic.DbCreator);
            this._fWriter.SQLWriter(fileModel.OutPutDestination);
        }


        public void MoveFileFromTo(IList<FileModel> moveFrom, IList<FileModel> moveTo, FileModel item)
        {
            moveTo.Add(item);
            Delete(moveFrom, item);
        }
    }
}
