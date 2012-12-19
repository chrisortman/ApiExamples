using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Api.Example.Security;
using Funq;
using ServiceStack.Common.Web;
using ServiceStack.Html;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using ServiceStack.VirtualPath;
using ServiceStack.WebHost.Endpoints;

namespace SimpleClient
{
  public class InProcessAppHost : AppHostHttpListenerBase
  {
      public InProcessAppHost() : base("InProcess Api Server",typeof(SecurityService).Assembly)
      {
      }

      public override void Configure(Container container)
      {
          
      }
  }
}