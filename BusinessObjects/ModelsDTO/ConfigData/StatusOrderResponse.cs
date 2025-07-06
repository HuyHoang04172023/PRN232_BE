using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ModelsDTO.ConfigData
{
    public class StatusOrderResponse
    {
        public int StatusOrderId { get; set; }

        public string StatusOrderName { get; set; } = null!;
    }
}
