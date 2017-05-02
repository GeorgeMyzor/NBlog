using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Entities
{
    public class BllAnswer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public int? AuthorId { get; set; }
        public bool? IsAnswer { get; set; }
        public BllUser Author { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
