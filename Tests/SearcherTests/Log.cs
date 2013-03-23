using System;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

public static class Log
{
    public static void Clear()
    {
        var memoryAppender = GetAppender();
        memoryAppender.Clear();
    }

    public static string Content
    {
        get
        {
            var memoryAppender = GetAppender();
            var ResultBuffer = new StringBuilder();
            ResultBuffer.AppendFormat("{0}Test Log:{0}",Environment.NewLine);
            foreach (var evt in memoryAppender.GetEvents())
            {
                ResultBuffer.AppendFormat("{0}{1}", evt.RenderedMessage, Environment.NewLine);
            }
            return ResultBuffer.ToString();
        }
    }

    private static MemoryAppender GetAppender()
    {
        Hierarchy hierarchy;
        MemoryAppender memoryAppender;

        // Get the default hierarchy for log4net
        hierarchy = (Hierarchy) LogManager.GetRepository();

        // Get the appender named "MemoryAppender" from the <root> logger 
        memoryAppender =
            (MemoryAppender)hierarchy.Root.GetAppender("MemoryAppender");

        if (memoryAppender == null)
        {
            throw new ApplicationException(
                        String.Format("Expected type in Repositary on position 0 - Memory Appender but was {0}",
                        LogManager.GetRepository().GetAppenders()[0].GetType()));
        }
        return memoryAppender;
    }
}

