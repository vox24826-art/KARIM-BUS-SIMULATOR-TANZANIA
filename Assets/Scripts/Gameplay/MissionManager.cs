using System.Collections.Generic;
using UnityEngine;

namespace KarimBusSimulator
{
    [System.Serializable]
    public class MissionDefinition
    {
        public string missionId;
        public string title;
        public string description;
        public int rewardMoney;
        public bool completed;
    }

    public class MissionManager : MonoBehaviour
    {
        public List<MissionDefinition> missions = new List<MissionDefinition>();

        private void Awake()
        {
            BuildDefaultMissions();
        }

        public void BuildDefaultMissions()
        {
            if (missions != null && missions.Count > 0)
            {
                return;
            }

            missions = new List<MissionDefinition>
            {
                new MissionDefinition { missionId = "first_route", title = "Complete First Route", description = "Drive the Dar es Salaam to Arusha route safely.", rewardMoney = 1500, completed = false },
                new MissionDefinition { missionId = "rain_drive", title = "Drive in Rain", description = "Complete a wet-weather journey with smooth braking and safe speed.", rewardMoney = 2000, completed = false },
                new MissionDefinition { missionId = "night_route", title = "Night Route", description = "Complete a night driving challenge with working lights and safe driving.", rewardMoney = 2500, completed = false },
                new MissionDefinition { missionId = "checkpoint_pass", title = "Police Checkpoint", description = "Pass a checkpoint without penalties and keep a clean record.", rewardMoney = 1800, completed = false },
                new MissionDefinition { missionId = "buy_second_bus", title = "Second Bus", description = "Earn enough money to purchase a second bus for the fleet.", rewardMoney = 3500, completed = false }
            };
        }

        public MissionDefinition GetMissionById(string missionId)
        {
            foreach (var mission in missions)
            {
                if (mission.missionId == missionId)
                {
                    return mission;
                }
            }

            return missions.Count > 0 ? missions[0] : null;
        }

        public void CompleteMission(string missionId)
        {
            var mission = GetMissionById(missionId);
            if (mission == null)
            {
                return;
            }

            mission.completed = true;
        }
    }
}
