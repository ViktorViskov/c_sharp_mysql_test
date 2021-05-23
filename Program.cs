using System;
using ui;
using utils;

namespace console_menu
{
    class Program
    {
        static void Main(string[] args)
        {
            UI disp = new UI();

            pageData someMenu = new pageData("Menu page","ESC > exit, Up Down > Navigation, Space > select", new string[]{"menu_one","menu_two","menu_three"});
            pageData someInput = new pageData("Input page","ESC > cancel, Up Down > Navigation, Enter > input data, Space > continue", new string[]{"input_one","input_two","input_three"});
            pageData someMesasge = new pageData("Message page","Press any key to continue...", new string[]{"Line one","line two","line three"});

            // Console.Write(disp.Menu(someMenu));
            // disp.Message(someMesasge);

            foreach (var item in disp.Input(someInput))
            {
                Console.WriteLine(item);
            }
        }

    }
}
