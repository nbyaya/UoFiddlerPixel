/***************************************************************************
 *
 * $Author: MuadDib & Turley
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

namespace UoFiddler.Plugin.MultiEditor
{
    public class MultiEditorPluginBase : PluginBase
    {
        private UserControls.MultiEditorControl _multiEditorControl;

        public MultiEditorPluginBase()
        {
            PluginEvents.ModifyItemsControlContextMenuEvent += EventsModifyItemsControlContextMenuEvent;
        }

        /// <summary>
        /// 插件作者
        /// </summary>
        public override string Author { get; } = "MuadDib & Turley";

        /// <summary>
        /// 插件用途描述
        /// </summary>
        public override string Description { get; } = "用于编辑多重结构的插件\r\n（新增 1 个选项卡）";

        /// <summary>
        /// 插件宿主。
        /// </summary>
        public override IPluginHost Host { get; set; } = null;

        /// <summary>
        /// 插件名称
        /// </summary>
        public override string Name { get; } = "MultiEditorPlugin";

        /// <summary>
        /// 插件版本
        /// </summary>
        public override string Version { get; } = "1.7.0";

        public override void Unload()
        {
            // 在 Fiddler 关闭时触发
        }

        public override void Initialize()
        {
            // 在 Fiddler 启动时触发
            _ = Files.RootDir;
        }

        public override void ModifyPluginToolStrip(ToolStripDropDownButton toolStrip)
        {
            // 需要在插件下拉菜单中添加条目？
        }

        // 在末尾添加一个新的选项卡页
        public override void ModifyTabPages(TabControl tabControl)
        {
            TabPage page = new TabPage
            {
                Tag = tabControl.TabCount + 1, // 用于定义顺序的停靠/取消停靠功能
                Text = "多重编辑器"
            };

            _multiEditorControl = new UserControls.MultiEditorControl
            {
                Dock = DockStyle.Fill
            };
            page.Controls.Add(_multiEditorControl);
            tabControl.TabPages.Add(page);
        }

        private void EventsModifyItemsControlContextMenuEvent(ContextMenuStrip strip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = "多重编辑器：选择物品"
            };
            item.Click += ItemShowContextClicked;
            strip.Items.Add(item);
        }

        private void ItemShowContextClicked(object sender, EventArgs e)
        {
            int currSelected = Host.GetSelectedIdFromItemsControl();
            if (currSelected <= -1)
            {
                return;
            }

            _multiEditorControl?.SelectDrawTile((ushort)currSelected);
        }
    }
}