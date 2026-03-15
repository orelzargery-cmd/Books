<<<<<<< HEAD
﻿using System;
using Newtonsoft.Json;

namespace targil
{
    public class Book
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("publish_date")]
        public DateTime PublishDate { get; set; }
    }
}
=======
﻿using System;
using Newtonsoft.Json;

namespace targil
{
    public class Book
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("publish_date")]
        public DateTime PublishDate { get; set; }
    }
}
>>>>>>> bf9ba983594dbb25861411437f49852f733a64c3
