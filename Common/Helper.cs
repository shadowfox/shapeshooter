using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace Common
{
    public static class Helper
    {
        public static long getRUI(NetIncomingMessage msg)
        {
            return msg.SenderConnection.RemoteUniqueIdentifier;
        }

        public static string getRUIHex(NetIncomingMessage msg)
        {
            return NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier);
        }

        public static string getRemoteTag(NetIncomingMessage msg)
        {
            return String.Format("{0}:{1}", msg.SenderEndpoint.ToString(), getRUIHex(msg));
        }
    }
}
