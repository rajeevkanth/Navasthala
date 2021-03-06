﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Navasthala.ControllerRegistry;
using WebMatrix.WebData;

namespace Navasthala
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Register();
            InitialiseDependencies();
            InitialiseDataBase();
        }

        private void InitialiseDependencies()
        {
            BootStrapper.BootStrapper.ConfigureDependencies();
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
        }

        private void InitialiseDataBase()
        {
            var migrator = new DbMigrator(new DataLayer.Migrations.Configuration());
            migrator.Update();
           
            if (!WebSecurity.Initialized)
            WebSecurity.InitializeDatabaseConnection("NavasthalaContext", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }

        private void Register()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            
        }

    }

    }