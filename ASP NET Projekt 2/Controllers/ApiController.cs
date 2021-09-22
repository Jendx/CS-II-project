using Datalayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP_NET_Projekt_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        public async Task<IActionResult> GetJson(string APIKEY = " ", int offset = 0, int limit = 10)
        {
            if(limit > 50)
            {
                limit = 50;
            }
            JSONAPI js = new JSONAPI();
            var data = await js.GetUctenky();
            int total = data.Count;

            var users = await JSONAPI.GetUsers();
            bool isRegistered = false;
            foreach(var u in users)
            {
                if(u.APIKEY == APIKEY)
                {
                    isRegistered = true;
                    break;
                }
            }

            if(isRegistered)
            {
                var d = data
                .Skip(offset)
                .Take(limit)
                .ToList();

                return Ok(new
                {
                    Data = d,
                    Paging = new
                    {
                        Total = total,
                        Limit = limit,
                        Offset = offset,
                        Returnder = d.Count
                    }
                });
            }
            string Error;
            return Ok(Error = "APIKEY YOU PROVIDED IS INVALID! ACCESS DENIED!");
        }

    }
}
