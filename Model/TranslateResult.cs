using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackParse.Model
{
    public partial class TranslateResult
    {
        [JsonProperty("string")]
        public String String { get; set; }
    }

    public partial class String
    {
        [JsonProperty("-xmlns")]
        public Uri Xmlns { get; set; }

        [JsonProperty("#text")]
        public string Text { get; set; }
    }
}
