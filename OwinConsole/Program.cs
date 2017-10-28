using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin.Hosting;

namespace OwinConsole
{
  class Program
  {
    static void Main(string[] args)
    {
      string uri = "http://localhost:8080";

      using (WebApp.Start<Settings>(uri))
      {
        Console.WriteLine("Server started");
        Console.ReadKey();
        Console.ReadKey();
        Console.WriteLine("Server closed");
      }
    }
  }

  class Settings
  {
    public void Configuration(IAppBuilder app)
    {
      app.UseWelcomePage();
      //app.Run((ctx) => { return ctx.Response.WriteAsync("execute order 88"); });
    }
  }
}

