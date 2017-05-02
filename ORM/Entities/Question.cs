using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }

        public virtual User Author { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}
