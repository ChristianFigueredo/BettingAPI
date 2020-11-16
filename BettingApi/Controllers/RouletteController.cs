using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Objects.output;

namespace BettingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        [HttpGet]
        public RouletteResponse1 List()
        {
            RouletteResponse1 response = new RouletteResponse1();
            try
            {
                DB db = new DB();
                response = db.GetRoulettes();
            }
            catch (Exception ex)
            {
                response.message = new Message("0000", "Transaccion exitosa", ex);
            }

            return response;
        }


        [HttpPost]
        public RouletteResponse2 Create()
        {
            RouletteResponse2 response = new RouletteResponse2();
            try
            {
                DB db = new DB();
                response = db.CreateRoulette();
            }
            catch (Exception ex) 
            {
                response.message = new Message("0003", "Transaccion fallida", ex);
            }

            return response; 
        }

        [HttpPut("Open/{rouletteId}")]
        public Message Open( int rouletteId )
        {
            Message message = new Message();
            try
            {
                DB db = new DB();
                message = db.OpenRoulette(rouletteId);
            }
            catch (Exception ex) 
            {
                message = new Message("0007", "Transaccion fallida.", ex);
            }

            return message;
        }

        [HttpPut("Close/{rouletteId}")]
        public CloseRouletteResponse Close(int rouletteId)
        {
            CloseRouletteResponse response = new CloseRouletteResponse();
            try
            {
                DB db = new DB();
                response = db.CloseRoulette(rouletteId);
            }
            catch (Exception ex)
            {
                response.message = new Message("0024", "Transaccion fallida.", ex);
            }

            return response;
        }

    }
}
