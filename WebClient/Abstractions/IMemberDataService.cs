using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.Commands;
using Domain.Queries;

namespace WebClient.Abstractions
{
    public interface IMemberDataService
    {
        public Task<CreateMemberCommandResult> Create(CreateMemberCommand command);
        public Task<UpdateMemberCommandResult> Update(UpdateMemberCommand command);
        public Task<GetAllMembersQueryResult> GetAllMembers();
        public Task<GetMemberQueryResult> GetById(Guid id);
        public Task<DeleteMemberQueryResult> Delete(DeleteMemberCommand command);
    }
}
