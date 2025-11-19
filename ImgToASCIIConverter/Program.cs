using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImgToASCIIConverter
{
    internal class Program
    {
        private const double WIDTH_OFFSET = 1.5;

        [STAThread]
        static void Main(string[] args)
        {
            // Это консольное приложение, но здесь подключена dll winforms, чтобы изображение можно было открывать в окне, а не писать путь до файла в консоли
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images | *.bmp; *.png; *.jpg; *.JPEG"
            };

            Console.WriteLine("Для запуска нажмите Enter . . .");

            while (true) 
            {
                Console.ReadLine();

                // Чтобы запуск окна работал, необходимо у метода мэин добавить атрибут[STAThread], иначе будет ошибка
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    continue;

                Console.Clear();

                var bitmap = ResizeBitmap(new Bitmap(openFileDialog.FileName));

                // см. класс Extensions
                bitmap.ToGrayScale();

                var converter = new ConvertService(bitmap);
                var darkImgRows = converter.ConvertToDark();
                var lightImgRows = converter.ConvertToLight();

                // Т. к. консоль черная, для нее лучше подходит темный вариант
                foreach (var row in darkImgRows)
                    Console.WriteLine(row);

                // Сохраняются оба варианта
                File.WriteAllLines("dark_image.txt", darkImgRows.Select(r => new string(r)));
                File.WriteAllLines("light_image.txt", lightImgRows.Select(r => new string(r)));

                Console.Write("\nИзображения в формате .txt сохранены рядом с приложением! :)\nДля конвертации нового изображения нажмите Enter . . .");
            }
        }

        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var maxWidth = Console.WindowWidth - 1;

            // Сохранение пропорций изображения
            var newHeight = bitmap.Height / WIDTH_OFFSET * maxWidth / bitmap.Width;

            if (bitmap.Width > maxWidth || bitmap.Height > newHeight)
                bitmap = new Bitmap(bitmap, new Size(maxWidth, (int)newHeight));

            return bitmap;
        }
    }
}
