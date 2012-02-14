using System.Collections.Generic;
using Mogade.WindowsPhone;

namespace WindowsPhoneGame135.GameClasses
{
   public enum Leaderboards
   {
      Main = 1,
   }
   public class MogadeHelper
   {
      //Your game key and game secret
       private const string _gameKey = "4f30358d563d8a4acf00000d";
       private const string _secret = "PnW5:VdK=ENl]gpb?nS3q[]rIdyCL[]nluw";
      private static readonly IDictionary<Leaderboards, string> _leaderboardLookup = new Dictionary<Leaderboards, string>
                                                                                           {
                                                                                              {Leaderboards.Main, "4f3036d5563d8a4c25000010"}
                                                                                           };

      public static string LeaderboardId(Leaderboards leaderboard)
      {
         return _leaderboardLookup[leaderboard];
      }

      public static IMogadeClient CreateInstance()
      {
         //In your own game, when you are ready, REMOVE the ContectToTest to hit the production mogade server (instead of testing.mogade.com)
         //Also, if you are upgrading from the v1 library and you were using UserId (or not doing anything), you *must* change the UniqueIdStrategy to LegacyUserId
         MogadeConfiguration.Configuration(c => c.UsingUniqueIdStrategy(UniqueIdStrategy.UserId));
         return MogadeClient.Initialize(_gameKey, _secret);
      }
   }
}