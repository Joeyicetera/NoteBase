﻿using NoteBaseDALInterface;
using NoteBaseDALInterface.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NoteBaseDAL
{
    public class TagDAL : ITagDAL
    {
        private readonly string ConnString;

        public TagDAL(string _connString)
        {
            ConnString = _connString;
        }

        public DALResponse<TagDTO> Create(TagDTO _tag)
        {
            DALResponse<TagDTO> response = new(true);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string query = @"INSERT INTO Tag (Title) VALUES (@Title)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", _tag.Title);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            int result = reader.GetInt32(0);
                            if (result == 0)
                            {
                                response.Succeeded = false;
                                response.Message = "TagDAL.Create(" + _tag.Title + ") ERROR: Could not Create Tag";
                            }
                        }
                    }
                }
            }
            //het opvangen van een mogelijke error
            catch (SqlException e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Create(" + _tag.Title + ") ERROR: " + e.Message,
                    Code = e.Number
                };
            }
            catch (Exception e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Create(" + _tag.Title + ") ERROR: " + e.Message
                };
            }

            return response;
        }

        public DALResponse<TagDTO> GetById(int _tagId)
        {
            DALResponse<TagDTO> response = new(true);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string query = @"SELECT ID, Title From Tag WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", _tagId);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            TagDTO tripDTO = new TagDTO(reader.GetInt32(0), reader.GetString(1));

                            response.AddItem(tripDTO);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Get(" + _tagId + ") ERROR: " + e.Message,
                    Code = e.Number
                };
            }
            catch (Exception e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Get(" + _tagId + ") ERROR: " + e.Message
                };
            }

            return response;
        }

        //need to remake this for using person id
        public DALResponse<TagDTO> GetByPerson(int _PersonId)
        {
            DALResponse<TagDTO> response = new(true);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string query = @"SELECT ID, Title FROM NoteTags WHERE PersonId = @PersonId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonId", _PersonId);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            TagDTO tripDTO = new TagDTO(reader.GetInt32(0), reader.GetString(1));

                            response.AddItem(tripDTO);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Get(" + _PersonId + ") ERROR: " + e.Message,
                    Code = e.Number
                };
            }
            catch (Exception e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Get(" + _PersonId + ") ERROR: " + e.Message,
                };
            }

            return response;
        }

        public DALResponse<TagDTO> GetByNote(int _noteId)
        {
            DALResponse<TagDTO> response = new(true);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string query = @"SELECT ID, Title FROM NoteTags WHERE NoteID = @NoteId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NoteId", _noteId);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            TagDTO tripDTO = new TagDTO(reader.GetInt32(0), reader.GetString(1));

                            response.AddItem(tripDTO);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Get(" + _noteId + ") ERROR: " + e.Message,
                    Code = e.Number
                };
            }
            catch (Exception e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Get(" + _noteId + ") ERROR: " + e.Message
                };
            }

            return response;
        }

        public DALResponse<TagDTO> GetByTitle(string _Title)
        {
            DALResponse<TagDTO> response = new(true);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string query = @"SELECT ID, Title From Tag WHERE Title = @Title";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", _Title);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            TagDTO tripDTO = new TagDTO(reader.GetInt32(0), reader.GetString(1));

                            response.AddItem(tripDTO);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                response = new(false)
                {
                    Message = "TagDAL.GetByTitle(" + _Title + ") ERROR: " + e.Message,
                    Code = e.Number
                };
            }
            catch (Exception e)
            {
                response = new(false)
                {
                    Message = "TagDAL.GetByTitle(" + _Title + ") ERROR: " + e.Message
                };
            }

            return response;
        }

        /*public DALResponse<TagDTO> GetTagWithNote()
        {
            DALResponse<TagDTO> response = new(true);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string query = @"SELECT DISTINCT TagID From NoteTag";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            TagDTO tripDTO = new TagDTO(reader.GetInt32(0), reader.GetString(1));

                            response.AddItem(tripDTO);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                response = new(false)
                {
                    Message = "TagDAL.GetTagWithNote() ERROR: " + e.Message,
                    Code = e.Number
                };
            }
            catch (Exception e)
            {
                response = new(false)
                {
                    Message = "TagDAL.GetTagWithNote() ERROR: " + e.Message
                };
            }

            return response;
        }*/

        public DALResponse<TagDTO> Update(TagDTO _tag)
        {
            DALResponse<TagDTO> response = new(true);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string query = @"UPDATE Tag SET Title = @Title WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", _tag.Title);
                        command.Parameters.AddWithValue("@ID", _tag.ID);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            int result = reader.GetInt32(0);
                            if (result == 0)
                            {
                                response.Succeeded = false;
                                response.Message = "TagDAL.Update(" + _tag.ID + ",TagDTO) ERROR: Could not update Tag";
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Update(" + _tag.ID + ", TagDTO) ERROR: " + e.Message,
                    Code = e.Number
                };
            }
            catch (Exception e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Update(" + _tag.ID + ", TagDTO) ERROR: " + e.Message
                };
            }

            return response;
        }

        public DALResponse<TagDTO> Delete(int _tagId)
        {
            DALResponse<TagDTO> response = new(true);

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string query = @"DELETE From Tag WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", _tagId);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            int result = reader.GetInt32(0);
                            if (result == 0)
                            {
                                response.Succeeded = false;
                                response.Message = "TagDAL.Delete(" + _tagId + ") ERROR: Could not delete Tag";
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Delete(" + _tagId + ") ERROR: " + e.Message,
                    Code = e.Number
                };
            }
            catch (Exception e)
            {
                response = new(false)
                {
                    Message = "TagDAL.Delete(" + _tagId + ") ERROR: " + e.Message,
                };
            }

            return response;
        }
    }
}
