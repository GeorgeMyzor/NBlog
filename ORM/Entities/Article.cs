using System;
using System.Collections.Generic;

namespace ORM.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? AuthorId { get; set; }

        public virtual List<Comment> Comments { get; set; }
        public virtual User Author { get; set; }
    }
}
