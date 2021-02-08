using System.Diagnostics;
using System.Linq;
using DapperVsEfPerf.Models;
using DapperVsEfPerf.TestData;
using Microsoft.EntityFrameworkCore;

namespace DapperVsEfPerf.DataAccess
{
    public class EntityFrameworkCoreWithTracking : ITestSignature
    {
        public long GetPlayerByID(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (SportContextEfCore context = new SportContextEfCore(Database.GetOptions()))
            {
                var player = context.Players.First(x=>x.Id == id);
            }
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

        public long GetRosterByTeamID(int teamId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (SportContextEfCore context = new SportContextEfCore(Database.GetOptions()))
            {
                var teamRoster = context.Teams.Include(x => x.Players).Single(x => x.Id == teamId);
            }
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

        public long GetTeamRostersForSport(int sportId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (SportContextEfCore context = new SportContextEfCore(Database.GetOptions()))
            {
                var players = context.Teams.Include(x => x.Players).Where(x => x.SportId == sportId).ToList();
            }
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }
    }
}