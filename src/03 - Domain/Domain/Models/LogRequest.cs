﻿using Domain.Converters;
using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class LogRequest
    {
        public int Id { get; set; }

        [JsonConverter(typeof(DateFormatConverter))]
        public DateTime Date { get; set; }

        public string Method { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
    }
}