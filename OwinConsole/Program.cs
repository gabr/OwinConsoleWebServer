using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using Microsoft.Owin.Hosting;

namespace OwinConsole
{
  using AppFunc = Func<IDictionary<string, object>, Task>;

  class Program
  {
    static void Main(string[] args)
    {
      string uri = "http://localhost:8080";

      using (WebApp.Start<Settings>(uri))
      {
        Console.WriteLine("Server started");
        Console.ReadKey();
        Console.WriteLine("Server closed");
      }
    }
  }

  class Settings
  {
    public void Configuration(IAppBuilder app)
    {
      //app.Run((ctx) => { return ctx.Response.WriteAsync("execute order 88"); });
      //app.UseWelcomePage();

      app.Use(async (environment, next) =>
      {
        foreach (var k in environment.Environment.Keys)
          Console.WriteLine($"{k} -> [{environment.Environment[k]?.GetType()?.Name ?? "null"}] '{environment.Environment[k]?.ToString() ?? "null"}'");

        /* example result:

            owin.RequestPath        -> [String] '/'
            owin.ResponseHeaders    -> [ResponseHeadersDictionary] 'Microsoft.Owin.Host.HttpListener.RequestProcessing.ResponseHeadersDictionary'
            owin.RequestHeaders     -> [RequestHeadersDictionary]  'Microsoft.Owin.Host.HttpListener.RequestProcessing.RequestHeadersDictionary'
            owin.ResponseBody       -> [HttpListenerStreamWrapper] 'Microsoft.Owin.Host.HttpListener.RequestProcessing.HttpListenerStreamWrapper'
            owin.RequestBody        -> [HttpListenerStreamWrapper] 'Microsoft.Owin.Host.HttpListener.RequestProcessing.HttpListenerStreamWrapper'
            owin.RequestId          -> [String] '00000000-0000-0000-6000-0080020000fa'
            owin.ResponseStatusCode -> [Int32] '200'
            owin.RequestQueryString -> [String] ''
            owin.CallCancelled      -> [CancellationToken] 'System.Threading.CancellationToken'
            owin.RequestMethod      -> [String] 'GET'
            owin.RequestScheme      -> [String] 'http'
            owin.RequestPathBase    -> [String] ''
            owin.RequestProtocol    -> [String] 'HTTP/1.1'
            owin.Version            -> [String] '1.0'
            host.TraceOutput        -> [DualWriter] 'Microsoft.Owin.Hosting.Tracing.DualWriter'
            host.AppName            -> [String] 'OwinConsole.Settings, OwinConsole, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'
            host.OnAppDisposing     -> [CancellationToken] 'System.Threading.CancellationToken'
        */

        await next();
      });

      app.Use(async (environment, next) =>
      {
        Console.WriteLine("Requesting: " + environment.Request.Path);
        await next();
        Console.WriteLine("Response: " + environment.Response.StatusCode);
      });

      app.UseOrderPage();
    }
  }

  static class OrderComponentAppBuilderExtension
  {
    public static void UseOrderPage(this IAppBuilder app)
    {
      app.Use<OrderComponent>();
    }
  }

  class OrderComponent // known also as Middleware
  {
    private AppFunc _nextComponentInvoke;
    public OrderComponent(AppFunc next)
    {
      _nextComponentInvoke = next;
    }

    public Task Invoke(IDictionary<string, object> environment)
    {
      var response = environment["owin.ResponseBody"] as Stream;
      using (var sw = new StreamWriter(response))
      {
        return sw.WriteAsync("execute order 88");
      }

      // !!!! NEVER USES _nextComponentInvoke so next components are not called
    }
  }
}

