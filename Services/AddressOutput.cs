
namespace GeoPetWebApi.Services.output {
    public class AddressOutput {
        public long place_id { get; set; }
        public string licence { get; set; }
        public string osm_type { get; set; }
        public long osm_id { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string display_name { get; set; }
        public IAddress address {get; set;}
        public string error {get; set;}
    };
}
