using System;

namespace Logic
{
    public class Process
    {
        public Result ExecuteRoulette()
        {
            string[] colors = {"NEGRO","ROJO"};
            Result result = new Result();
            int number = 0;
            int color = 0;
            Random rdn = new Random();
            Random rdn2 = new Random();
            number = rdn.Next(0,37);
            color = rdn2.Next(0,2);
            if ((color == 0 && (number % 2 != 0)) || (color == 1 && (number % 2 == 0)))
            {
                result.color = colors[color];
                result.number = number;
            }
            else 
            {
                result = ExecuteRoulette();
            }

            return result;
        }        
    }
}
