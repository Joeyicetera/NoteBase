﻿using NoteBaseDALFactory;
using NoteBaseDALInterface.Models;
using NoteBaseDALInterface;
using NoteBaseInterface;
using NoteBaseLogicInterface.Models;

namespace NoteBaseLogic
{
    public class TagProcessor : IProcessor<Tag>
    {
        private readonly IDAL<TagDTO> TagDAL;
        public TagProcessor(string _connString)
        {
            TagDAL = Factory.CreateTagDAL(_connString);
        }

        public TagProcessor(IDAL<TagDTO> _tagDAL)
        {
            TagDAL = _tagDAL;
        }

        public Response<Tag> Create(Tag _tag)
        {
            DALResponse<TagDTO> DALreponse = TagDAL.Create(_tag.ToDTO());

            //create response
            Response<Tag> response = new(DALreponse.Status, DALreponse.Message);

            return response;
        }

        public Response<Tag> Delete(int _tagId)
        {
            DALResponse<TagDTO> DALreponse = TagDAL.Delete(_tagId);

            List<TagDTO> resposeTagDTO = (List<TagDTO>)DALreponse.Data;
            IModel<TagDTO> tag = new Tag(resposeTagDTO[0].ID, resposeTagDTO[0].Title);

            //create response
            Response<Tag> response = new(DALreponse.Status, DALreponse.Message);
            response.AddItem(tag);

            return response;
        }

        public Response<Tag> Get(int _tagId)
        {
            DALResponse<TagDTO> DALreponse = TagDAL.Get(_tagId);

            List<TagDTO> resposeTagDTO = (List<TagDTO>)DALreponse.Data;
            Tag tag = new(resposeTagDTO[0].ID, resposeTagDTO[0].Title);

            //create response
            Response<Tag> response = new(DALreponse.Status, DALreponse.Message);
            response.AddItem(tag);

            return response;
        }

        public Response<Tag> Get(string _UserMail)
        {
            DALResponse<TagDTO> DALreponse = TagDAL.Get(_UserMail);
            Response<Tag> response = new(DALreponse.Status, DALreponse.Message);

            foreach (TagDTO tagDTO in DALreponse.Data)
            {
                Tag tag = new(tagDTO.ID, tagDTO.Title);
                response.AddItem(tag);
            }

            return response;
        }

        public Response<Tag> Update(int _tagId, Tag _tag)
        {
            TagDTO tagDTO = new(_tag.ID, _tag.Title);

            DALResponse<TagDTO> DALreponse = TagDAL.Update(_tagId, tagDTO);

            List<TagDTO> resposeTagDTO = (List<TagDTO>)DALreponse.Data;

            //create response
            Response<Tag> response = new(DALreponse.Status, DALreponse.Message);

            return response;
        }
    }
}
