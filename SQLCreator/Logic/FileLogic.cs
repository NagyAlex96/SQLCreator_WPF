using SQLCreator.Assets;
using SQLCreator.Interfaces;
using SQLCreator.Model;

namespace SQLCreator.Logic
{
    public class FileLogic : IFileLogic
    {
        IFileReaderLogic _fileReaderLogic = new FileReaderLogic();

        public void AddFile(IList<DataBaseModel> fileModels)
        {
            foreach (var item in _fileReaderLogic.FileOpener())
            {
                fileModels.Add(item);
            }
        }

        public void Modify(DataBaseModel fModel)
        {
            //TODO
            //ModifyPage Modify = new ModifyPage(fModel, true);
            //Modify.ShowDialog();
        }

        public void Delete(IList<DataBaseModel> removeFrom, DataBaseModel removeWhat)
        {
            removeFrom.Remove(removeWhat);
        }

        public void MoveFileFromTo(IList<DataBaseModel> moveFrom, IList<DataBaseModel> moveTo, DataBaseModel item)
        {
            moveTo.Add(item);
            Delete(moveFrom, item);
        }
    }
}
