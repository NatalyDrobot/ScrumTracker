using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerPosition
    {
        IEnumerable<PositionDto> GetAllPositions();

        void AddPosition(PositionDto position);

        void RemovePosition(PositionDto position);
        IEnumerable<PositionDto> GetTeamByName(string name);
    }
}