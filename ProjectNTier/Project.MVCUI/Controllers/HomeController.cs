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
    public class HomeController : Controller
    {

        AppUserRepository aprep = new AppUserRepository();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Prefix = "Item1")]AppUser item1, [Bind(Prefix = "Item2")]AppUserDetail item2)
        {
            if (aprep.Any(x => x.UserName == item1.UserName))
            {
                ViewBag.ZatenVar = "Lutfen baska bir kullanıcı ismi giriniz";
                return View();
            }
            //Kullanıcı basarılı bir şekilde register işlemini tamamlıyorsa ona mail gönder..

            string gonderilecekMail = "Tebrikler hesabınız olusturulmustur. Hesabınızı aktive etmek icin http://localhost:51754/Home/Aktivasyon/" + item1.ActivationCode + " linkine tıklayabilirsiniz.";

            MailSender.Send(item1.Email, password: "Cf8885++--", body: gonderilecekMail, subject: "Hosgeldiniz!");

            aprep.Add(item1); //buradan sonra item1'in ID'si olusmus oluyor. O yüzden item2'nin ID'si verecek isek buradan sonra vermeliyiz.
            item2.ID = item1.ID;
            apdRep.Add(item2);

            return View("RegisterOk");
        }

        AppUserDetailRepository apdRep = new AppUserDetailRepository();

        public ActionResult Aktivasyon(Guid id)
        {
            if (aprep.Any(x => x.ActivationCode == id))
            {
                aprep.Default(x => x.ActivationCode == id).IsActive = true;

                TempData["HesapAktif"] = "Hesabınız aktif hale getirildi";
                if (Session["bekleyenUrun"] != null)
                {
                    //Product bekleyenUrun = Session["bekleyenUrun"] as Product;
                    //CartItem ci = new CartItem();
                    //ci.ID = bekleyenUrun.ID;
                    //ci.Name = bekleyenUrun.ProductName;
                    //ci.Price = Convert.ToDecimal(bekleyenUrun.UnitPrice);
                    //ci.ImagePath = bekleyenUrun.ImagePath;
                    //Cart c = new Cart();
                    //c.SepeteEkle(ci);


                    Session["scart"] = ControlProduct.KeepProduct(Session["bekleyenUrun"]);
                }




                return RedirectToAction("Index", "Member");
            }
            TempData["HesapAktif"] = "Aktif edilecek hesap bulunamadı";
            return RedirectToAction("Register");
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AppUser item)
        {
            if (aprep.Any(x => x.UserName == item.UserName && x.Password == item.Password && x.Status != MODEL.Enums.DataStatus.Deleted && x.UserRole == MODEL.Enums.Role.Admin))
            {
                AppUser girisYapan = aprep.Default(x => x.UserName == item.UserName && x.Password == item.Password && x.Status != MODEL.Enums.DataStatus.Deleted && x.UserRole == MODEL.Enums.Role.Admin);

                Session["admin"] = girisYapan;

                //todo Area yönlendirmesi 
                return RedirectToAction("");
            }
            else if (aprep.Any(x => x.UserName == item.UserName && x.Password == item.Password && x.UserRole == MODEL.Enums.Role.Member && x.Status != MODEL.Enums.DataStatus.Deleted))
            {
                AppUser girisYapanUye = aprep.Default(x => x.UserName == item.UserName && x.Password == item.Password && x.UserRole == MODEL.Enums.Role.Member && x.Status != MODEL.Enums.DataStatus.Deleted);

                Session["member"] = girisYapanUye;
                if (Session["bekleyenUrun"]!=null)
                {
                    Session["scart"] = ControlProduct.KeepProduct(Session["bekleyenUrun"]);

                }

                return RedirectToAction("Index", "Member");
            }

            ViewBag.KullaniciBulunamadi = "Böyle bir kullanıcı yoktur";
            return View();
        }
    }
}