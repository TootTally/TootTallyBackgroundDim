using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;

namespace TootTallyBackgroundDim
{
    public static class OptionalGameplayUIReducer
    {
        public static bool ShouldHideBreathMeter()
        {
            try
            {
                Type plugin = Type.GetType("GameplayUIReducer.Plugin, GameplayUIReducer");
                if (plugin == null) return false;
                var info = plugin.GetField("ConfigEntries", BindingFlags.NonPublic | BindingFlags.Static);
                var configEntries = (Dictionary<string, ConfigEntry<bool>>)info.GetValue(null);
                ConfigEntry<bool> breathMeter;
                if (!configEntries.TryGetValue("Breath Meter", out breathMeter)) return false;
                return breathMeter.Value;
            }
            catch (Exception e)
            {
                Plugin.LogError("Exception trying to get GameplayUIReducer settings");
                Plugin.LogError(e.Message);
                Plugin.LogError(e.StackTrace);
                return false;
            }
        }
    }
}
