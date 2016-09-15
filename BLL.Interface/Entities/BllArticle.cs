using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class BllArticle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public int? AuthorId { get; set; }
        public BllUser Author { get; set; }
        public byte[] HeaderPicture { get; set; }
        public DateTime PublicationDate { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<BllComment> Comments { get; set; } 
    }
}
