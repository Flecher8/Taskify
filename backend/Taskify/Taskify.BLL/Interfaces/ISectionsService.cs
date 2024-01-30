using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taskify.Core.DbModels;
using Taskify.Core.Result;

namespace Taskify.BLL.Interfaces
{
    public interface ISectionsService
    {
        Task<Result<Section>> CreateSectionAsync(Section section);
        Task<Result<bool>> DeleteSectionAsync(string id, string redirectSectionId);
        Task<Result<Section>> GetSectionByIdAsync(string id);
        Task<Result<List<Section>>> GetSectionsByProjectIdAsync(string projectId);
        Task<Result<bool>> UpdateSectionAsync(Section section);
        Task<Result<bool>> MoveSectionAsync(string sectionId, int targetSequenceNumber);
        Task<Result<bool>> ArchiveSectionAsync(string sectionId);
        Task<Result<bool>> UnarchiveSectionAsync(string sectionId);
    }
}
