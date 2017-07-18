﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Weather.WebApi.Service;

namespace Weather.WebApi.App_Start
{
    public class AutofacWebapiConfig
    {
        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            //Register your Web API controllers.  
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            
            var webApiResolver = new AutofacWebApiDependencyResolver(Container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
            
            builder.RegisterType<WeatherService>().As<IWeatherService>();
            builder.RegisterType<GlobalWeather.GlobalWeatherSoapClient>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<HttpClient>().SingleInstance();
            

            return Container;
        }
    }
}