﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MusicStore.Core
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class Genre
    {
        [DataMember]
        [Key]
        public int GenreId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public virtual ICollection<Album> Albums { get; set; }
    }
}
