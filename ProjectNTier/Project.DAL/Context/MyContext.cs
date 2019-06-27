﻿using Project.DAL.Strategy;
using Project.MAP.Options;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Context
{
    public class MyContext:DbContext
    {
        public MyContext():base("ProjectCon")
        {

            //todo Password validation

            Configuration.ValidateOnSaveEnabled = false;



            Database.SetInitializer(new MyInitializer()); //SetInitializer...Unutmayın SetInitializer Metodu normal migration senaryolarında ayaga kalkamaz. Eger veritabanınızı illa belli baslangıc verileriyle olusturmak istiyorsanız ona bir yerden istek göndermek en saglıklısıdır.
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AppUserMap());
            modelBuilder.Configurations.Add(new AppUserDetailMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new OrderDetailMap());
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<AppUserDetail> AppUserDetails { get; set; }


    }
}
