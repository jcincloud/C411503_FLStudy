using System.Web;
namespace DotWeb.WebApp
{
    interface IFileHandle
    {
        string aj_FList(int id, string FileKind);
        string aj_FUpload(int id, string FilesKind, HttpPostedFileBase file);
        string aj_FDelete(int id, string FileKind, string FileName);
    }
}
