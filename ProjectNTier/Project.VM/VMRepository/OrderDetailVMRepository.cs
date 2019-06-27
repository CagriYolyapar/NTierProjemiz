using Project.BLL.RepositoryPattern.BaseRep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.VM.VMs;

namespace Project.VM.VMRepository
{
    public class OrderDetailVMRepository:BaseRepository<OrderDetailVM>
    {
        public override List<OrderDetailVM> SelectActives()
        {
            return db.OrderDetails.Select(x => new OrderDetailVM
            {
                OrderID = x.OrderID,
                ProductID = x.ProductID
            }).ToList();
        }
    }
}
