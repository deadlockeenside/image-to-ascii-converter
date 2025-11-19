using System.Drawing;

namespace ImgToASCIIConverter
{
    public class ConvertService
    {
        private readonly char[] _asciiTableDark = { '.', ',', ':', '+', '*', '?', '%', '$', '#', '@' };
        private readonly char[] _asciiTableLight = { '@', '#', '$', '%', '?', '*', '+', ':', ',', '.' };
        private readonly Bitmap _bitmap;

        public ConvertService(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public char[][] ConvertToDark() => Convert(_asciiTableDark);
        public char[][] ConvertToLight() => Convert(_asciiTableLight);

        private char[][] Convert(char[] asciiTable)
        {
            var result = new char[_bitmap.Height][];

            for (int y = 0; y < _bitmap.Height; y++)
            {
                result[y] = new char[_bitmap.Width];

                for (int x = 0; x < _bitmap.Width; x++)
                {
                    int mapIndex = (int)ConvertRangeValue(_bitmap.GetPixel(x, y).R, 0, 255, 0, asciiTable.Length - 1);
                    result[y][x] = asciiTable[mapIndex];
                }
            }

            return result;
        }

        private float ConvertRangeValue(float value, float inMin, float inMax, float outMin, float outMax)
            => ((value - inMin) / (inMax - inMin)) * (outMax - outMin) + outMin;
    }
}
