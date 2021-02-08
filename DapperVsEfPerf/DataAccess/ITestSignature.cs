namespace DapperVsEfPerf.DataAccess
{
    public interface ITestSignature
    {
        long GetPlayerByID(int id);
        long GetRosterByTeamID(int teamID);
        long GetTeamRostersForSport(int sportID);
    }
}
