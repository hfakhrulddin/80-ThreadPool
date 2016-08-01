// Useful way to store info that can be passed as a state on a work item

namespace ThreadPoolG
{
    public class SomeState
    {
            public int Cookie;

            public SomeState(int iCookie)
            {
                Cookie = iCookie;
            } 
    }
}
