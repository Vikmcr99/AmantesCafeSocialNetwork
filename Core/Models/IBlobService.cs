using System.IO;
using System.Threading.Tasks;

namespace Core.Models
{
    public interface IBlobService
    {
        Task<string> UploadAsync(Stream stream);
        Task DeleteAsync(string blobName);
    }
}
