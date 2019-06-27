using Project.DAL.Context;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus.DataSets;

namespace Project.DAL.Strategy
{
    //Bu sınıfımız icerisindeki görevler sayesinde veritabanı olustugunda onun icerisinde hazır veriler olmasını saglayacak. FakeData icin Bogus kütüphanesini indiriyoruz.

        //Bu sınıfın Veritabanı olustugunda verileri hazır olarak yaratabilmesi icin bu tarz görevi üstlenen hazır sistem sınıflarından miras alması lazım...

        //Bir veritabanını CodeFirst'te hazır verilerle ayaga kaldırmak icin bizim işimize yarayacak belirli sınıflardan miras alması gerekir. CreateDatabaseIfNotExists,DropCreateDatabaseIfModelChanges,DropCreateDatabaseAlways gibi sınıflar burada hazır verilerle veritabanını olusturmamızı saglayabilir. Bu sınıfların Seed isimli bir metodu vardır.

        //Veritabanı olusutkrne veri eklenmesini de Seed metodunu ezerek saglarız.
    public class MyInitializer:CreateDatabaseIfNotExists<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                AppUser ap = new AppUser();
               
                ap.UserName = new Internet("tr").UserName();
                ap.Password = new Internet("tr").Password();
                ap.Email = new Internet("tr").Email();
                context.AppUsers.Add(ap);
            }
            context.SaveChanges();

            for (int i = 1; i < 11; i++)
            {

                AppUserDetail apd = new AppUserDetail();
                apd.ID = i; // Birebir ilişki oldugundan dolayı id'leri elle verdik.
                apd.FirstName = new Name("tr").FirstName();

                apd.LastName = new Name("tr").LastName();

                apd.Address = new Address("tr").Locale;
                context.AppUserDetails.Add(apd);
            }
            context.SaveChanges();

            for (int i = 0; i < 50; i++)
            {
                Category c = new Category();
                c.CategoryName = new Commerce("tr").Categories(1)[0];
                c.Description = new Lorem("tr").Sentence(100);

                for (int x = 0; x < 50; x++)
                {
                    Product p = new Product();
                    p.ProductName = new Commerce("tr").ProductName();
                    p.UnitPrice = Convert.ToDecimal(new Commerce("tr").Price());
                    p.ImagePath = new Images("en").Nightlife();
                    

                    c.Products.Add(p);
                }
                context.Categories.Add(c);
                context.SaveChanges();
            }

        }
    }
}
