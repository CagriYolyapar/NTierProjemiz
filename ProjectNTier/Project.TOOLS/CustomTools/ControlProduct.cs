using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.TOOLS.CustomTools
{
    public static class ControlProduct
    {
        public static Cart KeepProduct(object sessionData)
        {
            Product bekleyenUrun = sessionData as Product;
            CartItem ci = new CartItem();
            ci.ID = bekleyenUrun.ID;
            ci.Name = bekleyenUrun.ProductName;
            ci.ImagePath = bekleyenUrun.ImagePath;
            ci.Price = Convert.ToDecimal(bekleyenUrun.UnitPrice);
            Cart c = new Cart();
            c.SepeteEkle(ci);

            return c;
        }
    }
}
