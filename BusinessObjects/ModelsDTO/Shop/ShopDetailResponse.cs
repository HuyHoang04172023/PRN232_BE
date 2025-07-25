using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ModelsDTO.Shop
{
    public class ShopDetailResponse
    {
        public int ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? ShopAddress { get; set; }
        public string? ShopImage { get; set; }
        public string? ShopDescription { get; set; }
        public string? StatusShopName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
