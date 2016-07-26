using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;

namespace BusinessLayer
{
    public  class ManagerPosition:BaseManager, IManagerPosition
    {
        public IEnumerable<PositionDto> GetAllPositions()
        {
           return _srv.Positions.GetList();
        }

        public void AddPosition(PositionDto position)
        {
            _srv.Positions.Add(position);
        }

        public void RemovePosition(PositionDto position)
        {
            _srv.Positions.Remove(position);
        }
        public IEnumerable<PositionDto> GetTeamByName(string name)
        {
            var listUsers = _srv.Positions.GetWithFilter(x => x.Name == name).ToArray();
            return listUsers.Length > 0 ? listUsers : null;
        }
    }
}
