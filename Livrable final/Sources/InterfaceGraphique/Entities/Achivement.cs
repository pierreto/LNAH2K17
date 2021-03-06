﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceGraphique.Entities
{
    public class Achievement
    {
        public AchivementType AchivementType { get; set; }

        public string Category { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string DisabledImageUrl { get; set; }

        public string EnabledImageUrl { get; set; }

        public string ImageUrl
        {
            get
            {
                return IsEnabled ? Directory.GetCurrentDirectory() + EnabledImageUrl : Directory.GetCurrentDirectory() + DisabledImageUrl;
            }
        }

        public bool IsEnabled { get; set; }

        public Achievement()
        {

        }

        public Achievement(string disabledImageUrl, string enabledImageUrl)
        {
            DisabledImageUrl = disabledImageUrl;
            EnabledImageUrl = enabledImageUrl;
        }
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
