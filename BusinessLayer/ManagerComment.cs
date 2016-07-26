using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;

namespace BusinessLayer
{
   public class ManagerComment: BaseManager,IManagerComment
    {
       public void Add(CommentDto comment)
       {
         _srv.Comments.Add(comment);
       }

       public void Remove(CommentDto comment)
       {
            _srv.Comments.Remove(comment);
        }

       public void Update(CommentDto comment)
       {
            _srv.Comments.Update(comment); 
       }

       public CommentDto GetById(string id)
       {
          return _srv.Comments.GetWithFilter(c=>c.Id.ToString()==id).FirstOrDefault();
       }

       public IEnumerable<CommentDto> GetAll()
       {
           return _srv.Comments.GetList().ToArray();
       }
    }
}
