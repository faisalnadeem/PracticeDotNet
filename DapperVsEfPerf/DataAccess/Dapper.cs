using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using DapperVsEfPerf.DTOs;

namespace DapperVsEfPerf.DataAccess
{
    public class Dapper : ITestSignature
    {
        public long GetPlayerByID(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (SqlConnection conn = new SqlConnection(Constants.SportsConnectionString))
            {
                conn.Open();
                var player = conn.QuerySingle<PlayerDTO>("SELECT Id, FirstName, LastName, DateOfBirth, TeamId FROM Player WHERE Id = @ID", new { ID = id });
            }
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

        public long GetRosterByTeamID(int teamId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (SqlConnection conn = new SqlConnection(Constants.SportsConnectionString))
            {
                conn.Open();
                var team = conn.QuerySingle<TeamDTO>("SELECT Id, Name, SportID, FoundingDate FROM Team WHERE ID = @id", new { id = teamId });

                team.Players = conn.Query<PlayerDTO>("SELECT Id, FirstName, LastName, DateOfBirth, TeamId FROM Player WHERE TeamId = @ID", new { ID = teamId }).ToList();
            }
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

        public long GetTeamRostersForSport(int sportId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (SqlConnection conn = new SqlConnection(Constants.SportsConnectionString))
            {
                conn.Open();
                var teams = conn.Query<TeamDTO>("SELECT ID, Name, SportID, FoundingDate FROM Team WHERE SportID = @ID", new { ID = sportId });

                var teamIDs = teams.Select(x => x.Id).ToList();

                var players = conn.Query<PlayerDTO>("SELECT ID, FirstName, LastName, DateOfBirth, TeamID FROM Player WHERE TeamID IN @IDs", new { IDs = teamIDs });

                foreach (var team in teams)
                {
                    team.Players = players.Where(x => x.TeamId == team.Id).ToList();
                }
            }
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }
    }
}