using PagedList;
using Project.BLL.RepositoryPattern.ConcRep;
using Project.MODEL.Entities;
using Project.TOOLS.CustomTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class MemberController : Controller
    {
        ProductRepository prep;
        CategoryRepository crep;
        OrderRepository orep;
        OrderDetailRepository odrep;

        public MemberController()
        {
            prep = new ProductRepository();
            crep = new CategoryRepository();
            orep = new OrderRepository();
            odrep = new OrderDetailRepository();
        }

        // GET: Member
        public ActionResult Index(int? page)
        {
            return View(Tuple.Create(prep.SelectActives().ToPagedList(page??1,9),crep.SelectActives()));
        }

        public ActionResult SelectByCategory(int id,int? page)
        {
            ViewBag.KategoriID = id;
            return View(Tuple.Create(prep.Where(x=>x.CategoryID==id).ToPagedList(page??1,9),crep.SelectActives()));
        }

        public ActionResult AddToCart(int id)
        {
            if (Session["member"]==null)
            {
                TempData["uyeDegil"] = "Lutfen sepete ürün eklemeden önce üye olun";
                Product bekleyenUrun = prep.GetByID(id);
                Session["bekleyenUrun"] = bekleyenUrun;
                return RedirectToAction("Register","Home");
            }

            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;

            Product eklenecekUrun = prep.GetByID(id);

            CartItem ci = new CartItem();
            ci.ID = eklenecekUrun.ID;
            ci.Name = eklenecekUrun.ProductName;
            ci.Price =Convert.ToDecimal( eklenecekUrun.UnitPrice);
            ci.ImagePath = eklenecekUrun.ImagePath;
            c.SepeteEkle(ci);

            Session["scart"] = c;

            return RedirectToAction("Index");
        }

        public ActionResult CartPage()
        {
            if (Session["scart"]!=null)
            {
                Cart c = Session["scart"] as Cart;
                return View(c);
            }

            TempData["message"] = "Sepetinizde ürün bulunmamaktadır";

            return RedirectToAction("Index");
        }

        public ActionResult SiparisiOnayla()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SiparisiOnayla(Order item)
        {
            if (item.RequiredDate<DateTime.Now)
            {
                ViewBag.Tarih = "Gecmiş bir tarih secemezsiniz";
                return View();
            }
            else if ((item.RequiredDate-DateTime.Now).Value.Days<=2)
            {
                ViewBag.Tarih = "2 gün veya daha az sürede bir gönderim maalesef yapamayız";
                return View();
            }
            item.AppUserID = (Session["member"] as AppUser).ID;

            orep.Add(item); //item'in ID'si olustu...

            Cart sepet = Session["scart"] as Cart;

            string gonderilecekMesaj = null;
            foreach (CartItem urun in sepet.Sepetim)
            {
                OrderDetail od = new OrderDetail();
                od.OrderID = item.ID;
                od.ProductID = urun.ID;
                od.TotalPrice = urun.SubTotal;
                od.Quantity = urun.Amount;
                
                odrep.Add(od);

                //Bu kesimde ürünün stok sayısının da düsürülmesi gerekir. Hatta customexception yaratılıp ürün stok sayısıruntimeda düserse try catch ile burada ilgili maili gönderip sadece stoktaki kadar aldırmak uygundur(ödev)
                /*
                 
                 prep.GetByID(urun.ID); => stogundan düsmemiz gereken ürünü verir..Cünkü urun.ID'si ile ProductID aynıdır.
                 
                 
                 */
                


                gonderilecekMesaj += $"Urun:{urun.Name}, Adedi:{urun.Amount}, Toplam Fiyatı:{urun.SubTotal}\n";
                
            }
            gonderilecekMesaj += $"Odemeniz gereken toplam tutar{sepet.TotalPrice}";

            Session.Remove("scart");

            TempData["satis"] = "Siparişiniz bize ulasmıstır. Detaylı liste mail adresinize gönderildi...";

            MailSender.Send((Session["member"] as AppUser).Email, password: "Cf8885++--", body: gonderilecekMesaj, subject: "Siparis Detay!");

            return RedirectToAction("Index");

        }

        public PartialViewResult SearchProducts(string item,int? page)
        {
            ViewBag.Keyword = item;
            return PartialView(prep.Where(x => x.ProductName.Contains(item)).ToPagedList(page ?? 1, 9));
        }


    }
}