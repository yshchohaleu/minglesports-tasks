using System;

namespace Minglesports.Tasks.BuildingBlocks
{
    public static class Helpers
    {
        public static void Times(this int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }
    }
}