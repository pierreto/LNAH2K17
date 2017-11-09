using AirHockeyServer.Entities;
using AirHockeyServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services
{
    public class AchievementInfoService : IAchievementInfoService
    {
        public Dictionary<AchivementType, string> EnabledImage { get; set; }

        public Dictionary<AchivementType, string> DisabledImage { get; set; }

        public Dictionary<AchivementType, string> Name { get; set; }

        public Dictionary<AchivementType, string> Categories { get; set; }

        public Dictionary<AchivementType, int> Order { get; set; }

        public string GetEnabledImage(AchivementType achivementType)
        {
            if(EnabledImage.ContainsKey(achivementType))
            {
                return EnabledImage[achivementType];
            }

            return string.Empty;
        }

        public string GetDisabledImage(AchivementType achivementType)
        {
            if (DisabledImage.ContainsKey(achivementType))
            {
                return DisabledImage[achivementType];
            }

            return string.Empty;
        }

        public string GetName(AchivementType achivementType)
        {
            if (Name.ContainsKey(achivementType))
            {
                return Name[achivementType];
            }

            return string.Empty;
        }

        public string GetCategory(AchivementType achivementType)
        {
            if (Categories.ContainsKey(achivementType))
            {
                return Categories[achivementType];
            }

            return string.Empty;
        }

        public int GetOrder(AchivementType achivementType)
        {
            if (Order.ContainsKey(achivementType))
            {
                return Order[achivementType];
            }

            return 0;
        }

        public AchievementInfoService()
        {
            EnabledImage = new Dictionary<AchivementType, string>();
            DisabledImage = new Dictionary<AchivementType, string>();
            Name = new Dictionary<AchivementType, string>();
            Categories = new Dictionary<AchivementType, string>();
            Order = new Dictionary<AchivementType, int>();

            EnabledImage.Add(AchivementType.FirstGamePlayed, "\\media\\image\\game_played_1_enabled.png");
            EnabledImage.Add(AchivementType.FiveGamesPlayed, "\\media\\image\\game_played_5_enabled.png");
            EnabledImage.Add(AchivementType.TenGamesPlayed, "\\media\\image\\game_played_10_enabled.png");

            EnabledImage.Add(AchivementType.FirstTournamentPlayed, "\\media\\image\\tournament_played_1_enabled.png");
            EnabledImage.Add(AchivementType.FiveTournamentsPlayed, "\\media\\image\\tournament_played_5_enabled.png");
            EnabledImage.Add(AchivementType.TenTournamentPlayed, "\\media\\image\\tournament_played_10_enabled.png");
            
            EnabledImage.Add(AchivementType.FivePoints, "\\media\\image\\coin_5_enabled.png");
            EnabledImage.Add(AchivementType.ThirtyPoints, "\\media\\image\\coin_30_enabled.png");
            EnabledImage.Add(AchivementType.EightyPoints, "\\media\\image\\coin_80_enabled.png");

            EnabledImage.Add(AchivementType.FiveTournamentWon, "\\media\\image\\tournament_won_5_enabled.png");
            EnabledImage.Add(AchivementType.TenTournamentWon, "\\media\\image\\tournament_won_10_enabled.png");

            EnabledImage.Add(AchivementType.FiveGameWon, "\\media\\image\\game_won_5_enabled.png");
            EnabledImage.Add(AchivementType.TenGameWon, "\\media\\image\\game_won_10_enabled.png");

            // ***************************************************************************************** //

            DisabledImage.Add(AchivementType.FirstGamePlayed, "\\media\\image\\game_played_1_disabled.png");
            DisabledImage.Add(AchivementType.FiveGamesPlayed, "\\media\\image\\game_played_5_disabled.png");
            DisabledImage.Add(AchivementType.TenGamesPlayed, "\\media\\image\\game_played_10_disabled.png");

            DisabledImage.Add(AchivementType.FirstTournamentPlayed, "\\media\\image\\tournament_played_1_disabled.png");
            DisabledImage.Add(AchivementType.FiveTournamentsPlayed, "\\media\\image\\tournament_played_5_disabled.png");
            DisabledImage.Add(AchivementType.TenTournamentPlayed, "\\media\\image\\tournament_played_10_disabled.png");

            DisabledImage.Add(AchivementType.FivePoints, "\\media\\image\\coin_5_disabled.png");
            DisabledImage.Add(AchivementType.ThirtyPoints, "\\media\\image\\coin_30_disabled.png");
            DisabledImage.Add(AchivementType.EightyPoints, "\\media\\image\\coin_80_disabled.png");

            DisabledImage.Add(AchivementType.FiveTournamentWon, "\\media\\image\\tournament_won_5_disabled.png");
            DisabledImage.Add(AchivementType.TenTournamentWon, "\\media\\image\\tournament_won_10_disabled.png");

            DisabledImage.Add(AchivementType.FiveGameWon, "\\media\\image\\game_won_5_disabled.png");
            DisabledImage.Add(AchivementType.TenGameWon, "\\media\\image\\game_won_10_disabled.png");

            // ******************************************************************************************* //

            Name.Add(AchivementType.FirstGamePlayed, "Premier match");
            Name.Add(AchivementType.FiveGamesPlayed, "5 matchs joués");
            Name.Add(AchivementType.TenGamesPlayed, "10 matchs joués");

            Name.Add(AchivementType.FirstTournamentPlayed, "Premier tournois joué");
            Name.Add(AchivementType.FiveTournamentsPlayed, "5 tournois joué");
            Name.Add(AchivementType.TenTournamentPlayed, "10 tournois joué");

            Name.Add(AchivementType.FivePoints, "5 points accumulés");
            Name.Add(AchivementType.ThirtyPoints, "30 points accumulés");
            Name.Add(AchivementType.EightyPoints, "80 points accumulés");

            Name.Add(AchivementType.FiveTournamentWon, "5 tournois gagnés");
            Name.Add(AchivementType.TenTournamentWon, "10 tournois gagnés");

            Name.Add(AchivementType.FiveGameWon, "5 matchs gagnés");
            Name.Add(AchivementType.TenGameWon, "10 matchs gagnés");

            // ***************************************************************************************** //

            Categories.Add(AchivementType.FirstGamePlayed, "GamePlayed");
            Categories.Add(AchivementType.FiveGamesPlayed, "GamePlayed");
            Categories.Add(AchivementType.TenGamesPlayed, "GamePlayed");

            Categories.Add(AchivementType.FirstTournamentPlayed, "TournamentPlayed");
            Categories.Add(AchivementType.FiveTournamentsPlayed, "TournamentPlayed");
            Categories.Add(AchivementType.TenTournamentPlayed, "TournamentPlayed");

            Categories.Add(AchivementType.FivePoints, "Points");
            Categories.Add(AchivementType.ThirtyPoints, "Points");
            Categories.Add(AchivementType.EightyPoints, "Points");

            Categories.Add(AchivementType.FiveTournamentWon, "TournamentWon");
            Categories.Add(AchivementType.TenTournamentWon, "TournamentWon");

            Categories.Add(AchivementType.FiveGameWon, "GameWon");
            Categories.Add(AchivementType.TenGameWon, "GameWon");

            // ********************************************************************************************* //

            Order.Add(AchivementType.FirstGamePlayed, 1);
            Order.Add(AchivementType.FiveGamesPlayed, 2);
            Order.Add(AchivementType.TenGamesPlayed, 3);

            Order.Add(AchivementType.FirstTournamentPlayed, 1);
            Order.Add(AchivementType.FiveTournamentsPlayed, 2);
            Order.Add(AchivementType.TenTournamentPlayed, 3);

            Order.Add(AchivementType.FivePoints, 1);
            Order.Add(AchivementType.ThirtyPoints, 2);
            Order.Add(AchivementType.EightyPoints, 3);

            Order.Add(AchivementType.FiveTournamentWon, 1);
            Order.Add(AchivementType.TenTournamentWon, 2);

            Order.Add(AchivementType.FiveGameWon, 1);
            Order.Add(AchivementType.TenGameWon, 2);
        }
    }
}