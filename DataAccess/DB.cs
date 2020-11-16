using Logic;
using Objects.input;
using Objects.output;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace DataAccess
{
    public class DB
    {
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;

        public void Open() 
        {
             connection = new SqlConnection("Data Source=LAPTOP-HTNHL6CO\\SQLEXPRESS; Initial Catalog=Betting; Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
             connection.Open();
        }

        public void Close() 
        {
            connection.Close();
        }

        public RouletteResponse1 GetRoulettes() 
        {
            List<Roulette> rows = new List<Roulette>();
            RouletteResponse1 response = new RouletteResponse1();
            try
            {
                Open();
                command = new SqlCommand("SELECT * FROM dbo.getRouletteStatus", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rows.Add(new Roulette(reader.GetFieldValue<int>(0), reader.GetFieldValue<DateTime>(1), reader.GetFieldValue<DateTime>(2), reader.GetFieldValue<string>(3), reader.GetFieldValue<int>(4), reader.GetFieldValue<int>(5)));
                }
                Close();
                response.message = new Message("0000", "Transaccion exitosa", null);
            }
            catch (Exception ex)
            {
                response.message = new Message( "0002" , "Ocurrio un error", ex );
            }
            response.rouletteList = rows;

            return response;
        }

        public RouletteResponse2 CreateRoulette() 
        {
            RouletteResponse2 response = new RouletteResponse2();
            response.rouletteId = 0;
            int answer = 0;
            try
            {
                Open();
                command = new SqlCommand("EXEC dbo.createNewRoulette", connection);
                reader = command.ExecuteReader();
                while (reader.Read()) { answer = reader.GetFieldValue<int>(0); }
                Close();
                if (answer != 0)
                {
                    response.message = new Message("0000", "Transaccion exitosa, ruleta creada", null);
                    response.rouletteId = answer;
                }
                else 
                {
                    response.message = new Message("0006", "Transaccion fallida.", null);
                }
            }
            catch (Exception ex) 
            {
                response.message = new Message("0005", "Transaccion fallida.", ex);
            }

            return response;
        }

        public Message OpenRoulette(int rouletteId)
        {
            Message message = new Message();
            int answer = 0;
            try
            {
                Open();
                command = new SqlCommand("dbo.openRoulette", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@id", rouletteId);
                reader = command.ExecuteReader();
                while (reader.Read()) { answer = reader.GetFieldValue<int>(0); }
                Close();
                if (answer != 0)
                {
                    message = new Message("0000", "Transaccion exitosa, se realizó apertura de la ruleta #" + rouletteId, null);
                }
                else
                {
                    message = new Message("0009", "Transaccion fallida.", null);
                }
            }
            catch (Exception ex)
            {
                message = new Message("0008", "Transaccion fallida.", ex);
            }

            return message;
        }

        public Message CreateClient(NewClientRequest newClient) 
        {
            Message message = new Message();
            int answer = 0;
            try
            {
                Open();
                command = new SqlCommand("dbo.spCreateNewUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@username", newClient.username);
                command.Parameters.AddWithValue("@password", newClient.password);
                command.Parameters.AddWithValue("@name", newClient.name);
                reader = command.ExecuteReader();
                while (reader.Read()) { answer = reader.GetFieldValue<int>(0); }
                Close();
                if (answer != 0)
                {
                    message = new Message("0000", "Transaccion exitosa, cliente registrado", null);
                }
                else
                {
                    message = new Message("0010", "Transaccion fallida.", null);
                }
            }
            catch (Exception ex)
            {
                message = new Message("0011", "Transaccion fallida.", ex);
            }

            return message;
        }

        public SessionTokenResponse GetSessionToken(ClientSessionRequest sessionRequest)
        {
            SessionTokenResponse response = new SessionTokenResponse();
            var answer = "0";
            try
            {
                Open();
                command = new SqlCommand("dbo.getUserSessionId", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@username", sessionRequest.username);
                command.Parameters.AddWithValue("@password", sessionRequest.password);
                reader = command.ExecuteReader();
                while (reader.Read()) 
                { 
                    answer = reader.GetFieldValue<string>(0); 
                }
                Close();
                if (answer != "0")
                {
                   response.token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(answer.ToString()));
                   response.message = new Message("0000", "Transaccion exitosa", null);
                }
                else
                {
                   response.message = new Message("0016", "Transaccion fallida", null);
                }
            }
            catch (Exception ex)
            {
                response.message = new Message("0015", "Transaccion fallida.", ex);
            }

            return response;
        }

        public Message CreateBet(BetRequest request )
        {
            Message message = new Message();
            int answer = 0;
            string remark = "";
            int bet = 0;
            try
            {
                string tokenDecoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(request.token));
                Open();
                command = new SqlCommand("dbo.betTransacction", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tokenDecoded", tokenDecoded);
                command.Parameters.AddWithValue("@betNumber", request.number);
                command.Parameters.AddWithValue("@betColor", request.color.ToUpper());
                command.Parameters.AddWithValue("@mount", request.amount);
                command.Parameters.AddWithValue("@rouletteId", request.rouletteId);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    answer = reader.GetFieldValue<int>(0);
                    remark = reader.GetFieldValue<string>(1);
                    bet = reader.GetFieldValue<int>(2);
                }
                if (answer != 0)
                {
                    message = new Message("0000", "Transaccion exitosa, " + remark + ", su id de apuesta es: " + bet, null);
                }
                else
                {               
                    message = new Message("0023", "Transaccion fallida. " + remark, null);
                }
                Close();
            }
            catch (Exception ex)
            {
                message = new Message("0022", "Transaccion fallida.", ex);
            }

            return message;
        }

        public CloseRouletteResponse CloseRoulette(int rouletteId) 
        {
            CloseRouletteResponse response = new CloseRouletteResponse();
            Process process = new Process();
            List<BetResult> resultList = new List<BetResult>();
            string value = ""; 
            try
            {
                response.gameResult = process.ExecuteRoulette();
                Open();
                command = new SqlCommand("dbo.closeRoulette", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@rouletteId", rouletteId);
                command.Parameters.AddWithValue("@number", response.gameResult.number);
                command.Parameters.AddWithValue("@color", response.gameResult.color);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    resultList.Add(new BetResult(reader.GetFieldValue<string>(0), reader.GetFieldValue<string>(1), reader.GetFieldValue<double>(2), reader.GetFieldValue<int>(3), reader.GetFieldValue<string>(4), reader.GetFieldValue<double>(5), reader.GetFieldValue<int>(6), reader.GetFieldValue<string>(7)));
                    response.resultList = resultList;
                    value = reader.GetFieldValue<string>(7);
                }
                Close();
                if (value == "0")
                {
                    response.message = new Message("0026", "Transaccion fallida, la ruleta indicada no es valida.", null);
                    response.gameResult = null;
                }
                else 
                {
                    response.message = new Message("0000", "Transaccion exitosa.", null);
                }
            }
            catch (Exception ex) 
            {
                response.message = new Message("0025", "Ocurrio un error. ", ex);
            }

            return response;
        }

        public void CloseSession()
        {
            Open();
            command = new SqlCommand("EXEC dbo.destroyUserSessions", connection);
            reader = command.ExecuteReader();
            Close();
        }

        public Message GetCreditCoins(GetCreditCoinsRequest request) 
        {
            Message message = new Message();
            try
            {
                Open();
                string answer = "0";
                command = new SqlCommand("dbo.reloadClientCredits", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@amount", request.amount);
                command.Parameters.AddWithValue("@clientId", request.clientId);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    answer = reader.GetFieldValue<string>(0);
                }
                Close();
                if (answer == "1")
                {
                    message = new Message("0000", "Transaccion exitosa.", null);
                }
                else
                {
                    message = new Message("0032", "Transaccion fallida.", null);
                }
            }
            catch (Exception ex) 
            {
                message = new Message("0031", "Ocurrio un error. ", ex);
            }
            return message;
        }
    }
}
