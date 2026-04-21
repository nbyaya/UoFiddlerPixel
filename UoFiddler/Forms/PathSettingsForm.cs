/***************************************************************************
 *
 * $Author: Turley
 * Advanced Nikodemus
 * 
 * "啤酒-葡萄酒许可证"
 * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
 * 如果我们某天相遇，并且你认为这个东西有价值，
 * 你可以请我喝杯啤酒和葡萄酒作为回报。
 *
 ***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ultima;
using Ultima.Helpers;
using UoFiddler.Controls.Classes;

namespace UoFiddler.Forms
{
    public partial class PathSettingsForm : Form
    {
        public PathSettingsForm()
        {
            InitializeComponent();
            Icon = Options.GetFiddlerIcon();

            pgPaths.SelectedObject = new DictionaryPropertyGridAdapter(Files.MulPath);
            tsTbRootPath.Text = Files.RootDir;
        }

        #region [ ReloadPath ]
        private void ReloadPath(object sender, EventArgs e)
        {
            Files.ReLoadDirectory();
            Files.LoadMulPath();
            MapHelper.CheckForNewMapSize();
            pgPaths.SelectedObject = new DictionaryPropertyGridAdapter(Files.MulPath);
            pgPaths.Refresh();
            tsTbRootPath.Text = Files.RootDir;
        }
        #endregion

        #region [ OnClickManual ]
        private void OnClickManual(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择包含客户端文件的目录";
                dialog.ShowNewFolderButton = false;
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                Files.SetMulPath(dialog.SelectedPath);
                pgPaths.SelectedObject = new DictionaryPropertyGridAdapter(Files.MulPath);
                pgPaths.Update();
                tsTbRootPath.Text = Files.RootDir;
                MapHelper.CheckForNewMapSize();
            }
        }
        #endregion

        #region [ OnKeyDownDir ]
        private void OnKeyDownDir(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            Files.SetMulPath(tsTbRootPath.Text);
            pgPaths.SelectedObject = new DictionaryPropertyGridAdapter(Files.MulPath);
            pgPaths.Refresh();
            tsTbRootPath.Text = Files.RootDir;
            MapHelper.CheckForNewMapSize();
        }
        #endregion

        #region [ newDirAndMulToolStripMenuItem ]
        private void newDirAndMulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string directoryPath = dialog.SelectedPath;
                    string[] mulFiles = Directory.GetFiles(directoryPath, "*.mul"); // 筛选 .mul 文件
                    string[] uopFiles = Directory.GetFiles(directoryPath, "*.uop"); // 筛选 .uop 文件
                    string[] files = mulFiles.Concat(uopFiles).ToArray(); // 合并 .mul 和 .uop 文件

                    foreach (string filePath in files)
                    {
                        string key = Path.GetFileNameWithoutExtension(filePath); // 使用不带扩展名的文件名作为键
                        Files.MulPath.Add(key, filePath);
                    }

                    pgPaths.SelectedObject = new DictionaryPropertyGridAdapter(Files.MulPath);
                    pgPaths.Refresh();
                }
            }
        }
        #endregion

        #region [ loadSingleMulFileToolStripMenuItem ]
        private void loadSingleMulFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "mul 文件 (*.mul)|*.mul|uop 文件 (*.uop)|*.uop"; // 筛选 .mul 和 .uop 文件
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = dialog.FileName;
                    string key = Path.GetFileName(filePath); // 使用完整文件名作为键

                    if (Files.MulPath.ContainsKey(key))
                    {
                        Files.MulPath[key] = filePath; // 如果键已存在则更新路径
                    }
                    else
                    {
                        Files.MulPath.Add(key, filePath); // 如果键不存在则添加
                    }

                    pgPaths.SelectedObject = new DictionaryPropertyGridAdapter(Files.MulPath);
                    pgPaths.Refresh();
                }
            }
        }

        #endregion

        #region [ DeleteLineToolStripMenuItem ]
        private void DeleteLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pgPaths.SelectedGridItem != null)
            {
                string key = pgPaths.SelectedGridItem.Label; // 键是所选 GridItem 的标签
                if (Files.MulPath.ContainsKey(key))
                {
                    Files.MulPath.Remove(key);
                    pgPaths.SelectedObject = new DictionaryPropertyGridAdapter(Files.MulPath);
                    pgPaths.Refresh();
                }
            }
        }
        #endregion

        #region [ tsBtnBackup ]
        private void tsBtnBackup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tsTbRootPath.Text) || !Directory.Exists(tsTbRootPath.Text))
            {
                MessageBox.Show("请在 tsTbRootPath 中指定有效的源目录。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择备份的目标目录";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceDir = tsTbRootPath.Text;
                    string destDir = dialog.SelectedPath;

                    DialogResult result = MessageBox.Show("此功能会将源目录中的所有文件和文件夹复制到目标目录。是否继续？", "确认备份", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            CopyDirectory(sourceDir, destDir);
                            MessageBox.Show("备份成功完成。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"备份过程中发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        #endregion

        #region [ CopyDirectory ]
        private void CopyDirectory(string sourceDir, string destDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDir, subdir.Name);
                CopyDirectory(subdir.FullName, tempPath);
            }
        }
        #endregion
    }


    #region DictionaryPropertyGridAdapter
    internal class DictionaryPropertyGridAdapter : ICustomTypeDescriptor
    {
        private readonly IDictionary _dictionary;

        public DictionaryPropertyGridAdapter(IDictionary d)
        {
            _dictionary = d;
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _dictionary;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        PropertyDescriptorCollection
            ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(Array.Empty<Attribute>());
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
            foreach (DictionaryEntry e in _dictionary)
            {
                properties.Add(new DictionaryPropertyDescriptor(_dictionary, e.Key));
            }

            PropertyDescriptor[] props = properties.ToArray();

            return new PropertyDescriptorCollection(props);
        }
    }
    #endregion

    #region DictionaryPropertyDescriptor
    internal class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        private readonly IDictionary _dictionary;
        private readonly object _key;

        internal DictionaryPropertyDescriptor(IDictionary d, object key)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        public override Type PropertyType => typeof(string);

        public override void SetValue(object component, object value)
        {
            _dictionary[_key] = value;
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key];
        }

        public override bool IsReadOnly => false;

        public override Type ComponentType => null;

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
    #endregion
}