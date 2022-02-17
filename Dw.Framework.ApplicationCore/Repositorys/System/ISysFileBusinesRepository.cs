using Dw.Framework.Infrastructure.Database;
using Dw.Framework.Model.Dto.System;
using Dw.Framework.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dw.Framework.ApplicationCore.Repositorys.System
{
    public interface ISysFileBusinesRepository : IRepository<SysFileBusines, Guid>
    {
        List<FileUploadDto> GetFileUploadDto(string businesCode, Guid businesId);
        List<FileUploadDto> GetFileUploadDto(string businesCode, List<Guid> businesIds);
        List<SysFileBusines> GetFileUploadUrl(string businesCode, IEnumerable<Guid> businesIds);
        string GetFileUploadImgUrl(List<SysFileBusines> fileDatas, Guid businesId);
        Task SetFilesStatus(Guid businesId);
        Task DeleteFileByBusinesId(Guid businesId);
    }
}
