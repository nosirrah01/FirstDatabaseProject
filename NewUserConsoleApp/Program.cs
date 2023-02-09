namespace NewUserConsoleApp
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            string name = UIinator.AskName();
            if (!SqlDoer.ValueIsInColumn(name, "User", "Username"))
            {
                SqlDoer.AddNameToUser(name);
            }
            UIinator.DisplayUserShowsNRatings(name);
            int actionInt;
            do
            {
                actionInt = UIinator.GetValidInt(6, UIinator.GetActionQuestion());
                UIinator.DoAction(actionInt, name);
            } while (actionInt != 6);
        }

    }
}
