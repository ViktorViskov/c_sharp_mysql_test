// libs
using System;
using utils;

namespace ui
{
    // class for show interface
    class UI
    {
        // 
        // variables
        // 

        // 
        // constructor
        // 
        public UI()
        {

        }

        // 
        // methods
        // 

        // method for printing menu
        public int Menu(pageData container)
        {
            // variables
            bool exit = false;
            int itemNumber = 0;
            int currentPage = 0;
            int maxLineLength = Console.WindowWidth - 6;
            int maxLines = Console.WindowHeight - 6;
            int pageLines = maxLines <= container.items.Length - currentPage * maxLines ? maxLines : container.items.Length - currentPage * maxLines;
            int maxPages = (container.items.Length - 1) / maxLines;
            int pos_x = Console.WindowWidth / 2 - Langest(container.items) / 2;
            int pos_y = (Console.WindowHeight / 2 - pageLines / 2) - 1;

            // console initialization
            Console.CursorVisible = false;

            // start main interface loop
            while (!exit)
            {
                // clean console
                Console.Clear();

                // print header, page number and footer message
                Console.SetCursorPosition(Console.WindowWidth / 2 - container.header.Length / 2, 0);
                Console.Write(container.header);
                Console.SetCursorPosition(Console.WindowWidth / 2 - 6 / 2, Console.WindowHeight - 3);
                Console.Write($"Page {currentPage + 1}");
                Console.SetCursorPosition(Console.WindowWidth / 2 - container.footer.Length / 2, Console.WindowHeight - 1);
                Console.Write(container.footer);

                // init console
                Console.SetCursorPosition(pos_x, pos_y);

                // start printing
                for (int i = 0; i < maxLines && i + currentPage * maxLines < container.items.Length; i++)
                {
                    // print targeted item
                    if (i == itemNumber)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(container.items[i + currentPage * maxLines]);
                        Console.ResetColor();
                    }
                    // not targeted item
                    else
                    {
                        Console.Write(container.items[i + currentPage * maxLines]);
                    }

                    // set next cursor position
                    Console.SetCursorPosition(pos_x, pos_y + i + 1);

                }

                // read user input
                switch (Console.ReadKey(true).Key)
                {
                    // move down
                    case ConsoleKey.DownArrow:
                        itemNumber += itemNumber + 1 < pageLines ? 1 : 0;
                        break;

                    // move up
                    case ConsoleKey.UpArrow:
                        itemNumber -= itemNumber - 1 < 0 ? 0 : 1;
                        break;
                    // next page
                    case ConsoleKey.RightArrow:
                        itemNumber = 0;
                        currentPage += currentPage != maxPages ? 1 : 0;
                        break;

                    // previous page
                    case ConsoleKey.LeftArrow:
                        itemNumber = 0;
                        currentPage -= currentPage != 0 ? 1 : 0;
                        break;

                    // make action
                    case ConsoleKey.Spacebar:
                        return itemNumber + (currentPage * maxLines);

                    // cancel action
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }

                // update positions
                maxLineLength = Console.WindowWidth - 6;
                maxLines = Console.WindowHeight - 6;
                maxPages = (container.items.Length - 1) / maxLines;
                pageLines = maxLines <= container.items.Length - currentPage * maxLines ? maxLines : container.items.Length - currentPage * maxLines;
                pos_x = Console.WindowWidth / 2 - Langest(container.items) / 2;
                pos_y = (Console.WindowHeight / 2 - pageLines / 2) - 1;
            }

            // clean console
            Console.Clear();

            // return -1 becouse was canceled
            return -1;
        }

        // method for input screen
        public Object[] Input(pageData container)
        {
            // variables
            Object[] result = new Object[container.items.Length];
            bool exit = false;
            int itemNumber = 0;
            int langesItemInArray = Langest(container.items);
            int pos_x = Console.WindowWidth / 2 - langesItemInArray;
            int pos_y = Console.WindowHeight / 2 - container.items.Length / 2;

            // console initialization
            Console.CursorVisible = false;

            // start main interface loop
            while (!exit)
            {
                // clean console
                Console.Clear();

                // print header and footer message
                Console.SetCursorPosition(Console.WindowWidth / 2 - container.header.Length / 2, 0);
                Console.Write(container.header);
                Console.SetCursorPosition(Console.WindowWidth / 2 - container.footer.Length / 2, Console.WindowHeight - 1);
                Console.Write(container.footer);

                // init console
                Console.SetCursorPosition(pos_x, pos_y);

                // start printing
                for (int i = 0; i < container.items.Length; i++)
                {
                    // print targeted item
                    if (i == itemNumber)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(container.items[i]);
                        Console.SetCursorPosition(pos_x + langesItemInArray + 1, Console.GetCursorPosition().Top);
                        Console.Write(result[i]);


                        Console.ResetColor();
                    }
                    // not targeted item
                    else
                    {
                        Console.Write(container.items[i]);
                        Console.SetCursorPosition(pos_x + langesItemInArray + 1, Console.GetCursorPosition().Top);
                        Console.Write(result[i]);

                    }

                    // set next cursor position
                    Console.SetCursorPosition(pos_x, Console.GetCursorPosition().Top + 1);

                }

                // read user input
                switch (Console.ReadKey(true).Key)
                {
                    // move down
                    case ConsoleKey.DownArrow:
                        itemNumber += itemNumber + 1 < container.items.Length ? 1 : 0;

                        break;

                    // move up
                    case ConsoleKey.UpArrow:
                        itemNumber -= itemNumber - 1 < 0 ? 0 : 1;
                        break;

                    // start input mode
                    case ConsoleKey.Enter:
                        // cursor initialization
                        Console.SetCursorPosition(Console.WindowWidth / 2 + 1, pos_y + itemNumber);
                        Console.CursorVisible = true;

                        // start input mode
                        result[itemNumber] = Console.ReadLine();

                        //cursor hidden
                        Console.CursorVisible = false;

                        break;

                    // enter action
                    case ConsoleKey.Spacebar:
                        exit = true;
                        break;

                    // cancel action
                    case ConsoleKey.Escape:
                        return new Object[] { };
                }

                // update positions
                pos_x = Console.WindowWidth / 2 - langesItemInArray;
                pos_y = Console.WindowHeight / 2 - container.items.Length / 2;
            }

            // clean console
            Console.Clear();

            // return result
            return result;
        }

        // method for printing menu
        public void Message(pageData container)
        {

            // variables
            bool exit = false;
            int currentPage = 0;
            int maxLineLength = Console.WindowWidth - 6;
            int maxLines = Console.WindowHeight - 6;
            int pageLines = maxLines <= container.items.Length - currentPage * maxLines ? maxLines : container.items.Length - currentPage * maxLines;
            int maxPages = (container.items.Length - 1) / maxLines;
            int pos_x = Console.WindowWidth / 2 - Langest(container.items) / 2;
            int pos_y = (Console.WindowHeight / 2 - pageLines / 2) - 1;

            // console initialization
            Console.CursorVisible = false;






            // start main interface loop
            while (!exit)
            {
                // clean console
                Console.Clear();

                // print header, page number and footer message
                Console.SetCursorPosition(Console.WindowWidth / 2 - container.header.Length / 2, 0);
                Console.Write(container.header);
                Console.SetCursorPosition(Console.WindowWidth / 2 - 6 / 2, Console.WindowHeight - 3);
                Console.Write($"Page {currentPage + 1}");
                Console.SetCursorPosition(Console.WindowWidth / 2 - container.footer.Length / 2, Console.WindowHeight - 1);
                Console.Write(container.footer);

                // init console
                Console.SetCursorPosition(pos_x, pos_y);

                // start printing
                for (int i = 0; i < maxLines && i + currentPage * maxLines < container.items.Length; i++)
                {
                    // print item
                    Console.Write(container.items[i + currentPage * maxLines]);

                    // set next cursor position
                    Console.SetCursorPosition(pos_x, pos_y + i + 1);
                }

                // read user input
                switch (Console.ReadKey(true).Key)
                {
                    // next page
                    case ConsoleKey.RightArrow:
                        currentPage += currentPage != maxPages ? 1 : 0;
                        break;

                    // previous page
                    case ConsoleKey.LeftArrow:
                        currentPage -= currentPage != 0 ? 1 : 0;
                        break;

                    // exit action
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }

                // update positions
                maxLineLength = Console.WindowWidth - 6;
                maxLines = Console.WindowHeight - 6;
                maxPages = (container.items.Length - 1) / maxLines;
                pageLines = maxLines <= container.items.Length - currentPage * maxLines ? maxLines : container.items.Length - currentPage * maxLines;
                pos_x = Console.WindowWidth / 2 - Langest(container.items) / 2;
                pos_y = (Console.WindowHeight / 2 - pageLines / 2) - 1;
            }

            // clean console
            Console.Clear();
        }

        // search langest item in array and return size of this item
        private int Langest(string[] stringArray)
        {

            int result = 0;

            foreach (string item in stringArray)
            {
                if (item.Length > result)
                {
                    result = item.Length;
                }
            }
            return result;
        }
    }
}