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

    }

    public enum AchivementType
    {
        FirstGamePlayed,
        FirstTournamentPlayed,

        FiveGamesPlayed,
        FiveTournamentsPlayed,

        TenGamesPlayed,
        TenTournamentPlayed,

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