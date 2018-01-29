using System;
namespace EmptyEngine
{
    public static class EventExecute
    {
        static public void Execute(this Action action)
        {
            if (action == null)
                return;
            action();
        }
    }
}
