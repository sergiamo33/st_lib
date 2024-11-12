using CitizenFX.Core;
using CitizenFX.FiveM; // FiveM game related types (client only)
using CitizenFX.FiveM.Native; // FiveM natives (client only)
namespace Client
{
    public class Class1 : BaseScript
    {
        public Class1()
        {
            
        }
        
        [Command("dosomething", false, RemapParameters = true)]
        public void DoSomething(object[] args)
        {
            Debug.WriteLine(args[0].ToString());
        }
    }
}
