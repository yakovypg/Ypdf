namespace ypdf.Informing
{
    internal static class Guide
    {
        public static void Print()
        {
            using (var reader = new StreamReader(SharedConfig.Files.Readme))
            {
                while (!reader.EndOfStream)
                {
                    Console.WriteLine(reader.ReadLine());
                }
            }
        }

        public static void PrintSection(string sectionName)
        {
            if (string.IsNullOrEmpty(sectionName))
                return;

            using (var reader = new StreamReader(SharedConfig.Files.Readme))
            {
                bool print = false;
                int sectionDepth = int.MinValue;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    bool isSection = line.StartsWith('#');

                    int curSectionDepth = isSection
                        ? line.IndexOf(' ')
                        : -1;

                    if (isSection && curSectionDepth <= sectionDepth)
                        break;

                    string curSectionName = curSectionDepth >= 0 && curSectionDepth < line.Length - 1
                        ? line[(curSectionDepth + 1)..]
                        : string.Empty;

                    if (curSectionName == sectionName)
                    {
                        print = true;
                        sectionDepth = curSectionDepth;
                    }

                    if (print)
                        Console.WriteLine(line);
                }
            }
        }

        public static void TryPrint()
        {
            TryPrint(Print);
        }

        public static void TryPrintSection(string sectionName)
        {
            TryPrint(() => PrintSection(sectionName));
        }

        private static void TryPrint(Action printMethod)
        {
            try
            {
                printMethod?.Invoke();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Guide file not found.");
            }
            catch
            {
                Console.WriteLine("Could not read data from guide file.");
            }
        }
    }
}
