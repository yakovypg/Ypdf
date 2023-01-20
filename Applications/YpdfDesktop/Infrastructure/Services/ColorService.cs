using Avalonia.Media;

namespace YpdfDesktop.Infrastructure.Services
{
    internal static class ColorService
    {
        public static string GetHex(Color color)
        {
            string a = color.A.ToString("X2");
            string r = color.R.ToString("X2");
            string g = color.G.ToString("X2");
            string b = color.B.ToString("X2");

            return $"#{a}{r}{g}{b}";
        }
    }
}
