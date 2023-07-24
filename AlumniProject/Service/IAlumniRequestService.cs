using AlumniProject.Dto;
using AlumniProject.Entity;

namespace AlumniProject.Service;

public interface IAlumniRequestService
{
    Task<int> CreateAlumniRequest(AccessRequest accessRequest,int schoolId);
    Task<AccessRequest> UpdateAccessRequest(int accessRequestId,int status);
    Task DeleteAccessRequest(int accessRequestId);
    Task<AccessRequest> GetAccessRequestsById(int id);
    Task<PagingResultDTO<AccessRequest>> GetAccessRequestsByScchoolId(int pageNo, int pageSize, int schoolId);
    Task<bool> IsRequestExist(String email, int classId);

}
