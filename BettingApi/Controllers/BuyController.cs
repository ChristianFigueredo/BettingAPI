using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Objects.input;
using Objects.output;

namespace BettingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        [HttpPut]
        public Message Put( [FromBody] GetCreditCoinsRequest request)
        {
            Message message = new Message();
            try
            {
                if (request != null)
                {
                    if (request.amount > 0 && request.clientId > 0)
                    {
                        DB db = new DB();
                        message = db.GetCreditCoins(request);
                    }
                    else 
                    {
                        message = new Message("0035", "Ocurrio un error, El valor de los campos debe ser mayor a cero.", null);
                    }
                }
                else 
                {
                    message = new Message("0034", "Ocurrio un error, se requiere el cuerpo de la peticion.", null);
                }
            }
            catch (Exception ex) 
            {
                message = new Message("0033", "Ocurrio un error.", ex);
            }
            return (message);
        }
    }
}
