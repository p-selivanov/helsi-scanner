using System;
using System.Text.Json.Serialization;

namespace HelsiScanner.Dtos
{
    internal class Slot
    {
        [JsonPropertyName("sd")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("ed")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("rs")]
        public SlotStatus Status { get; set; }
    }
}
