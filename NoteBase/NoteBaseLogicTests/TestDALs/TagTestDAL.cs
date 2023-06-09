﻿using NoteBaseDALInterface;
using NoteBaseDALInterface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBaseLogicTests.TestDALs
{
    internal class TagTestDAL : ITagDAL
    {
        public TagDTO Create(string _title)
        {
            return new(0, _title);
        }

        public void Delete(int _tagId)
        {
            
        }

        public TagDTO GetById(int _tagId)
        {
            throw new NotImplementedException();
        }

        public List<TagDTO> GetByPerson(string _userMail)
        {
            throw new NotImplementedException();
        }

        public TagDTO GetByTitle(string _Title)
        {
            if (_Title == "fontys")
            {
                return new(11, "fontys");
            } 
            else if (_Title == "eindhoven")
            {
                return new(12, "fontys");
            }

            return new(0, "");
        }

        public List<TagDTO> GetByNote(int _noteId)
        {
            throw new NotImplementedException();
        }

        public TagDTO Update(int _id, string _title)
        {
            throw new NotImplementedException();
        }

        public List<TagDTO> GetByPerson(int _PersonId)
        {
            throw new NotImplementedException();
        }

        public void CreateNoteTag(int _noteId, int _tagId)
        {
            
        }

        public void DeleteNoteTag(int _noteId)
        {
            
        }
    }
}
