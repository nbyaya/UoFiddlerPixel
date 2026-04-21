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

using System.Windows.Forms;
using Ultima;
using UoFiddler.Controls.Plugin;
using UoFiddler.Controls.Plugin.Interfaces;
using UoFiddler.Plugin.Compare.UserControls;

namespace UoFiddler.Plugin.Compare
{
    public class ComparePluginBase : PluginBase
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public override string Name { get; } = "ComparePlugin";

        /// <summary>
        /// 插件用途描述
        /// </summary>
        public override string Description { get; } =
            "比较 2 个物品文件\r\n"
            + "比较 2 个文本\r\n"
            + "比较 2 个色盘文件\r\n"
            + "比较 2 个地图文件\r\n"
            + "比较 2 个界面图片文件\r\n"
            + "比较 2 个纹理文件\r\n"
            + "（新增 7 个选项卡）";

        /// <summary>
        /// 插件作者
        /// </summary>
        public override string Author { get; } = "Turley";

        /// <summary>
        /// 插件版本
        /// </summary>
        public override string Version { get; } = "1.8.0";

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
                Tag = tabControl.TabCount + 1,
                Text = "比较物品"
            };
            CompareItemControl compArt = new CompareItemControl
            {
                Dock = DockStyle.Fill
            };
            page.Controls.Add(compArt);
            tabControl.TabPages.Add(page);

            TabPage page2 = new TabPage
            {
                Tag = tabControl.TabCount + 1,
                Text = "比较地形"
            };
            CompareLandControl compLandControl = new CompareLandControl
            {
                Dock = DockStyle.Fill
            };
            page2.Controls.Add(compLandControl);
            tabControl.TabPages.Add(page2);

            TabPage page3 = new TabPage
            {
                Tag = tabControl.TabCount + 1,
                Text = "比较文本"
            };
            CompareCliLocControl compCli = new CompareCliLocControl
            {
                Dock = DockStyle.Fill
            };
            page3.Controls.Add(compCli);
            tabControl.TabPages.Add(page3);

            TabPage page4 = new TabPage
            {
                Tag = tabControl.TabCount + 1,
                Text = "比较色盘"
            };
            CompareHuesControl compH = new CompareHuesControl
            {
                Dock = DockStyle.Fill
            };
            page4.Controls.Add(compH);
            tabControl.TabPages.Add(page4);

            TabPage page5 = new TabPage
            {
                Tag = tabControl.TabCount + 1,
                Text = "比较界面"
            };
            CompareGumpControl compG = new CompareGumpControl
            {
                Dock = DockStyle.Fill
            };
            page5.Controls.Add(compG);
            tabControl.TabPages.Add(page5);

            TabPage page6 = new TabPage
            {
                Tag = tabControl.TabCount + 1,
                Text = "比较地图"
            };
            CompareMapControl compM = new CompareMapControl
            {
                Dock = DockStyle.Fill
            };
            page6.Controls.Add(compM);
            tabControl.TabPages.Add(page6);
            TabPage page7 = new TabPage
            {
                Tag = tabControl.TabCount + 1,
                Text = "比较纹理"
            };
            CompareTextureControl compTextureControl = new CompareTextureControl
            {
                Dock = DockStyle.Fill
            };
            page7.Controls.Add(compTextureControl);
            tabControl.TabPages.Add(page7);
        }

        public override void ModifyPluginToolStrip(ToolStripDropDownButton toolStrip)
        {
        }
    }
}