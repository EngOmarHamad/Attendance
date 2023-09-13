
using Attendance.DataAccess.Interfaces.FileUploader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Attendance.DataAccess.Services.FileUploader
{
    public class FileSystemUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static decimal fileNumber = 1;
        public FileSystemUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IFormFile DownLoad(string path)
        {
            path = _webHostEnvironment.WebRootPath + path;
            string fileName = Path.GetFileName(path);
            using Stream stream = new FileStream(path, FileMode.Open);
            IFormFile file = new FormFile(stream, 0, stream.Length, fileName, fileName);
            return file;
        }
        public void Delete(string FilePath)
        {
            string path = _webHostEnvironment.WebRootPath + FilePath;
            if (File.Exists(path)) File.Delete(path);
        }

        public async Task<string> Edit(string OldFilePath, IFormFile NewFile, string folderName)
        {
            if (NewFile is not null)
            {
                Delete(OldFilePath);
                return await UploadAsync(NewFile, folderName);
            }
            return OldFilePath;
        }

        public async Task<string> UploadAsync(IFormFile NewFile, string FolderName)
        {
            fileNumber++;
            string path = _webHostEnvironment.WebRootPath + "\\" + FolderName;
            // string fileName = Guid.NewGuid().ToString() + "-" + Path.GetFileNameWithoutExtension(NewFile.FileName) + Path.GetExtension(NewFile.FileName);
            string fileName = fileNumber + "-" + Guid.NewGuid().ToString() + Path.GetExtension(NewFile.FileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using FileStream fileStream = new(Path.Combine(path, fileName), FileMode.Create);
            await NewFile.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            fileStream.Close();

            return "\\" + Path.Combine(FolderName, fileName);
        }
        public async Task<byte[]> UploadAsArrayAsync(IFormFile NewFile)
        {
            using MemoryStream memoryStream = new();
            await NewFile.CopyToAsync(memoryStream);
            await memoryStream.FlushAsync();
            memoryStream.Close();
            return memoryStream.ToArray();
        }

    }
}
