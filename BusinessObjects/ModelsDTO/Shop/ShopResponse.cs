using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace BusinessObjects.ModelsDTO.Shop
{
    public partial class ShopResponse
    {
        public int ShopId { get; set; }

        public string? ShopName { get; set; }

        public string? ShopAddress { get; set; }

        public string? ShopImage { get; set; }

        public string? ShopDescription { get; set; }

        public int? StatusShopId { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? ApprovedBy { get; set; }

        public string? StatusShop { get; set; }
    }
}
