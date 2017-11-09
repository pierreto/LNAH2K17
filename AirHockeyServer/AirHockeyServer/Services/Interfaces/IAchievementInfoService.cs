using AirHockeyServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHockeyServer.Services.Interfaces
{
    public interface IAchievementInfoService
    {
        string GetEnabledImage(AchivementType achivementType);

        string GetDisabledImage(AchivementType achivementType);

        string GetName(AchivementType achivementType);

        string GetCategory(AchivementType achivementType);

        int GetOrder(AchivementType achivementType);

    }
}