using Microsoft.AspNetCore.Http;

namespace Attendance.DataAccess.Interfaces.FileUploader
{
    public interface IFileUploader
    {
        Task<string> UploadAsync(IFormFile NewFile, string FolderName);
        Task<byte[]> UploadAsArrayAsync(IFormFile NewFile);
        void Delete(string FilePath);
        IFormFile DownLoad(string path);
        Task<string> Edit(string OldFilePath, IFormFile NewFile, string FolderName);
    }
}
