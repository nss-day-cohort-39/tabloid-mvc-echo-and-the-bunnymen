//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TabloidMVC.Models;

//namespace TabloidMVC.Repositories
//{
//    public class CommentRepository : BaseRepository
//    {
//        public CommentRepository(IConfiguration config) : base(config) { }
//        public List<Comment> GetAllComments()
//        {
//            using (var conn = Connection)
//            {
//                conn.Open();
//                using (var cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                       SELECT p.Id, p.Title, p.Content, 
//                              p.ImageLocation AS HeaderImage,
//                              p.CreateDateTime, p.PublishDateTime, p.IsApproved,
//                              p.CategoryId, p.UserProfileId,
//                              c.[Name] AS CategoryName,
//                              u.FirstName, u.LastName, u.DisplayName, 
//                              u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
//                              u.UserTypeId, 
//                              ut.[Name] AS UserTypeName
//                         FROM Post p
//                              LEFT JOIN Category c ON p.CategoryId = c.id
//                              LEFT JOIN UserProfile u ON p.UserProfileId = u.id
//                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
//                        WHERE IsApproved = 1 AND PublishDateTime < SYSDATETIME()
//                        ORDER BY PublishDateTime DESC
//                        ";

//                    var reader = cmd.ExecuteReader();

//                    var comments = new List<Comment>();

//                    while (reader.Read())
//                    {
//                        comments.Add(NewCommentFromReader(reader));
//                    }

//                    reader.Close();

//                    return comments;
//                }
//            }
//        }

//        public Comment GetCommentById(int id)
//        {
//            using (var conn = Connection)
//            {
//                conn.Open();
//                using (var cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                       SELECT p.Id, p.Title, p.Content, 
//                              p.ImageLocation AS HeaderImage,
//                              p.CreateDateTime, p.PublishDateTime, p.IsApproved,
//                              p.CategoryId, p.UserProfileId,
//                              c.[Name] AS CategoryName,
//                              u.FirstName, u.LastName, u.DisplayName, 
//                              u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
//                              u.UserTypeId, 
//                              ut.[Name] AS UserTypeName
//                         FROM Post p
//                              LEFT JOIN Category c ON p.CategoryId = c.id
//                              LEFT JOIN UserProfile u ON p.UserProfileId = u.id
//                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
//                        WHERE IsApproved = 1 AND PublishDateTime < SYSDATETIME()
//                              AND p.id = @id";

//                    cmd.Parameters.AddWithValue("@id", id);
//                    var reader = cmd.ExecuteReader();

//                    Comment comment = null;

//                    if (reader.Read())
//                    {
//                        comment = NewPostFromReader(reader);
//                    }

//                    reader.Close();

//                    return comment;
//                }
//            }
//        }

//        public Comment GetUserCommentById(int id, int userProfileId)
//        {
//            using (var conn = Connection)
//            {
//                conn.Open();
//                using (var cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                       SELECT p.Id, p.Title, p.Content, 
//                              p.ImageLocation AS HeaderImage,
//                              p.CreateDateTime, p.PublishDateTime, p.IsApproved,
//                              p.CategoryId, p.UserProfileId,
//                              c.[Name] AS CategoryName,
//                              u.FirstName, u.LastName, u.DisplayName, 
//                              u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
//                              u.UserTypeId, 
//                              ut.[Name] AS UserTypeName
//                         FROM Post p
//                              LEFT JOIN Category c ON p.CategoryId = c.id
//                              LEFT JOIN UserProfile u ON p.UserProfileId = u.id
//                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
//                        WHERE p.id = @id AND p.UserProfileId = @userProfileId";

//                    cmd.Parameters.AddWithValue("@id", id);
//                    cmd.Parameters.AddWithValue("@userProfileId", userProfileId);
//                    var reader = cmd.ExecuteReader();

//                    Comment comment = null;

//                    if (reader.Read())
//                    {
//                        comment = NewPostFromReader(reader);
//                    }

//                    reader.Close();

//                    return comment;
//                }
//            }
//        }


//        public List<Comment> GetAllCommentsByUserId(int userProfileId)
//        {
//            using (var conn = Connection)
//            {
//                conn.Open();
//                using (var cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                       SELECT p.Id, p.Title, p.Content, 
//                              p.ImageLocation AS HeaderImage,
//                              p.CreateDateTime, p.PublishDateTime, p.IsApproved,
//                              p.CategoryId, p.UserProfileId,
//                              c.[Name] AS CategoryName,
//                              u.FirstName, u.LastName, u.DisplayName, 
//                              u.Email, u.CreateDateTime, u.ImageLocation AS AvatarImage,
//                              u.UserTypeId, 
//                              ut.[Name] AS UserTypeName
//                         FROM Post p
//                              LEFT JOIN Category c ON p.CategoryId = c.id
//                              LEFT JOIN UserProfile u ON p.UserProfileId = u.id
//                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
//                        WHERE p.UserProfileId = @userProfileId
//                        ORDER BY PublishDateTime DESC
//                        ";
//                    cmd.Parameters.AddWithValue("@userProfileId", userProfileId);
//                    var reader = cmd.ExecuteReader();

//                    var comments = new List<Comment>();

//                    while (reader.Read())
//                    {
//                        comments.Add(NewPostFromReader(reader));
//                    }

//                    reader.Close();

//                    return comments;
//                }
//            }
//        }

//        public void Add(Comment comment)
//        {
//            using (var conn = Connection)
//            {
//                conn.Open();
//                using (var cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                        INSERT INTO Post (
//                            Title, Content, ImageLocation, CreateDateTime, PublishDateTime,
//                            IsApproved, CategoryId, UserProfileId )
//                        OUTPUT INSERTED.ID
//                        VALUES (
//                            @Title, @Content, @ImageLocation, @CreateDateTime, @PublishDateTime,
//                            @IsApproved, @CategoryId, @UserProfileId )";
//                    cmd.Parameters.AddWithValue("@Title", post.Title);
//                    cmd.Parameters.AddWithValue("@Content", post.Content);
//                    cmd.Parameters.AddWithValue("@ImageLocation", DbUtils.ValueOrDBNull(post.ImageLocation));
//                    cmd.Parameters.AddWithValue("@CreateDateTime", post.CreateDateTime);
//                    cmd.Parameters.AddWithValue("@PublishDateTime", DbUtils.ValueOrDBNull(post.PublishDateTime));
//                    cmd.Parameters.AddWithValue("@IsApproved", post.IsApproved);
//                    cmd.Parameters.AddWithValue("@CategoryId", post.CategoryId);
//                    cmd.Parameters.AddWithValue("@UserProfileId", post.UserProfileId);

//                    post.Id = (int)cmd.ExecuteScalar();
//                }
//            }
//        }

//        private Comment NewCommentFromReader(SqlDataReader reader)
//        {
//            return new Comment()
//            {
//                Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                Title = reader.GetString(reader.GetOrdinal("Title")),
//                Content = reader.GetString(reader.GetOrdinal("Content")),
//                ImageLocation = DbUtils.GetNullableString(reader, "HeaderImage"),
//                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
//                PublishDateTime = DbUtils.GetNullableDateTime(reader, "PublishDateTime"),
//                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
//                Category = new Category()
//                {
//                    Id = reader.GetInt32(reader.GetOrdinal("CategoryId")),
//                    Name = reader.GetString(reader.GetOrdinal("CategoryName"))
//                },
//                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
//                UserProfile = new UserProfile()
//                {
//                    Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
//                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                    DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
//                    Email = reader.GetString(reader.GetOrdinal("Email")),
//                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
//                    ImageLocation = DbUtils.GetNullableString(reader, "AvatarImage"),
//                    UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
//                    UserType = new UserType()
//                    {
//                        Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
//                        Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
//                    }
//                }
//            };
//        }

//        public void UpdateComment(Post post)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();

//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                            UPDATE Post
//                            SET 
//                                Title = @title, 
//                                Content = @content, 
//                                ImageLocation = @imageLocation,
//                                PublishDateTime = @publishDateTime, 
//                                CreateDateTime = @createDateTime,
//                                CategoryId = @categoryId
//                            WHERE Id = @id";

//                    cmd.Parameters.AddWithValue("@title", post.Title);
//                    cmd.Parameters.AddWithValue("@content", post.Content);
//                    cmd.Parameters.AddWithValue("@imageLocation", DbUtils.ValueOrDBNull(post.ImageLocation));
//                    cmd.Parameters.AddWithValue("@publishDateTime", DbUtils.ValueOrDBNull(post.PublishDateTime));
//                    cmd.Parameters.AddWithValue("@createDateTime", post.CreateDateTime);
//                    cmd.Parameters.AddWithValue("@categoryId", post.CategoryId);
//                    cmd.Parameters.AddWithValue("@id", post.Id);
//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }


//        public void DeleteComment(int commentId)
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();

//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                            DELETE FROM Comment
//                            WHERE Id = @id
//                        ";

//                    cmd.Parameters.AddWithValue("@id", commentId);

//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }
//    }
//}
