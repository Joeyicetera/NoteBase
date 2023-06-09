﻿using NoteBaseDALInterface;
using NoteBaseDALInterface.Models;
using System.Data.SqlClient;

namespace NoteBaseDAL
{
    public class NoteDAL : INoteDAL
    {
        private readonly string ConnString;
        private readonly TagDAL TagDAL;

        public NoteDAL(string _connString)
        {
            ConnString = _connString;
            TagDAL = new(_connString);
        }

        public NoteDTO Create(string _title, string _text, int _categoryId, int _personId)
        {
            NoteDTO result = new(0, _title, _text, _categoryId, _personId);

            using (SqlConnection connection = new(ConnString))
            {
                string query = @"INSERT INTO Note (Title, Text, CategoryID, PersonId) VALUES (@Title, @Text, @CategoryID, @PersonId); SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", _title);
                    command.Parameters.AddWithValue("@Text", _text);
                    command.Parameters.AddWithValue("@CategoryID", _categoryId);
                    command.Parameters.AddWithValue("@PersonId", _personId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if(reader.Read())
                    {
                        result = new((Int32)reader.GetDecimal(0), _title, _text, _categoryId, _personId);
                    }

                    connection.Close();
                }
            }

            return result;
        }

        public NoteDTO GetById(int _noteId)
        {
            NoteDTO result = new(0, "", "", 0, 0);

            using (SqlConnection connection = new(ConnString))
            {
                string query = @"SELECT ID, Title, Text, CategoryID, PersonId From Note WHERE ID = @ID";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", _noteId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        NoteDTO noteDTO = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));

                        List<TagDTO> tags = TagDAL.GetByNote(noteDTO.ID);

                        foreach (TagDTO tagDTO in tags)
                        {
                            noteDTO.tagList.Add(tagDTO);
                        }

                        result = noteDTO;
                    }
                    connection.Close();
                }
            }

            return result;
        }

        public List<NoteDTO> GetByPerson(int _personId)
        {
            List<NoteDTO> result = new();

            using (SqlConnection connection = new(ConnString))
            {
                string query = @"SELECT ID, Title, Text, CategoryId, PersonId FROM Note WHERE PersonId = @PersonId";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonId", _personId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        NoteDTO noteDTO = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));

                        List<TagDTO> tags = TagDAL.GetByNote(noteDTO.ID);

                        foreach (TagDTO tagDTO in tags)
                        {
                            noteDTO.tagList.Add(tagDTO);
                        }

                        result.Add(noteDTO);
                    }
                    connection.Close();
                }
            }

            return result;
        }

        public NoteDTO GetByTitle(string _title)
        {
            NoteDTO result = new(0, "", "", 0, 0);

            using (SqlConnection connection = new(ConnString))
            {
                string query = @"SELECT ID, Title, Text, CategoryID, PersonId From Note WHERE Title = @Title";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", _title);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        NoteDTO noteDTO = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));

                        List<TagDTO> tags = TagDAL.GetByNote(noteDTO.ID);

                        foreach (TagDTO tagDTO in tags)
                        {
                            noteDTO.tagList.Add(tagDTO);
                        }

                        result = noteDTO;
                    }
                    connection.Close();
                }
            }

            return result;
        }

        public List<NoteDTO> GetByCategory(int _catId)
        {
            List<NoteDTO> result = new();

            using (SqlConnection connection = new(ConnString))
            {
                string query = @"SELECT ID, Title, Text, CategoryId, PersonId FROM Note WHERE CategoryId = @categoryId";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@categoryId", _catId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        NoteDTO noteDTO = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));

                        List<TagDTO> tags = TagDAL.GetByNote(noteDTO.ID);

                        foreach (TagDTO tagDTO in tags)
                        {
                            noteDTO.tagList.Add(tagDTO);
                        }

                        result.Add(noteDTO);
                    }
                    connection.Close();
                }
            }

            return result;
        }

        public List<NoteDTO> GetByTag(int _tagId)
        {
            List<NoteDTO> result = new();

            using (SqlConnection connection = new(ConnString))
            {
                string query = @"SELECT ID, Title, Text, CategoryId, PersonId FROM TagNotes WHERE TagID = @tagId";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@tagId", _tagId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        NoteDTO noteDTO = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));

                        List<TagDTO> tags = TagDAL.GetByNote(noteDTO.ID);

                        foreach (TagDTO tagDTO in tags)
                        {
                            noteDTO.tagList.Add(tagDTO);
                        }

                        result.Add(noteDTO);
                    }
                    connection.Close();
                }
            }

            return result;
        }

        public NoteDTO Update(int _id, string _title, string _text, int _categoryId)
        {
            NoteDTO result = new(_id, _title, _text, _categoryId, 0);

            using (SqlConnection connection = new(ConnString))
            {
                string query = @"UPDATE Note SET Title = @Title, Text = @Text, CategoryID = @CategoryID WHERE ID = @ID;";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", _title);
                    command.Parameters.AddWithValue("@Text", _text);
                    command.Parameters.AddWithValue("@CategoryID", _categoryId);
                    command.Parameters.AddWithValue("@ID", _id);
                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }

            return result;
        }

        public void Delete(int _noteId)
        {
            using (SqlConnection connection = new(ConnString))
            {
                string query = @"DELETE FROM Note WHERE ID = @ID";

                using (SqlCommand command = new(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", _noteId);
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    //is dit nodig?
                    if (rowsAffected == 0)
                    {
                        throw new Exception("Note could not be deleted");
                    }

                    connection.Close();
                }
            }
        }
    }
}