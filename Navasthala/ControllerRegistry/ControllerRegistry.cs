﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace Navasthala.ControllerRegistry
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var controller =  ObjectFactory.GetInstance(controllerType) as IController;
            return controller;
        }
    }
}
