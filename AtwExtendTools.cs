// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using Wox.Infrastructure;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Everything
{
    internal sealed class AtwExtendTools
    {
        public static void Add(List<ContextMenuResult> contextMenus, SearchResult record, bool isFile)
        {
            AddEclipseMenu(contextMenus, record, isFile);
        }

        private static void AddEclipseMenu(List<ContextMenuResult> contextMenus, SearchResult record, bool isFile)
        {
            // 只对目录有效
            if (isFile) return;

            // 目录下是否有Exclipse的相关文件
            if (!Path.Exists(Path.Combine(record.Path, "pom.xml"))) return;
            if (!Path.Exists(Path.Combine(record.Path, "core"))) return;
            if (!Path.Exists(Path.Combine(record.Path, "timesheet"))) return;
            if (!Path.Exists(Path.Combine(record.Path, "webapp"))) return;
            if (!Path.Exists(Path.Combine(record.Path, "wwwroot"))) return;

            // 获取环境变量中定义的eclipse路径
            string eclipseExePath = Environment.GetEnvironmentVariable("ATW_ECLIPSE_EXE_PATH");
            if (!Path.Exists(eclipseExePath)) return;

            // https://learn.microsoft.com/en-us/windows/apps/design/style/segoe-ui-symbol-font
            contextMenus.Add(new ContextMenuResult
            {
                PluginName = Assembly.GetExecutingAssembly().GetName().Name,
                Title = "使用Eclipse打开该项目(Ctrl+Shift+D)",
                Glyph = "\xEC7A",
                FontFamily = "Segoe MDL2 Assets",
                AcceleratorKey = Key.D,
                AcceleratorModifiers = ModifierKeys.Control | ModifierKeys.Shift,
                Action = (context) =>
                {
                    if (!Helper.OpenInShell(eclipseExePath, $"-data \"{record.Path}\" -showlocation"))
                    {
                        return false;
                    }

                    return true;
                },
            });
        }
    }
}
