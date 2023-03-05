namespace Api.Helpers
{
    public class Helpers
    {
        public static string GenerateRandomString()
        {
            Random res = new();

            // String of alphabets 
            string str = "abcdefghijklmnopqrstuvwxyz";
            int size = 10;

            // Initializing the empty string
            string ran = "";

            for (int i = 0; i < size; i++)
            {

                // Selecting a index randomly
                int x = res.Next(26);

                // Appending the character at the 
                // index to the random string.
                ran += str[x];
            }

            return ran;
        }

        public static int GenerateRandomNumber(int max, int min = 0)
        {
            return new Random().Next(min, max);
        }
    }
}