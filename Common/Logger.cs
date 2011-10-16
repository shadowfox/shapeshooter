using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Logger
    {
        // Error levels
        public static string LEVEL_ERROR = "ERROR";
        public static string LEVEL_WARNING = "WARNI";
        public static string LEVEL_INFO = "INFOR";
        public static string LEVEL_DEBUG = "DEBUG";

        public Logger()
        {
        }

        public void Log(string level, string message)
        {
            Console.WriteLine(
                String.Format("[{0}] [{1}] {2}", level, currentTime(), message)
            );
        }

        public void Log(string level, string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(level, String.Format(message, formatArgs));
            }
            else
            {
                Log(level, message);
            }
        }

        // ----------------------------------------
        // Error logging
        // ----------------------------------------

        public void LogError(string message)
        {
            this.Log(LEVEL_ERROR, message);
        }

        public void LogError(string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(LEVEL_ERROR, String.Format(message, formatArgs));
            }
            else
            {
                Log(LEVEL_ERROR, message);
            }
        }

        public void Error(string message)
        {
            this.Log(LEVEL_ERROR, message);
        }

        public void Error(string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(LEVEL_ERROR, String.Format(message, formatArgs));
            }
            else
            {
                Log(LEVEL_ERROR, message);
            }
        }

        // ----------------------------------------
        // Warning logging
        // ----------------------------------------

        public void LogWarning(string message)
        {
            this.Log(LEVEL_WARNING, message);
        }

        public void LogWarning(string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(LEVEL_WARNING, String.Format(message, formatArgs));
            }
            else
            {
                Log(LEVEL_WARNING, message);
            }
        }

        public void Warning(string message)
        {
            this.Log(LEVEL_WARNING, message);
        }

        public void Warning(string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(LEVEL_WARNING, String.Format(message, formatArgs));
            }
            else
            {
                Log(LEVEL_WARNING, message);
            }
        }

        // ----------------------------------------
        // Info logging
        // ----------------------------------------

        public void LogInfo(string message)
        {
            this.Log(LEVEL_INFO, message);
        }

        public void LogInfo(string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(LEVEL_INFO, String.Format(message, formatArgs));
            }
            else
            {
                Log(LEVEL_INFO, message);
            }
        }

        public void Info(string message)
        {
            this.Log(LEVEL_INFO, message);
        }

        public void Info(string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(LEVEL_INFO, String.Format(message, formatArgs));
            }
            else
            {
                Log(LEVEL_INFO, message);
            }
        }

        // ----------------------------------------
        // Debug logging
        // ----------------------------------------

        public void LogDebug(string message)
        {
            this.Log(LEVEL_DEBUG, message);
        }

        public void LogDebug(string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(LEVEL_DEBUG, String.Format(message, formatArgs));
            }
            else
            {
                Log(LEVEL_DEBUG, message);
            }
        }

        public void Debug(string message)
        {
            this.Log(LEVEL_DEBUG, message);
        }

        public void Debug(string message, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Log(LEVEL_DEBUG, String.Format(message, formatArgs));
            }
            else
            {
                Log(LEVEL_DEBUG, message);
            }
        }

        public string currentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
