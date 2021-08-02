using System;
using System.Linq;
using System.Threading.Tasks;
using Cyrillic.Convert;
using Refit;

namespace HelsiScanner
{
    public static class Program
    {
        private const string HelsiBaseUri = "https://helsi.me";

        public static async Task<int> Main(string[] args)
        {
            Console.WriteLine("Helsi Scanner");

            try
            {
                var helsiClient = RestService.For<IHelsiClient>(HelsiBaseUri);
                var scanner = new Scanner(helsiClient);
                var slots = await scanner.ScanAsync();

                Console.WriteLine($"Found {slots.Count()} open slots.");

                foreach (var slot in slots)
                {
                    Console.WriteLine(slot.ToRussianLatin());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR:");
                Console.WriteLine(ex);
                return 1;
            }

            Console.WriteLine("DONE");

            return 0;
        }
    }
}
