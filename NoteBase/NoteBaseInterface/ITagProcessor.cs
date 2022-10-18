﻿using NoteBaseLogicInterface.Models;

namespace NoteBaseInterface
{
    public interface ITagProcessor
    {
        Response<Tag> Create(Tag _tag);
        Response<Tag> Delete(int _tagId);
        Response<Tag> Get(int _tagId);
        Response<Tag> Get(string _UserMail);
        Response<Tag> Update(int _tagId, Tag _tag);
    }
}