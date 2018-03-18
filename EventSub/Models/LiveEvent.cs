using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventSub.Models
{
    public class LiveEvent
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}