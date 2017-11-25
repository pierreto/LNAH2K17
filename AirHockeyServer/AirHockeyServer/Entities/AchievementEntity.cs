using AirHockeyServer.Core;
using AirHockeyServer.Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Entities
{
    public class AchievementEntity
    {
        public AchivementType AchivementType { get; set; }

        public string Name { get; set; }

        public string DisabledImageUrl { get; set; }

        public string EnabledImageUrl { get; set; }

        public bool IsEnabled { get; set; }

        public string Category { get; set; }

        public int Order { get; set; }

        public AchievementEntity(AchievementPoco poco)
        {
            AchivementType type = (AchivementType) Enum.Parse(AchivementType.GetType(), poco.AchievementType);
            var entity = Cache.Achievements[AchivementType];

            Name = entity.Name;
            DisabledImageUrl = entity.DisabledImageUrl;
            EnabledImageUrl = entity.EnabledImageUrl;
            IsEnabled = poco.IsEnabled;
            Category = entity.Category;
            Order = entity.Order;
        }

        public AchievementEntity()
        {

        }

    }

    public enum AchivementType
    {
        FirstGamePlayed,
        FirstTournamentPlayed,

        FiveGamesPlayed,
        FiveTournamentsPlayed,

        TenGamesPlayed,
        TenTournamentsPlayed,

        FivePoints,
        ThirtyPoints,
        EightyPoints,

        FirstGameWon,
        FirstTournamentWon,

        FiveGameWon,
        FiveTournamentWon,
        
        TenGameWon,
        TenTournamentWon
    }
}