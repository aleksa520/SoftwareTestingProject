using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.DataAccessLayer
{
    public class PlaceRepository : IPlaceRepository
    {
        DatabaseBroker broker;
        SqlTransaction transaction;

        public PlaceRepository()
        {
            broker = DatabaseBroker.Session();
        }

        public string Delete(Place entity)
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
                    $"delete from Place where PlaceId = {entity.PlaceId}"
                    };

                broker.Command.ExecuteNonQuery();
                Save();
                return $"Place {entity.Name} is successfully deleted!";
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

        public string Insert(Place entity)
        {
            transaction = null;
            string message = string.Empty;

            try
            {
                entity.PlaceId = broker.GetId("PlaceId", "Place");
                broker.Connection.Open();

                broker.Command =
                    new SqlCommand("PROC_INSERT_PLACE", broker.Connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                broker.Command.Parameters.AddWithValue("PlaceId", Convert.ToInt32(entity.PlaceId));
                broker.Command.Parameters.AddWithValue("Name", SqlDbType.VarChar).Value = entity.Name;
                broker.Command.Parameters.AddWithValue("Zipcode", SqlDbType.Int).Value = entity.Zipcode;
                if (entity.Population != null)
                    broker.Command.Parameters.AddWithValue("Population", SqlDbType.Int).Value = entity.Population;
                else
                    broker.Command.Parameters.AddWithValue("Population", DBNull.Value);

                int result = broker.Command.ExecuteNonQuery();

                if (result != -1)
                {
                    message = $"{entity.Name} successfully inserted!";
                }
                else
                {
                    throw new Exception("There was error inserting a place!");
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

        public IEnumerable<Place> SelectAll()
        {
            try
            {
                List<Place> places = new List<Place>();

                broker.Connection.Open();
                broker.Command = new SqlCommand("", broker.Connection, transaction)
                {
                    CommandText = $"Select * from Place"
                };

                SqlDataReader reader = broker.Command.ExecuteReader();

                while (reader.Read())
                {

                    Place place = new Place
                    {
                        PlaceId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Zipcode = reader.GetInt32(2),
                        Population = GetIntValueOrDefault(reader, "Population")
                    };
                    places.Add(place);
                }
                reader.Close();

                return places;
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

        public Place SelectById(int? id)
        {
            Place place = new Place();
            try
            {
                broker.Connection.Open();
                broker.Command = new SqlCommand("", broker.Connection, transaction)
                {
                    CommandText = $"Select * from Place where PlaceId={id}"
                };
                SqlDataReader reader = broker.Command.ExecuteReader();
                List<Person> people = new List<Person>();

                while (reader.Read())
                {
                    place.PlaceId = reader.GetInt32(0);
                    place.Name = reader.GetString(1);
                    place.Zipcode = reader.GetInt32(2);
                    place.Population = GetIntValueOrDefault(reader, "Population");

                }
                reader.Close();

                broker.Command = new SqlCommand("", broker.Connection, transaction)
                {
                    CommandText = $"Select * from Person where PlaceId={id}"
                };

                reader = broker.Command.ExecuteReader();

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
                        Place = place
                    };
                    people.Add(person);
                }
                reader.Close();

                place.People = people;
                return place;
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

        public string Update(Place entity)
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
                    $"update Place set Name ='{entity.Name}', Zipcode = {entity.Zipcode}, Population =  @Population where PlaceId = {entity.PlaceId}"
                    };
                if (entity.Population != null)
                {
                    broker.Command.Parameters.AddWithValue("Population", entity.Population);
                }
                else
                {
                    broker.Command.Parameters.AddWithValue("Population", DBNull.Value);
                }
                broker.Command.ExecuteNonQuery();

                Save();

                return "Successfully updated!";
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
