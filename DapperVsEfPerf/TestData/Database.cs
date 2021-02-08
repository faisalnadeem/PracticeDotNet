using System.Collections.Generic;
using DapperVsEfPerf.DTOs;
using DapperVsEfPerf.Models;
using Microsoft.EntityFrameworkCore;

namespace DapperVsEfPerf.TestData
{
    public static class Database
    {
        public static void Reset()
        {
            using SportContextEfCore context = new SportContextEfCore(GetOptions());
            context.Database.ExecuteSqlRaw("DELETE FROM Player");
            context.Database.ExecuteSqlRaw("DELETE FROM Team");
            context.Database.ExecuteSqlRaw("DELETE FROM Sport");
        }

        public static void Load(List<SportDTO> sports, List<TeamDTO> teams, List<PlayerDTO> players)
        {
            AddSports(sports);
            AddTeams(teams);
            AddPlayers(players);
        }

        private static void AddPlayers(List<PlayerDTO> players)
        {
            using var context = new SportContextEfCore(GetOptions());
            foreach (var player in players)
            {
                context.Players.Add(new Player()
                {
                    FirstName = player.FirstName,
                    LastName = player.LastName,
                    DateOfBirth = player.DateOfBirth,
                    TeamId = player.TeamId,
                    Id = player.Id
                });
            }

            context.SaveChanges();
        }

        private static void AddTeams(List<TeamDTO> teams)
        {
            using var context = new SportContextEfCore(GetOptions());
            foreach (var team in teams)
            {
                context.Teams.Add(new Team()
                {
                    Name = team.Name,
                    Id = team.Id,
                    SportId = team.SportId,
                    FoundingDate = team.FoundingDate
                });
            }

            context.SaveChanges();
        }

        private static void AddSports(List<SportDTO> sports)
        {
            using var context = new SportContextEfCore(GetOptions());
            foreach (var sport in sports)
            {
                context.Sports.Add(new Sport()
                {
                    Id = sport.Id,
                    Name = sport.Name
                });
            }

            context.SaveChanges();
        }

        public static DbContextOptions GetOptions()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(Constants.SportsConnectionString);

            return builder.Options;
        }
    }
}
