using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
    public class AppUserMap:BaseMap<AppUser>
    {
        public AppUserMap()
        {
            Ignore(x => x.ConfirmPassword); //ConfirmPassword isimli property'i Sql tarafına göndermiyoruz.

            HasOptional(x => x.AppUserDetail).WithRequired(x => x.AppUser);
            ToTable("Kullanıcılar");
        }
    }
}
