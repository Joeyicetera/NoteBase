﻿using NoteBaseLogicInterface.Models;
using System.ComponentModel;

namespace App.Models
{
    public class TagModel
    {
        public int ID { get; set; }

        [DisplayName("Titel")]
        public string Title { get; private set; }

        public TagModel(Tag _tag)
        {
            ID = _tag.ID;
            Title = _tag.Title;
        }
    }
}
