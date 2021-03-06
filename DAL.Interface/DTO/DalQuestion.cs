﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.DTO
{
    public class DalQuestion : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public int? AuthorId { get; set; }
        public DalUser Author { get; set; }
        public DateTime PublicationDate { get; set; }
        public IEnumerable<DalAnswer> Answers { get; set; }
    }
}
