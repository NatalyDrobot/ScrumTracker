using System;
using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerUserTeamPos
    {
        IEnumerable<UserTeamPositionDto> GetAllUserTeamPos();
        void RemoveUserTeamPos(UserTeamPositionDto userTeamPos);
        void AddUserTeamPos(UserTeamPositionDto team);
        void UpdateUserTeamPos(UserTeamPositionDto teamPos);
        IEnumerable<UserTeamPositionDto> GetUsersTeamPosByTeamId(Guid id);
        UserTeamPositionDto GetUsersTeamPosByTeamIdAndUserId(Guid? teamId, string userId);
    }
}
