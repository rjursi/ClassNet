﻿using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace HookerProcess
{
    class TaskMgrController
    {
        public void KillTaskMgr()
        {
            RegistryKey regkey;
            string keyValueInt = "1";
            string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";

            try
            {
                regkey = Registry.CurrentUser.CreateSubKey(subKey, true);
                regkey.SetValue("DisableTaskMgr", keyValueInt);
                regkey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void EnableTaskMgr()
        {
            try
            {
                string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
                using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(subKey, true)){

                    if (rk != null)
                        rk.DeleteValue("DisableTaskMgr", false);

                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
            }
        }
    }
}
