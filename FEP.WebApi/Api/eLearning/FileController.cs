using FEP.Model;
using FEP.Model.eLearning;
using FEP.WebApiModel.eLearning;
using System.Threading.Tasks;
using System.Web.Http;

namespace FEP.WebApi.Api.eLearning
{
    [Route("api/eLearning/File")]
    public class FileController : ApiController
    {
        private readonly DbEntities db = new DbEntities();

        /// <summary>
        /// For use in index page, to list all the courses but with some fields only
        /// </summary>
        /// <returns></returns>
        [Route("api/eLearning/File/UploadInfo")]
        [HttpPost]
        [ValidationActionFilter]
        public async Task<IHttpActionResult> UploadInfo([FromBody] FileDocumentModel request)
        {
            if (ModelState.IsValid)
            {
                var fileDocument = new FileDocument
                {
                    CreatedBy = request.CreatedBy,
                    CreatedDate = request.CreatedDate,
                    FileName = request.FileName,
                    FileNameOnStorage = request.FileNameOnStorage,
                    FilePath = request.FilePath,
                    FileSize = request.FileSize,
                    FileTag = request.FileTag,
                    FileType = request.FileType,
                    User = request.User
                };

                db.FileDocument.Add(fileDocument);
            
                await db.SaveChangesAsync();

                // Save the contentFile
                ContentFile contentFile = new ContentFile
                {
                    CourseId = request.CourseId,
                    CreatedBy = fileDocument.CreatedBy,
                    FileName = fileDocument.FileName,
                    FileDocument = fileDocument,
                    FileType = request.ContentFileType,
                    ModuleContentId = request.ContentId
                };

                await db.SaveChangesAsync();

                return Ok(contentFile.Id);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}