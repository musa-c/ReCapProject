﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Helper.FileHelper;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CarManager>().As<ICarService>().SingleInstance();
            builder.RegisterType<EfCarDal>().As<ICarDal>().SingleInstance();

            builder.RegisterType<CarImageManager>().As<ICarImageService>().SingleInstance();
            builder.RegisterType<EfCarImageDal>().As<ICarImageDal>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();


            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<FileHelper>().As<IFileHelper>().SingleInstance();
            //GetExecutingAssembly(): çalışan uygulama içerisinde
            // AsImplementedInterfaces(): implemente edilmiş interface'leri bul
            // onlar için AspectInterceptorSelector'i çağır. yani yukarıda tanımlanan sınıflar için 
            // (attribute)aspect'i olanlar için önce AspectInterceptorSelector()'i çalıştırır.
            // Yani yukarıda tanımlalanan bütün sınıflar için önce AspectInterceptorSelector()'ı
            // çalıştırır aspect'i sınıflarda aspect(attribute) olup olmadığına bakar.
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
