using System;

namespace OmoSeitokuEreceipt
{
    public static class SystemInfo
    {
        public static string Workgroup
        {
            get
            {
                var path = string.Format("Win32_ComputerSystem.Name='{0}'", Environment.MachineName);
                using (var sys = new System.Management.ManagementObject(path))
                {
                    return sys["Workgroup"].ToString();
                }
            }
        }
    }
}
