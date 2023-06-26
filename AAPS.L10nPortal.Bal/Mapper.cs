namespace CAPPortal.Bal
{
    public sealed class Mapper
    {
        private static readonly Mapper instance = new Mapper();





        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Mapper()
        {
        }

        private Mapper()
        {

        }

        public static Mapper Instance
        {
            get
            {
                return instance;
            }
        }


    }
}
