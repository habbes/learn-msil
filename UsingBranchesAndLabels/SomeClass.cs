namespace UsingBranchesAndLabels
{
    public class SomeClass
    {
        public static int Calculate(int x)
        {
            int result = 0;
            for (int i = 0; i < 10; i++)
            {
                result += i * x;
            }

            return result;
        }
    }
}