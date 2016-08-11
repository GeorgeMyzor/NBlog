using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.DTO
{
    public class DalComment : IEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ArticleId { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? AuthorId { get; set; }
        public DalUser Author { get; set; }
    }
}
