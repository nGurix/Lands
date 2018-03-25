using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Lands.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        const string isRemember = "IsRemember";
        static readonly string stringDefault = string.Empty;

        public static string IsRemember
        {
            get
            {
                return AppSettings.GetValueOrDefault(isRemember, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(isRemember, value);
            }
        }      
    }
}
