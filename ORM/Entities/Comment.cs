using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ArticleId { get; set; }
        public DateTime PublicationDate { get; set; }

        public virtual Article Article { get; set; }
        public virtual User Author { get; set; }
    }
}
