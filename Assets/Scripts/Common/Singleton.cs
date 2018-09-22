using System;

namespace Common
{
  public class Singleton<T> where T : class, new()
  {
    private static volatile T m_instance;
    private static object m_sync_obj = new object (); 

    public static T Instance
    {
      get 
      {
        // double check
        if (m_instance == null)
        {
          // for multithread's conflict
          lock (m_sync_obj)
          { 
            if (m_instance == null)
            {
              m_instance = new T ();
            }
          }
        }
        return m_instance;
      }
    }
      
    protected Singleton () {}
  }
}
