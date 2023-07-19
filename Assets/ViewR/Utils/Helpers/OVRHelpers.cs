namespace ViewR.Utils.Helpers
{
    public class OVRHelpers
    {
        public static string FetchQuestModelID()
        {
            var headset = OVRPlugin.GetSystemHeadsetType();

            return ((int) headset).ToString();
        }
    }
}