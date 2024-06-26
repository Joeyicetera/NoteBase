﻿using NoteBaseDALInterface.Models;
using NoteBaseDALInterface;
using NoteBaseInterface;
using NoteBaseLogicInterface.Models;

namespace NoteBaseLogic
{
    public class TagProcessor : ITagProcessor
    {
        private readonly ITagDAL TagDAL;

        public TagProcessor(ITagDAL _tagDAL)
        {
            TagDAL = _tagDAL;
        }

        public bool IsValidTitle(string _title)
        {
            //needs work (entering just spaces should not be seen as valid)
            return _title != "";
        }

        //rename
        public void CreateTags(string _text, int NoteId)
        {
            List<Tag> tags = ExtractTags(_text);

            foreach (Tag newtag in tags)
            {
                Tag tag = GetByTitle(newtag.Title);

                if (tag.ID == 0)
                {
                    Create(newtag.Title);
                    tag = GetByTitle(newtag.Title);
                }

                TagDAL.CreateNoteTag(NoteId, tag.ID);
            }
        }

        //rename
        public void UpdateTags(int _noteId, string _text, int _personId, List<Tag> _tags)
        {
            if (_tags.Count > 0)
            {
                TagDAL.DeleteNoteTag(_noteId);
            }

            List<Tag> newTags = ExtractTags(_text);

            foreach (Tag newtag in newTags)
            {
                Tag tag = GetByTitle(newtag.Title);

                if (tag.ID == 0)
                {
                    Create(newtag.Title);
                    tag = GetByTitle(newtag.Title);
                }

                TagDAL.CreateNoteTag(_noteId, tag.ID);
            }

            foreach (Tag tag in _tags)
            {
                //get all tags with same title. if there are non delete the tag
                if (newTags.Where(t => t.Title == tag.Title).Count() == 0)
                {
                    DeleteWhenUnused(tag.ID, _personId);
                }
            }
        }

        //what if somebody usses a tag with a hashtag in it like #C#
        private List<Tag> ExtractTags(string _text)
        {
            List<Tag> NewTagList = new();

            string[] allWords = _text.Split(" ");
            for (int i = 0; i < allWords.Length; i++)
            {
                string word = allWords[i];
                if (word.StartsWith("#"))
                {
                    Tag tag = new(0, word[1..].ToLower());
                    if (!NewTagList.Contains(tag))
                    {
                        NewTagList.Add(tag);
                    }
                }
            }

            return NewTagList;
        }

        public Tag Create(string _title)
        {
            if (!IsValidTitle(_title))
            {
                throw new ArgumentException("Title can't be empty");
            }

            TagDTO tagDTO = TagDAL.Create(_title);
            return new(tagDTO.ID, tagDTO.Title);
        }

        public Tag GetById(int _tagId)
        {
            TagDTO tagDTO = TagDAL.GetById(_tagId);

            return new(tagDTO.ID, tagDTO.Title);
        }

        //need to remake this for using person id
        public List<Tag> GetByPerson(int _personId)
        {
            List<Tag> tagList = new();

            List<TagDTO> tagDTOs = TagDAL.GetByPerson(_personId);
            foreach (TagDTO tagDTO in tagDTOs)
            {
                tagList.Add(new(tagDTO.ID, tagDTO.Title));
            }

            return tagList;
        }

        public Tag GetByTitle(string _title)
        {
            TagDTO tagDTO = TagDAL.GetByTitle(_title);

            return new(tagDTO.ID, tagDTO.Title);
        }

        public void DeleteWhenUnused(int _tagId, int _PersonId)
        {
            //get all used tags by person
            List<Tag> tagList = GetByPerson(_PersonId);

            //is tag in use? return
            foreach (Tag tag in tagList)
            {
                if (tag.ID == _tagId)
                {
                    return;
                }
            }

            //else delete it
            TagDAL.Delete(_tagId);
        }

        public void DeleteNoteTag(int _noteId)
        {
            TagDAL.DeleteNoteTag(_noteId);
        }
    }
}
