using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;

namespace StaminaRegen
{
    public class StaminaRegenMod : Mod
    {
                
        public override void Entry(params object[] objects)
        {
            ReadConfig();
            StardewModdingAPI.Events.GameEvents.UpdateTick += GameEvents_UpdateTick;
        }

        private float RegenTime = 3.0f;
        private int RegenAmount = 1;

        private double prevTime = 0;

        void GameEvents_UpdateTick(object sender, EventArgs e)
        {
            if (Game1.player == null || !Game1.hasLoadedGame)
                return;

            if (Game1.currentGameTime.TotalGameTime.TotalSeconds < prevTime + RegenTime)
                return;

            prevTime = Game1.currentGameTime.TotalGameTime.TotalSeconds;

            Game1.player.Stamina += RegenAmount;
        }

        private void ReadConfig()
        {
          var configLocation = Path.Combine(PathOnDisk, "StaminaRegenConfig.ini");
            if (File.Exists(configLocation))
            {
                var fileData = File.ReadAllLines(configLocation);
                if (fileData.Length > 1)
                {
                    //Load in TickRate
                    var regenTickRateString = fileData[0];
                    regenTickRateString = regenTickRateString.Replace("RegenTickRate:", "").Trim();
                    int newRate;
                    if (int.TryParse(regenTickRateString, out newRate))
                    {
                        RegenTime = Math.Max(1, newRate);
                    }

                    //Load in Amount
                    var regenAmountString = fileData[1];
                    regenAmountString = regenAmountString.Replace("RegenAmount:", "").Trim();
                    int newAmount;
                    if (int.TryParse(regenAmountString, out newAmount))
                    {
                        RegenAmount = newAmount;
                    }

                }
            }
            else
            {
                var dataToWrite = @"RegenTickRate: 3
RegenAmount: 1";

                File.WriteAllText(configLocation, dataToWrite);
            }
        }

    }
}
