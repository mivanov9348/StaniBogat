using Newtonsoft.Json;

namespace StaniBogat.Data
{
    public class QuestDeserialize
    {
        [JsonProperty("Quests")]
        public List<Quest> Quests { get; set; }
    }
}
