using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinConsole
{
  public class OrderController : ApiController
  {
    public Order Get()
    {
      return new Order() { Text = "execute Object order 88" };
    }
  }
}
