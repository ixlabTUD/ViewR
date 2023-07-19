using System;

namespace ViewR.HelpersLib.Extensions.JSON
{
    /// <summary>
    /// Allows us to serialize <see cref="DateTime"/>.
    /// </summary>
    /// <remarks>
    /// Based on
    ///     https://gamedev.stackexchange.com/a/137526
    ///
    /// Usage:
    ///     var time = DateTime.Now;
    ///     print(time);
    ///     var json = JsonUtility.ToJson((JsonDateTime) time);
    ///     print(json);
    ///     DateTime timeFromJson = JsonUtility.FromJson<JsonDateTime>(json);
    ///     print(timeFromJson);
    /// </remarks>
    [Serializable]
    public struct JsonDateTime
    {
        public long value;

        public static implicit operator DateTime(JsonDateTime jdt)
        {
            // Debug.Log("Converted to time");
            return DateTime.FromFileTimeUtc(jdt.value);
        }

        public static implicit operator JsonDateTime(DateTime dt)
        {
            // Debug.Log("Converted to JDT");
            var jdt = new JsonDateTime
            {
                value = dt.ToFileTimeUtc()
            };
            return jdt;
        }
    }
}