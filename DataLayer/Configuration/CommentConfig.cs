using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Configuration
{
   internal class CommentConfig: EntityTypeConfiguration<Comment>
    {
       public CommentConfig()
       {
            Property(с => с.Text).HasMaxLength(255);
           
            

        }
    }
}
