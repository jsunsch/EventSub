using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventSub.Models
{
    /// <summary>
    /// A custom Json Converter has been created <see cref="EventSub.Converters.LiveEventConverter"/> which
    /// populates this object with a dynamic dataset, while still supporting the ApiControllers
    /// ModelState validation techniques.
    /// </summary>
    public class LiveEvent
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}