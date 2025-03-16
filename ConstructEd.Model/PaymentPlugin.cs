using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Models
{
    [PrimaryKey(nameof(PaymentId), nameof(PluginId))]
    public class PaymentPlugin
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public int PluginId { get; set; }
        public Plugin Plugin { get; set; }

    }
}