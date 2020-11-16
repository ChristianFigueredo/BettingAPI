using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataAccess;
using Objects.output;
using Objects.input;
using System.Net.Http;

namespace BettingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BettingController : ControllerBase
    {
        [HttpGet]
        public string start()
        {
            return ("Hello guys, my name is Christian Figueredo. Nice to meet you");
        }

        [HttpPost]
        public Message CreateBet(BetRequest request, [FromHeader] string token)
        { 
            Message message = new Message();
            try
            {
                if (request != null)
                {
                    if (!String.IsNullOrEmpty(token))
                    {
                        request.token = token;
                        if (!String.IsNullOrEmpty(request.color) && request.amount != 0 && request.rouletteId != 0)
                        {
                            DB db = new DB();
                            message = db.CreateBet(request);
                        }
                        else
                        {
                            message = new Message("0021", "Ocurrio un error, los campos son requeridos", null);
                        }
                    }
                    else 
                    {
                        message = new Message("0020", "Ocurrio un error, se requiere un token para completar la solicitud", null);
                    }
                }
                else 
                {
                    message = new Message("0018", "Ocurrio un error, se requiere el cuerpo de la peticion.", null);
                }
            }
            catch (Exception ex) 
            {
                message = new Message("0017", "Ocurrio un error.", ex);
            }

            return message;
        }
    }
}
