using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lands.Models
{
    public class Currency
    {
        [JsonProperty(PropertyName = "code")] //se le agrega para seguir las buenas practicas de c# donde las propiedades comienzan con Mayusculas
        public string Code { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }
    }
}
