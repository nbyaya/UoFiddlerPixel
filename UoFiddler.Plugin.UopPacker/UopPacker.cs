/***************************************************************************
 *
 * $Author: Turley
 *
 * "啤酒许可证"
 * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
 * 如果我们某天相遇，并且你认为这个东西有价值，
 * 你可以请我喝杯啤酒作为回报。
 *
 ***************************************************************************/

using System;
using System.Windows.Forms;
using Ultima;
using UoFiddler.Controls.Plugin;
using UoFiddler.Controls.Plugin.Interfaces;
using UoFiddler.Plugin.UopPacker.UserControls;

namespace UoFiddler.Plugin.UopPacker
{
    public class UopPacker : PluginBase
    {
        private readonly Version _ver;

        public UopPacker()
        {
            _ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public override string Name { get; } = "UOP 打包器";

        /// <summary>
        /// 插件用途描述
        /// </summary>
        public override string Description { get; } = "UOP 打包/解包工具\r\n使用 RunUO UOP 打包器";

        /// <summary>
        /// 插件作者
        /// </summary>
        public override string Author { get; } = "Feeh / Epila";

        /// <summary>
        /// 插件版本
        /// </summary>
        public override string Version { get { return _ver.ToString(); } }

        /// <summary>
        /// 插件宿主。
        /// </summary>
        public override IPluginHost Host { get; set; }

        public override void Initialize()
        {
            _ = Files.RootDir;
        }

        public override void Unload()
        {
        }

        public override void ModifyTabPages(TabControl tabControl)
        {
            TabPage page = new TabPage
            {
                Tag = 0, // 用于定义顺序的停靠/取消停靠功能
                Text = "UOP 打包器"
            };
            page.Controls.Add(new UopPackerControl(Version));
            tabControl.TabPages.Add(page);
        }
    }
}