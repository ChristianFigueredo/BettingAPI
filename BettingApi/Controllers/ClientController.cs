using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Objects.output;
using DataAccess;
using Objects.input;

namespace BettingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpPost]
        public Message Post([FromBody] NewClientRequest request)
        {
            Message message = new Message();
            try
            {
                if (request != null)
                {
                    if (!String.IsNullOrEmpty(request.name) && !String.IsNullOrEmpty(request.username) && !String.IsNullOrEmpty(request.password))
                    {
                        DB db = new DB();
                        message = db.CreateClient(request);
                    }
                    else
                    { 
                        message = new Message("0009", "Ocurrio un error. Todos los datos son requeridos", null);
                    }
                }
                else 
                {
                    message = new Message("0008", "Ocurrio un error. El body de la peticion es requerido", null);
                }
            }
            catch (Exception ex) 
            {
                message = new Message("0007", "Ocurrio un error.", ex);
            }

            return message;
        }

        [HttpPut("Session")]
        public SessionTokenResponse Put([FromBody] ClientSessionRequest request)
        {
            SessionTokenResponse response = new SessionTokenResponse();
            try
            {
                if (request != null)
                {
                    if (!String.IsNullOrEmpty(request.username) && !String.IsNullOrEmpty(request.password))
                    {
                        DB db = new DB();
                        response = db.GetSessionToken(request);
                    }
                    else
                    {
                        response.message = new Message("0014", "Ocurrio un error. Todos los datos son requeridos", null);
                    }
                }
                else
                {
                    response.message = new Message("0013", "Ocurrio un error. El body de la peticion es requerido", null);
                }
            }
            catch (Exception ex)
            {
                response.message = new Message("0012", "Ocurrio un error.", ex);
            }

            return response;
        }
    }
}
