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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ultima;
using UoFiddler.Controls.Classes;
using UoFiddler.Controls.Plugin;
using UoFiddler.Controls.Plugin.Interfaces;
using UoFiddler.Plugin.ExamplePlugin.Forms;
using UoFiddler.Plugin.ExamplePlugin.UserControls;

namespace UoFiddler.Plugin.ExamplePlugin
{
    public class ExamplePluginBase : PluginBase
    {
        private const string _itemDescFileName = "itemdesc.cfg";

        public ExamplePluginBase()
        {
            PluginEvents.ModifyItemsControlContextMenuEvent += EventsModifyItemsControlContextMenuEvent;
        }

        /// <summary>
        /// 插件名称
        /// </summary>
        public override string Name { get; } = "PluginTest";

        /// <summary>
        /// 插件用途描述
        /// </summary>
        public override string Description { get; } = "这是一个示例插件。";

        /// <summary>
        /// 插件作者
        /// </summary>
        public override string Author { get; } = "Turley";

        /// <summary>
        /// 插件版本
        /// </summary>
        public override string Version { get; } = "1.0.0";

        /// <summary>
        /// 插件宿主。
        /// </summary>
        public override IPluginHost Host { get; set; }

        public override void Initialize()
        {
            // 做一些有用的事情
            _ = Files.RootDir;
        }

        public override void Unload()
        {
        }

        public override void ModifyTabPages(TabControl tabControl)
        {
            TabPage page = new TabPage
            {
                Tag = tabControl.TabCount + 1, // 用于定义顺序的停靠/取消停靠功能
                Text = "插件测试"
            };
            page.Controls.Add(new ExampleControl());
            tabControl.TabPages.Add(page);
        }

        public override void ModifyPluginToolStrip(ToolStripDropDownButton toolStrip)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = "插件测试"
            };
            item.Click += ItemClick;
            toolStrip.DropDownItems.Add(item);
        }

        private static void ItemClick(object sender, EventArgs e)
        {
            new ExampleForm().Show();
        }

        private void EventsModifyItemsControlContextMenuEvent(ContextMenuStrip strip)
        {
            strip.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem exportItemDescItem = new ToolStripMenuItem
            {
                Text = "将所选内容导出到 itemdesc.cfg"
            };
            exportItemDescItem.Click += ExportToItemDescClicked;
            strip.Items.Add(exportItemDescItem);

            strip.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem exportOffsetItem = new ToolStripMenuItem
            {
                Text = "将所有物品导出到 offset.cfg"
            };
            exportOffsetItem.Click += ExportToOffsetClicked;
            strip.Items.Add(exportOffsetItem);
        }

        private void ExportToOffsetClicked(object sender, EventArgs e)
        {
            List<int> itemIds = new List<int>();
            itemIds.AddRange(Host.GetItemsControl().ItemList);

            string fileName = Path.Combine(Options.OutputPath, "offset.cfg");

            string inputMessage = "是否要将所有物品导出到 offset.cfg？\r\n"
                                  + "可能需要一些时间（大约 10-20 秒）。\r\n\r\n"
                                  + "导出将替换位于以下位置的现有文件："
                                  + fileName
                                  + "\r\n\r\n继续？\r\n";

            if (MessageBox.Show(inputMessage, "将所有物品导出到 offset.cfg？", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (int itemId in itemIds)
            {
                if (itemId <= -1 || !Art.IsValidStatic(itemId))
                {
                    continue;
                }

                Art.Measure(Art.GetStatic(itemId), out int xMin, out int yMin, out int xMax, out int yMax);

                sb.AppendFormat("Item 0x{0:X4}", itemId).AppendLine();
                sb.AppendLine("{");
                sb.AppendFormat("   xMin    {0}", xMin).AppendLine();
                sb.AppendFormat("   yMin    {0}", yMin).AppendLine();
                sb.AppendFormat("   xMax    {0}", xMax).AppendLine();
                sb.AppendFormat("   yMax    {0}", yMax).AppendLine();
                sb.AppendLine("}").AppendLine();
            }

            File.WriteAllText(fileName, sb.ToString());

            MessageBox.Show("完成！");
        }

        private void ExportToItemDescClicked(object sender, EventArgs e)
        {
            var selectedArtIds = new List<int>();
            var itemsControl = Host.GetItemsControl();
            var itemsControlTileView = Host.GetItemsControlTileView();

            foreach (var item in itemsControlTileView.SelectedIndices)
            {
                var graphic = itemsControl.ItemList[item];
                if (Art.IsValidStatic(graphic))
                {
                    selectedArtIds.Add(graphic);
                }
            }

            ExportAllItems(selectedArtIds);
        }

        private static void ExportAllItems(ICollection<int> items)
        {
            if (items.Count == 0)
            {
                return;
            }

            string path = Options.OutputPath;
            string fileName = Path.Combine(path, _itemDescFileName);

            using (StreamWriter streamWriter = new StreamWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write), Encoding.GetEncoding(1252)))
            {
                foreach (var item in items)
                {
                    streamWriter.WriteLine(GetItemDescEntry(item));
                }
            }
        }

        private static string GetItemDescEntry(int itemId)
        {
            ItemData itemData = TileData.ItemTable[itemId];

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Item 0x{0:X4}", itemId).AppendLine();
            sb.AppendLine("{");
            sb.AppendFormat("   Name    {0}", itemData.Name).AppendLine();
            sb.AppendFormat("   Graphic 0x{0:X4}", itemId).AppendLine();
            sb.AppendFormat("   Weight  {0}", itemData.Weight).AppendLine();
            sb.AppendLine("}").AppendLine();

            return sb.ToString();
        }
    }
}