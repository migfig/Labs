using System;

namespace RelatedRows.Domain
{
    
    public partial class WindowViewModel 
    {
        public void WrapBlock(Action action, string template, params object[] props)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Logger.Log.Error(e, "Exception while " + template, props);
                MessageQueue.Enqueue(e.Message);
            }
        }

        public void WrapBlock(Action action, Action final, string template, params object[] props)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            {
                Logger.Log.Error(e, "Exception while " + template, props);
                MessageQueue.Enqueue(e.Message);
            }
            finally
            {
                final?.Invoke();
            }
        }
    }
}
