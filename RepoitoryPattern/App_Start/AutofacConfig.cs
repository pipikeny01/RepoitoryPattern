using Autofac;
using Autofac.Integration.Mvc;
using MvcTemplate.Core;
using MvcTemplate.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MvcTemplate.Core.Log;

namespace MvcTemplate
{
    public class AutofacConfig
    {
        /// <summary>
        /// 註冊DI注入物件資料
        /// </summary>
        public static void Register()
        {
            // 容器建立者
            ContainerBuilder builder = new ContainerBuilder();

            // 註冊Controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());


            // 註冊DbContextFactory
            string connectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["Entities"].ConnectionString;

            builder.RegisterType<DbContextFactory>()
                .WithParameter("connectionString", connectionString)
                .As<IDbContextFactory>()
                .InstancePerRequest();

            // 註冊 Repository UnitOfWork
            builder.RegisterGeneric(typeof(DBRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType(typeof(EFUnitOfWork)).As(typeof(IUnitOfWork));

            // 註冊Services
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces();

            //註冊LogModule
            builder.RegisterModule<NLogModule>();

            // 建立容器
            IContainer container = builder.Build();
            // 解析容器內的型別
            AutofacDependencyResolver resolver = new AutofacDependencyResolver(container);

            // 建立相依解析器
            DependencyResolver.SetResolver(resolver);
        }
    }
}