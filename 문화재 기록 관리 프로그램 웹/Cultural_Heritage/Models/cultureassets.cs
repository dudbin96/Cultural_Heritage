using System.ComponentModel.DataAnnotations;

namespace Cultural_Heritage.Models
{
    public class cultureassets
    {
        public int num { get; set; }
        public string ca_type { get; set; }
        public string ca_name { get; set; }
        public string ca_addr { get; set; }
        public string ca_period { get; set; }
        public DateTime ca_date { get; set; }
        public string ca_detail { get; set; }
    }
}
