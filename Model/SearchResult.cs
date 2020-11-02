using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackParse.Model
{
    public class ArrayResult
    {
        [JsonProperty("results")]
        public SearchResult[] Results { get; set; }
    }
    public class SearchResult
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("link")]
        public Uri Link { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
