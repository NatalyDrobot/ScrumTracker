using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;

namespace BusinessLayer
{
    public class ManagerUserTeamPos : BaseManager, IManagerUserTeamPos
    {
        public IEnumerable<UserTeamPositionDto> GetAllUserTeamPos()
        {
            return _srv.UserTeamPositions.GetList().ToArray();
        }
        public void UpdateUserTeamPos(UserTeamPositionDto teamPos)
        {
            _srv.UserTeamPositions.Update(teamPos);
        }
        public void RemoveUserTeamPos(UserTeamPositionDto userTeamPos)
        {
            _srv.UserTeamPositions.Remove(userTeamPos);
        }
        public void AddUserTeamPos(UserTeamPositionDto team)
        {
            _srv.UserTeamPositions.Add(team);
        }
        public UserTeamPositionDto GetUsersTeamPosByTeamIdAndUserId(Guid? teamId, string userId)
        {
            var listUsersTeamPos = _srv.UserTeamPositions.GetWithFilter(x => x.TeamId == teamId && x.UserId == userId).ToArray();
            return listUsersTeamPos.Length > 0 ? listUsersTeamPos.First() : null;
        }

        public IEnumerable<UserTeamPositionDto> GetUsersTeamPosByTeamId(Guid id)
        {
            var listUsersTeamPos = _srv.UserTeamPositions.GetWithFilter(x => x.TeamId == id).ToArray();
            return listUsersTeamPos;
        }

        //public IEnumerable<UserTeamPositionDto> GetPositionByUserId(Guid userId)
        //{
        //    var listUsersTeamPos = _srv.UserTeamPositions.GetWithFilter(x => x.TeamId == id).ToArray();
        //    return listUsersTeamPos;
        //}
    }
}
