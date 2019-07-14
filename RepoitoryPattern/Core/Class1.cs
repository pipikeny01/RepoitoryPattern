using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using AutoMapper;
using AutoMapper.Configuration;

namespace MvcTemplate.Core
{
    /// <summary>
    /// 設定ViewModel要對應的Model。
    /// 這個用預設的Convention來對應
    /// </summary>
    /// <typeparam name="T">要被對應到的Type</typeparam>
    public interface IMapFrom<T>
    {
    }

    /// <summary>
    /// 設定ViewModel要對應的Model
    /// 如果需要客制AutoMapper的邏輯，讓ViewModel實作此Interface
    /// </summary>
    public interface IHaveCustomMapping
    {
        /// <summary>
        /// 設定自定義的Mapping邏輯
        /// </summary>
        /// <param name="configuration">Automapper的Config物件</param>
        void CreateMappings(IConfigurationProvider configuration);
    }

    public interface IRunAtStartup
    {
        void Execute();
    }

    /// <summary>
    /// 註冊有設定AutoMapper的viewmodel
    /// </summary>
    public class AutoMapperConfig : IRunAtStartup
    {
        /// <summary>
        /// 要執行的邏輯
        /// </summary>
        public void Execute()
        {
            var typeOfIHaveCustomMapping = typeof(IHaveCustomMapping);
            var typeOfIMapFrom = typeof(IMapFrom<>);

            // Type 符合 IHaveCustomMapping 和 IMapFrom 的 predicate方法
            // 這個predicate 的條件和下面個別mapping的第一個條件是一致的。
            Func<Type, bool> predicate = (t => typeOfIHaveCustomMapping.IsAssignableFrom(t) // 找到符合IHaveCustomMapping
                                               || t.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeOfIMapFrom).Any()); // 找到符合IMapFrom<>

            // 選擇要讀進來的Assembly - 只有符合IHaveCustomMapping 和 IMapFrom才讀
            var types = AssemblyTypes.GetAssemblyFromDirectory(assembly => assembly.GetExportedTypes().Where(predicate).Any())    
                // 把讀進來的Assembly取出裡面符合兩個interface的Type
                .SelectMany(x => x.GetExportedTypes()
                    .Where(predicate)).ToList();

            LoadStandardMappings(types);

            LoadCustomMappings(types);
        }

        /// <summary>
        /// 註冊如果使用是自定義邏輯的Mapping
        /// </summary>
        /// <param name="types">可能符合的Type</param>
        private static void LoadCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                from i in t.GetInterfaces()
                where typeof(IHaveCustomMapping).IsAssignableFrom(t) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select (IHaveCustomMapping)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(AutoMapper.Mapper.Configuration);
            }
        }

        /// <summary>
        /// Loads the standard mappings.
        /// </summary>
        /// <param name="types">The types.</param>
        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                from i in t.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select new
                {
                    Source = i.GetGenericArguments()[0],
                    Destination = t
                }).ToArray();

            foreach (var map in maps)
            {
                Mapper.Initialize(cfg => { cfg.CreateMap(map.Source, map.Destination); });
                //AutoMapper.Mapper.CreateMap(map.Source, map.Destination);
            }
        }

    }


    /// <summary>
    /// Autofac用來註冊Task相關的服務
    /// </summary>
    public class TaskModule : Autofac.Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            var assemblies = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assemblies).As<IRunAtStartup>();
        }
    }

}