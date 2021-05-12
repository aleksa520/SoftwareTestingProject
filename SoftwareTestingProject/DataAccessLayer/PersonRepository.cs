using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.DataAccessLayer
{
    public class PersonRepository : IPersonRepository
    {
        DatabaseBroker broker;
        SqlTransaction transaction;

        public PersonRepository()
        {
            broker = DatabaseBroker.Session();
        }

        public string Delete(Person entity)
        {
            transaction = null;
            try
            {
                broker.Connection.Open();
                transaction = broker.Connection.BeginTransaction();
                broker.Command =
                    new SqlCommand("", broker.Connection, transaction)
                    {
                        CommandText =
                    $"delete from Person where PersonId = {entity.PersonId}"
                    };

                broker.Command.ExecuteNonQuery();
                Save();
                return $"Person {entity.FirstName} {entity.LastName} is successfully deleted!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.Message.ToString();
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        public string Insert(Person entity)
        {
            transaction = null;
            string message = string.Empty;

            try
            {
                entity.PersonId = broker.GetId("PersonId", "Person");
                broker.Connection.Open();

                broker.Command =
                    new SqlCommand("PROC_INSERT_PERSON", broker.Connection);
                broker.Command.CommandType = CommandType.StoredProcedure;

                broker.Command.Parameters.AddWithValue("PersonId", Convert.ToInt32(entity.PersonId));
                broker.Command.Parameters.AddWithValue("RegistrationNumber", SqlDbType.VarChar).Value = entity.RegistrationNumber;
                broker.Command.Parameters.AddWithValue("FirstName", SqlDbType.NVarChar).Value = entity.FirstName;
                broker.Command.Parameters.AddWithValue("LastName", SqlDbType.NVarChar).Value = entity.LastName;
                if(entity.Height == 0)
                {
                    broker.Command.Parameters.AddWithValue("Height", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Height", entity.Height);
                }

                if (entity.Weight == 0)
                {
                    broker.Command.Parameters.AddWithValue("Weight", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Weight", entity.Weight);
                }
                if (entity.EyeCollor == 0)
                {
                    broker.Command.Parameters.AddWithValue("EyeCollor", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("EyeCollor", entity.EyeCollor);
                }
                if (String.IsNullOrEmpty(entity.PhoneNumber))
                {
                    broker.Command.Parameters.AddWithValue("PhoneNumber", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("PhoneNumber", entity.PhoneNumber);
                }
                broker.Command.Parameters.AddWithValue("Email", SqlDbType.VarChar).Value = entity.Email;
                broker.Command.Parameters.AddWithValue("DateOfBirth", SqlDbType.Date).Value = entity.DateOfBirth;
                if (String.IsNullOrEmpty(entity.Address))
                {
                    broker.Command.Parameters.AddWithValue("Address", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Address", entity.Address);
                }
                if (String.IsNullOrEmpty(entity.Note))
                {
                    broker.Command.Parameters.AddWithValue("Note", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Note", entity.Note);
                }
                broker.Command.Parameters.AddWithValue("PlaceId", SqlDbType.Int).Value = entity.PlaceId;

                int result = broker.Command.ExecuteNonQuery();

                if (result != -1)
                {
                    message = $"{entity.FirstName} {entity.LastName} successfully inserted!";
                }
                else
                {
                    throw new Exception("There was error inserting a person!");
                }
                return message;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        public void Save()
        {
            transaction.Commit();
        }

        public IEnumerable<Person> SelectAll()
        {
            try
            {
                List<Person> people = new List<Person>();

                broker.Connection.Open();
                broker.Command = new SqlCommand("", broker.Connection, transaction);
                broker.Command.CommandText = $"Select * from Person person join Place place on (person.PlaceId = place.PlaceId)";

                SqlDataReader reader = broker.Command.ExecuteReader();

                while (reader.Read())
                {
                    Person person = new Person
                    {
                        PersonId = reader.GetInt32(0),
                        RegistrationNumber = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Height = GetIntValueOrDefault(reader, "Height"),
                        Weight = GetIntValueOrDefault(reader, "Weight"),
                        EyeCollor = (EyeCollor)GetIntValueOrDefault(reader, "EyeCollor"),
                        PhoneNumber = GetStringValueOrDefault(reader, "PhoneNumber"),
                        Email = reader.GetString(8),
                        DateOfBirth = reader.GetDateTime(9),
                        Address = GetStringValueOrDefault(reader, "Address"),
                        Note = GetStringValueOrDefault(reader, "Note"),
                        Place = new Place
                        {
                            PlaceId = reader.GetInt32(12),
                            Name = reader.GetString(14),
                            Zipcode = reader.GetInt32(15),
                            Population = GetIntValueOrDefault(reader, "Population")
                        }

                    };
                    people.Add(person);
                }
                reader.Close();

                return people;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        public Person SelectById(int? id)
        {
            Person person = null;
            try
            {
                broker.Connection.Open();
                broker.Command = new SqlCommand("", broker.Connection, transaction);
                broker.Command.CommandText = $"Select * from Person person join Place place on (person.PlaceId = place.PlaceId) where PersonId = {id}";
                SqlDataReader reader = broker.Command.ExecuteReader();

                while (reader.Read())
                {
                    person = new Person
                    {
                        PersonId = reader.GetInt32(0),
                        RegistrationNumber = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Height = GetIntValueOrDefault(reader, "Height"),
                        Weight = GetIntValueOrDefault(reader, "Weight"),
                        EyeCollor = (EyeCollor)GetIntValueOrDefault(reader, "EyeCollor"),
                        PhoneNumber = GetStringValueOrDefault(reader, "PhoneNumber"),
                        Email = reader.GetString(8),
                        DateOfBirth = reader.GetDateTime(9),
                        Address = GetStringValueOrDefault(reader, "Address"),
                        Note = GetStringValueOrDefault(reader, "Note"),
                        Place = new Place
                        {
                            PlaceId = reader.GetInt32(12),
                            Name = reader.GetString(14),
                            Zipcode = reader.GetInt32(15),
                            Population = GetIntValueOrDefault(reader, "Population")
                        }
                    };
                }
                reader.Close();
                person.PlaceId = (int)person.Place.PlaceId;
                return person;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        public string Update(Person entity)
        {
            transaction = null;

            try
            {
                broker.Connection.Open();
                transaction = broker.Connection.BeginTransaction();
                broker.Command =
                    new SqlCommand("", broker.Connection, transaction)
                    {
                        CommandText =
                    $"update Person set RegistrationNumber ='{entity.RegistrationNumber}', FirstName = '{entity.FirstName}', LastName = '{entity.LastName}', DateOfBirth = '{entity.DateOfBirth}', Note = @Note, Address = @Address, Height = @Height, Weight = @Weight, EyeCollor = @EyeCollor, PhoneNumber = @PhoneNumber, Email = '{entity.Email}', PlaceId = {entity.PlaceId} where PersonId = {entity.PersonId}"
                    };
                if (String.IsNullOrEmpty(entity.Note))
                {
                    broker.Command.Parameters.AddWithValue("@Note", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@Note", entity.Note);
                }
                if (String.IsNullOrEmpty(entity.Address))
                {
                    broker.Command.Parameters.AddWithValue("@Address", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@Address", entity.Address);
                }
                if (entity.Height == 0)
                {
                    broker.Command.Parameters.AddWithValue("@Height", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@Height", entity.Height);
                }
                if (entity.Weight == 0)
                {
                    broker.Command.Parameters.AddWithValue("@Weight", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@Weight", entity.Weight);
                }
                if (entity.EyeCollor == 0)
                {
                    broker.Command.Parameters.AddWithValue("@EyeCollor", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@EyeCollor", entity.EyeCollor);
                }
                if (String.IsNullOrEmpty(entity.PhoneNumber))
                {
                    broker.Command.Parameters.AddWithValue("@PhoneNumber", DBNull.Value);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("@PhoneNumber", entity.PhoneNumber);
                }

                broker.Command.ExecuteNonQuery();

                Save();

                return "Successful!";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ex.Message.ToString();
            }
            finally
            {
                if (broker.Connection != null)
                    broker.Connection.Close();
            }
        }

        private string GetStringValueOrDefault(SqlDataReader reader, string column)
        {
            string data = (reader.IsDBNull(reader.GetOrdinal(column)))
                      ? "" : reader[column].ToString();
            return data;
        }

        public int GetIntValueOrDefault(SqlDataReader reader, string column)
        {
            int data = (reader.IsDBNull(reader.GetOrdinal(column)))
                        ? 0 : int.Parse(reader[column].ToString());
            return data;
        }
    }
}
