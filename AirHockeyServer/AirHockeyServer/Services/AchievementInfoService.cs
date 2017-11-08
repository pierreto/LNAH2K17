using AirHockeyServer.Entities;
using AirHockeyServer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services
{
    public class AchievementInfoService : IAchievementInfoService
    {
        public Dictionary<AchivementType, string> EnabledImage { get; set; }

        public Dictionary<AchivementType, string> DisabledImage { get; set; }

        public Dictionary<AchivementType, string> Name { get; set; }

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

        public AchievementInfoService()
        {
            EnabledImage = new Dictionary<AchivementType, string>();
            DisabledImage = new Dictionary<AchivementType, string>();
            Name = new Dictionary<AchivementType, string>();

            EnabledImage.Add(AchivementType.FirstGamePlayed, "");
            EnabledImage.Add(AchivementType.FirstTournamentPlayed, "");
            EnabledImage.Add(AchivementType.FivePoints, "");

            DisabledImage.Add(AchivementType.FirstGamePlayed, "");
            DisabledImage.Add(AchivementType.FirstTournamentPlayed, "");
            DisabledImage.Add(AchivementType.FivePoints, "");

            Name.Add(AchivementType.FirstGamePlayed, "Premier match");
            Name.Add(AchivementType.FirstTournamentPlayed, "Premier tournoi");
            Name.Add(AchivementType.FivePoints, "5 points accumulés");
        }
    }
}