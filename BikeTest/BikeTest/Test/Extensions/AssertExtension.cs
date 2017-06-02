namespace BikeTest.Test.Extensions
{
    using System;
    public static class AssertExtension
    {
        public static void TryAssert(int count, Action primary, Action alternative)
        {
            for (int i = 0; i < count; i++)
            {
                try
                {
                    primary();
                    break;
                }
                catch(Exception ex)
                {
                    alternative();

                    if (count - 1 == i)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
