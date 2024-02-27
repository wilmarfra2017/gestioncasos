using System.Security.Cryptography;

namespace GestionCasos.Domain.Utils.GeneradorId;

public class GeneradorNumeroAleatorio : IGeneradorNumeroAleatorio
{
    public int Generar(int minValue, int maxValue)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            var intValue = Math.Abs(BitConverter.ToInt32(randomNumber, 0));
            return intValue % (maxValue - minValue + 1) + minValue;
        }
    }
}

