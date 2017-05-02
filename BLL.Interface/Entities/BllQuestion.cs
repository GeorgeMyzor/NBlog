using System;
using System.Collections.Generic;

namespace BLL.Interface.Entities
{
    public class BllQuestion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public int? AuthorId { get; set; }
        public BllUser Author { get; set; }
        public DateTime PublicationDate { get; set; }
        public IEnumerable<BllAnswer> Answers { get; set; }
    }
}
