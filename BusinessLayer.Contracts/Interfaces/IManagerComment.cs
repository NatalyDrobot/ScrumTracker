
using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerComment
    {
        void Add(CommentDto comment);
        void Remove(CommentDto comment);
        void Update(CommentDto comment);
        CommentDto GetById(string id);
        IEnumerable<CommentDto> GetAll();
    }
}