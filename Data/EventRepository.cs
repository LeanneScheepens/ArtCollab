using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtCollab.Models;
using Microsoft.Data.SqlClient;
using Logic.Interfaces;
using System.Data.SqlClient;

namespace Data
{
    public class EventRepository : IEventRepository
    {
        private readonly string _connectionString;

        public EventRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateEvent(Event evt)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
                INSERT INTO Event (Title, StartDate, EndDate, Description, Owner) 
                VALUES (@Title, @StartDate, @EndDate, @Description, @Owner); 
                SELECT SCOPE_IDENTITY();", connection);

            cmd.Parameters.AddWithValue("@Title", evt.Title);
            cmd.Parameters.AddWithValue("@StartDate", evt.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", evt.EndDate);
            cmd.Parameters.AddWithValue("@Description", evt.Description);
            cmd.Parameters.AddWithValue("@Owner", evt.Owner);

            evt.Id = Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<Event> GetAllEvents()
        {
            var events = new List<Event>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand("SELECT Id, Title, StartDate, EndDate, Description, Owner FROM Event", connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                events.Add(new Event(
                    reader.GetInt32(0),      // Id
                    reader.GetString(1),     // Title
                    reader.GetDateTime(2),   // StartDate
                    reader.GetDateTime(3),   // EndDate
                    reader.GetString(4),     // Description
                    reader.GetString(5)      // Owner
                ));
            }
            foreach (var evt in events)
            {
                evt.Artworks = GetArtworksByEventId(evt.Id);
            }
            return events;
        }
        private List<Artwork> GetArtworksByEventId(int eventId)
        {
            var artworks = new List<Artwork>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand(@"
        SELECT a.Id, a.Title, a.Description, a.Owner, a.UploadDate, a.ImageUrl
        FROM Artwork a
        INNER JOIN EventArtworks ea ON ea.ArtworkId = a.Id
        WHERE ea.EventId = @EventId", connection);

            cmd.Parameters.AddWithValue("@EventId", eventId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                artworks.Add(new Artwork(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetDateTime(4),
                    reader.GetString(5)
                ));
            }

            return artworks;
        }

        public Event GetEventById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var cmd = new SqlCommand("SELECT Id, Title, StartDate, EndDate, Description, Owner FROM Event WHERE Id = @Id", connection);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Event(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetDateTime(2),
                    reader.GetDateTime(3),
                    reader.GetString(4),
                    reader.GetString(5)
                );
            }

            return null;
        }

        public void AddArtworksToEvent(int eventId, List<int> artworkIds)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            foreach (var artworkId in artworkIds)
            {
                var cmd = new SqlCommand("INSERT INTO EventArtworks (EventId, ArtworkId) VALUES (@EventId, @ArtworkId)", connection);
                cmd.Parameters.AddWithValue("@EventId", eventId);
                cmd.Parameters.AddWithValue("@ArtworkId", artworkId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}