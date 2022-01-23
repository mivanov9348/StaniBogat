using Newtonsoft.Json;

namespace StaniBogat.Data
{
    [JsonObject]
    public class Quest
    {

        public int Id { get; set; }

        [JsonProperty("Question")]
        public string Question  { get; set; }

        [JsonProperty("A")]
        public string A  { get; set; }

        [JsonProperty("B")]
        public string B { get; set; }

        [JsonProperty("C")]
        public string C { get; set; }

        [JsonProperty("D")]
        public string D { get; set; }

        [JsonProperty("CorrectAnswer")]
        public string CorrectAnswer { get; set; }

        public Scale Scale { get; set; }
        [JsonProperty("ScaleId")]
        public int ScaleId { get; set; }

    }
}
