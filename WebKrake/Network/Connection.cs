using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WebKrake.Models;

namespace WebKrake.Network
{
    public class Connection
    {
        public MySqlConnection conn { get; }

        public Connection()
        {

            string connectionString = "server=localhost;uid=root;port=3306;pwd=;database=krake;";   //DEBUG 
            conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public int getAccount(Account account)
        {
            int error = OpenDBConnection();
            if(error != 200)
            {
                return error;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM account WHERE name = '" + account.Name + "' AND password = '" + account.Password + "'", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();
                    return 200;
                }
                else
                {
                    reader.Close();
                    conn.Close();
                    return 400;
                }
            }
            catch (Exception)
            {
                conn.Close();
                return 501;
            }            
        }


        public int AddEvent(Event e, string table)
        {
            int error = OpenDBConnection();
            if (error != 200)
            {
                return error;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO " + table + "(Id,Title,Location,Text,Image,City,Time,Entryfee,Day,Month,Year,Sponsored) VALUES(@Id,@Title,@Location,@Txt,@Image,@City,@Time,@Entryfee,@Day,@Month,@Year,@Sponsored)", conn);

                var pointIndex = e.Date.IndexOf("-");
                var point2 = e.Date.LastIndexOf("-");
                var year = e.Date.Substring(0, pointIndex);
                var length = point2 - (pointIndex + 1);
                var month = e.Date.Substring(pointIndex + 1, length);
                var day = e.Date.Substring(point2 + 1, 2);
                var sponsored = 0;
                if (e.Sponsored == null)
                {
                    sponsored = 0;
                }
                else
                {
                    sponsored = 1;
                }

                cmd.Parameters.AddWithValue("@Id", e.Id);
                cmd.Parameters.AddWithValue("@Title", e.Title);
                cmd.Parameters.AddWithValue("@Location", e.Description);
                cmd.Parameters.AddWithValue("@Txt", e.Text);
                cmd.Parameters.AddWithValue("@Image", e.Image);
                cmd.Parameters.AddWithValue("@City", e.City);
                cmd.Parameters.AddWithValue("@Time", e.Time);
                cmd.Parameters.AddWithValue("@Entryfee", e.Entryfee);
                cmd.Parameters.AddWithValue("@Day", day);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Sponsored", sponsored);

                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                return 501;
            }
            conn.Close();
            return 200;
        }

        public int AddLocation(Location l)
        {
            int error = OpenDBConnection();
            if (error != 200)
            {
                return error;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO location(Id,Name,Street,Image,City,Text,Bar) VALUES(@Id,@Name,@Street,@Image,@City,@Text,@Bar)", conn);

                cmd.Parameters.AddWithValue("@Id", l.Id);
                cmd.Parameters.AddWithValue("@Name", l.Name);
                cmd.Parameters.AddWithValue("@Street", l.Street);
                cmd.Parameters.AddWithValue("@Image", l.Image);
                cmd.Parameters.AddWithValue("@City", l.City);
                cmd.Parameters.AddWithValue("@Text", l.Text);
                cmd.Parameters.AddWithValue("@Bar", l.Bar);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return 501;
            }
            conn.Close();
            return 200;
        }

        /// <summary>
        /// Event Objekt steht für Party und Bar objekt
        /// wird durch BOOL Bar differeniert
        /// In die Beschreibung kommt der Name der Location 
        /// </summary>
        /// <returns></returns>
        public List<Event> GetEvents(string city, string table, string location = null)
        {
            int error = OpenDBConnection();
            List<Event> events = new List<Event>();

            if (error != 200)
            {
                Event e = new Event
                {
                    Id = "-" + error.ToString(),
                    Title = "",
                    Description = "",
                    Text = "",
                    Image = "",
                    City = "",
                    Date = "",
                    Time = "",
                    Entryfee = "",
                    Sponsored = ""
                };
                events.Add(e);
                return events;
            }
            try
            {
                int thisTime = DateTime.Now.Hour;
                int thisDay = DateTime.Today.Day;
                int thisYear = DateTime.Today.Year;
                int yesterday = thisDay - 1;
                int thisMonth = DateTime.Today.Month;
                DateTime today = DateTime.Today;
                MySqlCommand cmd;

                if (thisTime > 6)
                {
                    if(location == null)
                    {
                        cmd = new MySqlCommand("SELECT * FROM " + table + " WHERE City = '" + city + "' AND Year = " + thisYear + " AND Month = " + thisMonth + " AND Day >= " + thisDay + " OR City = '" + city + "' AND Month > " + thisMonth + " AND Year = " + thisYear + " OR City = '" + city + "' AND Year > " + thisYear + " ORDER BY Sponsored DESC, Year ASC, Month ASC, Day ASC, Time ASC", conn);
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT * FROM party WHERE City = '" + city + "' AND Location = '" + location + "' AND Year = " + thisYear + " AND Month = " + thisMonth + " AND Day >= " + thisDay + " OR City = '" + city + "' AND Location LIKE '" + location + "' AND Month > " + thisMonth + " AND Year = " + thisYear + " OR City = '" + city + "' AND Location LIKE '" + location + "' AND Year > " + thisYear + " ORDER BY Sponsored DESC, Year ASC, Month ASC, Day ASC, Time ASC", conn);
                    }
                }
                else
                {
                    if (location == null)
                    {
                        cmd = new MySqlCommand("SELECT * FROM " + table + " WHERE City = '" + city + "' AND Year = " + thisYear + " AND Month = " + thisMonth + " AND Day >= " + yesterday + " OR City = '" + city + "' AND Month > " + thisMonth + " AND Year = " + thisYear + " OR City = '" + city + "' AND Year > " + thisYear + " ORDER BY Sponsored DESC, Year ASC, Month ASC, Day ASC, Time ASC", conn);
                    }
                    else
                    {
                        cmd = new MySqlCommand("SELECT * FROM party WHERE City = '" + city + "' AND Location = '" + location + "' AND Year = " + thisYear + " AND Month = " + thisMonth + " AND Day >= " + yesterday + " OR City = '" + city + "' AND Location LIKE '" + location + "' AND Month > " + thisMonth + " AND Year = " + thisYear + " OR City = '" + city + "' AND Location LIKE '" + location + "' AND Year > " + thisYear + " ORDER BY Sponsored DESC, Year ASC, Month ASC, Day ASC, Time ASC", conn);
                    }
                }

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Event e = new Event
                    {
                        Id = reader["Id"].ToString(),
                        Title = reader["Title"].ToString(),
                        Description = reader["Location"].ToString(),
                        Text = reader["Text"].ToString(),
                        Image = reader["Image"].ToString(),
                        City = reader["City"].ToString(),
                        Date = reader["Day"].ToString() + "." + reader["Month"].ToString(),
                        Time = reader["Time"].ToString(),
                        Entryfee = reader["Entryfee"].ToString(),
                        Sponsored = reader["Sponsored"].ToString()
                    };
                    events.Add(e);

                }

                reader.Close();
                conn.Close();
                return events;
            }
            catch (Exception)
            {
                Event e = new Event
                {
                    Id = "-501",
                    Title = "",
                    Description = "",
                    Text = "",
                    Image = "",
                    City = "",
                    Date = "",
                    Time = "",
                    Entryfee = "",
                    Sponsored = ""
                };
                conn.Close();
                events.Clear();
                events.Add(e);
                return events;
            }

        }

        /// <summary>
        /// Objekt steht für eine Location
        /// Locations bekommen ihrer Beschreibung die Straße
        /// und hinter Bild wird der Ordner der Location mit mehrer Bildern hinterlegt
        /// </summary>
        /// <returns></returns>
        public List<Location> GetLocations(string city)
        {
            int error = OpenDBConnection();
            List<Location> locations = new List<Location>();

            if (error != 200)
            {
                Location l = new Location
                {
                    Id = "-" + error.ToString(),
                    Name = "",
                    Street = "",
                    Text = "",
                    Image = "",
                    City = "",
                    Bar = false
                };
                locations.Add(l);
                return locations;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM location WHERE City = '" + city + "' ORDER BY Name", conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bool bar = false;
                    if (reader["Bar"].Equals("1")) { bar = true; }
                    Location l = new Location
                    {
                        Id = reader["Id"].ToString(),
                        Name = reader["Name"].ToString(),
                        Street = reader["Street"].ToString(),
                        Image = reader["Image"].ToString(),
                        City = reader["City"].ToString(),
                        Text = reader["Text"].ToString(),
                        Bar = bar
                    };
                    locations.Add(l);
                }
                reader.Close();
                conn.Close();
                return locations;
            }
            catch (Exception)
            {
                Location l = new Location
                {
                    Id = "-501",
                    Name = "",
                    Street = "",
                    Image = "",
                    City = "",
                    Text = "",
                    Bar = false
                };
                conn.Close();
                locations.Clear();
                locations.Add(l);
                return locations;
            }
        }

        public List<string> GetAllCitys()
        {
            List<string> Citys = new List<string>();

            int error = OpenDBConnection();
            if(error != 200)
            {
                Citys.Add("-" + error.ToString());
                return Citys;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT Name FROM city", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Citys.Add(reader["Name"].ToString());
                }

                reader.Close();
                conn.Close();
                return Citys;
            }
            catch (Exception)
            {
                Citys.Add("501");
                return Citys;
            }
            
        }

        public int DeleteEvent(string id, string table)
        {
            int error = OpenDBConnection();
            if(error != 200)
            {
                return error;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM " + table + " WHERE Id=" + id, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {
                return 501;
            }

            return 200;
        }

        public int DeleteLocation(string id)
        {
            int error = OpenDBConnection();
            if (error != 200)
            {
                return error;
            }
            try
            {
                MySqlCommand cmd = new MySqlCommand("DELETE FROM location WHERE Id=" + id, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                return 200;
            }
            catch (Exception)
            {
                return 501;
            }
        }

        public int[] DeleteExpiredEvents()
        {
            int error = OpenDBConnection();
            int[] ret = new int[2];
            if(error != 200)
            {
                ret[0] = (error);
                ret[1] = (0);
                return ret;
            }
            try
            {
                int thisMonth = DateTime.Today.Month;
                int thisDay = DateTime.Today.Day;
                int thisYear = DateTime.Today.Year;

                int affected = 0;

                MySqlCommand cmd = new MySqlCommand("DELETE FROM party WHERE Year < " + thisYear + " OR Year = " + thisYear + " AND Month < " + thisMonth + "  ORDER BY `Id` ASC", conn);
                affected = cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("DELETE FROM bar WHERE Year < " + thisYear + " OR Year = " + thisYear + " AND Month < " + thisMonth + "  ORDER BY `Id` ASC", conn);
                affected += cmd.ExecuteNonQuery();
                conn.Close();
                ret[0] = 200;
                ret[1] = affected;
                return ret;
            }
            catch (Exception)
            {
                ret[0] = 501;
                ret[1] = 0;
                return ret;
            }
        }

        private int OpenDBConnection()
        {
            try
            {
                if (conn.State.ToString().Equals("Closed"))
                {
                    conn.Open();
                }
                return 200;
            }
            catch (Exception)
            {
                return 500;
            }
        }
    }
}
