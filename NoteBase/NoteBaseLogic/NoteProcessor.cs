﻿using NoteBaseDAL;
using NoteBaseDALFactory;
using NoteBaseDALInterface;
using NoteBaseDALInterface.Models;
using NoteBaseInterface;
using NoteBaseLogicInterface;
using NoteBaseLogicInterface.Models;

namespace NoteBaseLogic
{
    public class NoteProcessor : INoteProcessor
    {
        private readonly INoteDAL NoteDAL;
        private readonly ITagDAL TagDAL;
        public NoteProcessor(INoteDAL _noteDAL, ITagDAL _tagDAL)
        {
            NoteDAL = _noteDAL;
            TagDAL = _tagDAL;
        }

        public Response<Note> Create(Note _note)
        {
            _note = AddTags(_note);
            DALResponse<NoteDTO> noteDALreponse = NoteDAL.Create(_note.ToDTO());
            string tempMessage = noteDALreponse.Message;
            noteDALreponse = NoteDAL.GetByTitle(_note.Title);
            noteDALreponse.Message = tempMessage;

            Response<Note> response = new(noteDALreponse.Succeeded, noteDALreponse.Message);

            foreach (Tag tag in _note.TagList)
            {
                TagDAL.Create(tag.ToDTO());

                DALResponse<TagDTO> tagDALresponse = TagDAL.GetByTitle(tag.Title);

                if (tagDALresponse.Data.Count  == 0)
                {
                    response.Succeeded = tagDALresponse.Succeeded;
                    response.Message = tagDALresponse.Message;

                    return response;
                }
                else if (noteDALreponse.Data.Count == 0)
                {
                    response.Succeeded = noteDALreponse.Succeeded;
                    response.Message = noteDALreponse.Message;

                    return response;
                }
                NoteDAL.CreateNoteTag(noteDALreponse.Data[0].ID, tagDALresponse.Data[0].ID);
            }

            return response;
        }

        public Note AddTags(Note _note)
        {
            string[] allWords = _note.Text.Split(" ");
            for (int i = 0; i < allWords.Length; i++)
            {
                string word = allWords[i];
                if (word.StartsWith("#"))
                {
                    Tag tag = new(i, word.Substring(1).ToLower());
                    _note.TryAddTag(tag);
                }
            }

            return _note;
        }

        public Response<Note> Get(int _noteId)
        {
            throw new NotImplementedException();
        }

        public Response<Note> GetByPerson(int _personId)
        {
            DALResponse<NoteDTO> DALreponse = NoteDAL.GetByPerson(_personId);
            Response<Note> response = new(DALreponse.Succeeded, DALreponse.Message);

            foreach (NoteDTO noteDTO in DALreponse.Data)
            {
                Category cat = new(noteDTO.Category.ID, noteDTO.Category.Title, noteDTO.Category.PersonId);
                Note note = new(noteDTO.ID, noteDTO.Title, noteDTO.Text, cat);

                foreach (TagDTO tagDTO in noteDTO.TagList)
                {
                    Tag tag = new(tagDTO.ID, tagDTO.Title);

                    note.TryAddTag(tag);
                }

                response.AddItem(note);
            }

            return response;
        }

        public Response<Note> Update(int _noteId, Note _note)
        {
            throw new NotImplementedException();
        }

        public Response<Note> Delete(int _noteId)
        {
            throw new NotImplementedException();
        }
    }
}