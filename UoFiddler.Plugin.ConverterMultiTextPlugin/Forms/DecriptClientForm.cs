// /***************************************************************************
//  *
//  * $Author: Turley
//  * 
//  * "啤酒许可证"
//  * 只要你保留此声明，你就可以对这个东西做任何你想做的事情。
//  * 如果我们某天相遇，并且你认为这个东西有价值，
//  * 你可以请我喝杯啤酒作为回报。
//  *
//  ***************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UoFiddler.Plugin.ConverterMultiTextPlugin.Forms
{
    public partial class DecriptClientForm : Form
    {
        private static string m_ClientFileLocation = AppDomain.CurrentDomain.BaseDirectory; // 应用程序的工作目录
        //string CLIENT = "client.exe";  // 未解密的客户端文件名
        private static byte[] bytes;    // 读取客户端的字节数组
        public static long FileSize;    // 读入字节数组的客户端长度

        public DecriptClientForm()
        {
            InitializeComponent();
        }

        // 读取客户端文件
        private void ReadClientFile(string clientFilePath)
        {
            LAB_StatusIS.Text = "正在读取客户端...";

            try
            {
                // 使用 clientFilePath 而不是 "client.exe"
                using (FileStream fStream = File.OpenRead(clientFilePath))
                {
                    FileSize = fStream.Length;
                    bytes = new byte[FileSize];

                    fStream.Read(bytes, 0, bytes.Length);
                    fStream.Close();

                    LAB_StatusIS.Text = "正在移除加密...";

                    RemoveEncryption(bytes, FileSize);

                    LAB_StatusIS.Text = "正在修补多客户端相关功能...";
                    MultiClientPatch();

                    // 使用 Path.GetDirectoryName(clientFilePath)
                    string decryptedClientFilePath = Path.Combine(Path.GetDirectoryName(clientFilePath), "Decrypted_client.exe");

                    LAB_StatusIS.Text = "正在写入新的客户端文件...";
                    using (FileStream foStream = File.OpenWrite(decryptedClientFilePath))
                    {
                        foStream.Write(bytes, 0, bytes.Length);
                    }

                    LAB_StatusIS.Text = "已创建解密后的 Client.exe。";
                }
            }
            catch (IOException)
            {
                LAB_StatusIS.Text = "文件 I/O 错误！！！";
            }
        }

        // 加密处理从这里开始...
        #region 多客户端补丁

        private static bool FindSignatureOffset(byte[] signature, byte[] buffer, out int offset)
        {
            bool found = false;
            offset = 0;
            for (int x = 0; x < buffer.Length - signature.Length; x++)
            {
                for (int y = 0; y < signature.Length; y++)
                {
                    if (buffer[x + y] == signature[y])
                        found = true;
                    else
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    offset = x;
                    break;
                }
            }
            return found;
        }

        private static bool ErrorCheckPatch(byte[] fileBuffer)
        {
            /* 修补以下检查：
             * GetLastError 返回非零值 */

            byte[] oldClientSig = new byte[] { 0x85, 0xC0, 0x75, 0x2F, 0xBF };
            byte[] newClientSig = new byte[] { 0x85, 0xC0, 0x5F, 0x5E, 0x75, 0x2F };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset)) //signature = 目标字节，因此无需额外检查
            {
                //XOR AX, AX
                fileBuffer[offset] = 0x66;
                fileBuffer[offset + 1] = 0x33;
                fileBuffer[offset + 2] = 0xC0;
                fileBuffer[offset + 3] = 0x90;
                return true;
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset)) //signature = 目标字节，因此无需额外检查
            {
                fileBuffer[offset + 4] = 0x90;
                fileBuffer[offset + 5] = 0x90;
                return true;
            }

            return false;
        }

        private static bool SingleCheckPatch(byte[] fileBuffer)
        {
            /* 修补以下检查：
             * "Another copy of UO is already running!" */

            byte[] oldClientSig = new byte[] { 0xC7, 0x44, 0x24, 0x10, 0x11, 0x01, 0x00, 0x00 };
            byte[] newClientSig = new byte[] { 0x83, 0xC4, 0x04, 0x33, 0xDB, 0x53, 0x50 };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                if (fileBuffer[offset + 0x17] == 0x74)
                {
                    fileBuffer[offset + 0x17] = 0xEB;
                    return true;
                }
                else
                {
                    // 找到了单实例检查签名，但实际字节与预期不符。中止。
                    return false;
                }
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                if (fileBuffer[offset + 0x0F] == 0x74)
                {
                    fileBuffer[offset + 0x0F] = 0xEB;
                    return true;
                }
                else
                {
                    // 找到了单实例检查签名，但实际字节与预期不符。中止。
                    return false;
                }
            }

            return false;
        }

        private static bool TripleCheckPatch(byte[] fileBuffer)
        {
            /* 修补以下检查：
             * "Another instance of UO may already be running."
             * "Another instance of UO is already running."
             * "An instance of UO Patch is already running." */

            byte[] oldClientSig = new byte[] { 0xFF, 0xD6, 0x6A, 0x01, 0xFF, 0xD7, 0x68 };
            byte[] newClientSig = new byte[] { 0x3B, 0xC3, 0x89, 0x44, 0x24, 0x08 };
            int offset;

            if (FindSignatureOffset(oldClientSig, fileBuffer, out offset))
            {
                if (fileBuffer[offset - 0x2D] == 0x75 && fileBuffer[offset - 0x0E] == 0x75 && fileBuffer[offset + 0x18] == 0x74)
                {
                    fileBuffer[offset - 0x2D] = 0xEB;
                    fileBuffer[offset - 0x0E] = 0xEB;
                    fileBuffer[offset + 0x18] = 0xEB;
                    return true;
                }
                else
                {
                    // 找到了三重检查签名，但实际字节与预期不符。中止。
                    return false;
                }
            }

            if (FindSignatureOffset(newClientSig, fileBuffer, out offset))
            {
                if (fileBuffer[offset + 0x06] == 0x75 && fileBuffer[offset + 0x2D] == 0x75 && fileBuffer[offset + 0x5F] == 0x74)
                {
                    fileBuffer[offset + 0x06] = 0xEB;
                    fileBuffer[offset + 0x2D] = 0xEB;
                    fileBuffer[offset + 0x5F] = 0xEB;
                    return true;
                }
                else
                {
                    // 找到了三重检查签名，但实际字节与预期不符。中止。
                    return false;
                }
            }

            return false;
        }

        private void MultiClientPatch()
        {
            if (!TripleCheckPatch(bytes))
            {
                // 未找到三重检查签名。中止。
                return;
            }
            if (!SingleCheckPatch(bytes))
            {
                // 未找到单实例检查签名。中止。
                return;
            }
            if (!ErrorCheckPatch(bytes))
            {
                // 未找到错误检查签名。中止。
                return;
            }

            this.LAB_StatusIS.Text = "多客户端修补...完成";
        }

        #endregion

        private void RemoveEncryption(byte[] InClient, long InClientLength)
        {
            // 以下是要搜索的登录加密签名
            byte[] CryptSig = { 0x81, 0xF9, 0x00, 0x00, 0x01, 0x00, 0x0F, 0x8F };
            byte[] CryptSigNew = { 0x00, 0x00, 0x00, 0x00, 0x75, 0x12, 0x8b, 0x54 };
            byte[] JNZSig = { 0x0F, 0x85 };
            byte[] JNESig = { 0x0F, 0x84 };

            int CryptPos = -1, CryptPosNew = -1, JNZPos = -1, JEPos = -1, NewClient = -1;

            /* 用于游戏加密 */
            byte[] BFGamecryptSig = { 0x2C, 0x52, 0x00, 0x00, 0x76 }; /* CMP XXX, 522c - JBE */
            byte[] CmpSig = { 0x3B, 0xC3, 0x0F, 0x84 }; /* CMP EAX,EBX - JE */
            int BFGamecryptPos = -1, CmpPos = -1;

            /* RTD: 确保 JE 0x10 和 JE 0x9X000000 保持不变，否则... 我需要找到新的方法 */
            byte[] TFGamecryptSig = { 0x8B, 0x8B, 0xCC, 0xCC, 0xCC, 0xCC, 0x81, 0xF9, 0x00, 0x01, 0x00, 0x00, 0x74, 0x10 }; /* MOV EBX, XXX - CMP ECX, 0x100 - JE 0x10 */
            byte[] TFGamecryptSigNew = { 0x74, 0x0f, 0x83, 0xb9, 0xb4, 0x00, 0x00, 0x00, 0x00 };
            byte[] JELong = { 0x0F, 0x84, 0xCC, 0x00, 0x00, 0x00, 0x55 }; /* JE XX000000 -  */
            int TFGamecryptPos = -1, TFGamecryptPosNew = -1, GJEPos = -1;

            /* 用于游戏解密 */
            byte[] DecryptSig = { 0x4A, 0x83, 0xCA, 0xF0, 0x42, 0x8A, 0x94, 0x32 };
            byte[] DectestSig = { 0x85, 0xCC, 0x74, 0xCC, 0x33, 0xCC, 0x85, 0xCC, 0x7E, 0xCC };
            byte[] DecryptSigNew = { 0x00, 0x00, 0x74, 0x37, 0x83, 0xbe, 0xb4, 0x00, 0x00, 0x00, 0x00 };
            int DecryptPos = -1, DectestPos = -1, DecryptPosNew = -1;

            byte[] MessageSig = { 0xA9, 0x20, 0x32, 0x30, 0x30, 0x36, 0x20, 0x45, 0x6C, 0x65,
                  0x63, 0x74, 0x72, 0x6F, 0x6E, 0x69, 0x63, 0x20, 0x41, 0x72,
                  0x74, 0x73, 0x20, 0x49, 0x6E, 0x63, 0x2E, 0x20, 0x20, 0x41,
                  0x6C, 0x6C, 0x20, 0x52, 0x69, 0x67, 0x68, 0x74, 0x73, 0x20,
                  0x52, 0x65, 0x73, 0x65, 0x72, 0x76, 0x65, 0x64, 0x2E};

            byte[] MessageSigOld = { 0xA9, 0x20, 0x32, 0x30, 0x30, 0x35, 0x20, 0x45, 0x6C, 0x65,
                     0x63, 0x74, 0x72, 0x6F, 0x6E, 0x69, 0x63, 0x20, 0x41, 0x72,
                     0x74, 0x73, 0x20, 0x49, 0x6E, 0x63, 0x2E, 0x20, 0x20, 0x41,
                     0x6C, 0x6C, 0x20, 0x52, 0x69, 0x67, 0x68, 0x74, 0x73, 0x20,
                     0x52, 0x65, 0x73, 0x65, 0x72, 0x76, 0x65, 0x64, 0x2E};

            byte[] MessageSig09 = { 0xA9, 0x20, 0x32, 0x30, 0x30, 0x39, 0x20, 0x45, 0x6C, 0x65,
                    0x63, 0x74, 0x72, 0x6F, 0x6E, 0x69, 0x63, 0x20, 0x41, 0x72,
                    0x74, 0x73, 0x20, 0x49, 0x6E, 0x63, 0x2E, 0x20, 0x20, 0x41,
                    0x6C, 0x6C, 0x20, 0x52, 0x69, 0x67, 0x68, 0x74, 0x73, 0x20,
                    0x52, 0x65, 0x73, 0x65, 0x72, 0x76, 0x65, 0x64, 0x2E};

            int MessagePos = -1, MessagePosOld = -1, MessagePos09 = -1;

            lbMessagePos.Text = "MessagePos: " + MessagePos;
            lbMessagePosOld.Text = "MessagePosOld: " + MessagePosOld;
            lbMessagePos09.Text = "MessagePos09: " + MessagePos09;

            // ***** 开始搜索登录加密... *****
            // magic x90 加密签名: 81 f9 00 00 01 00 0f 8f
            // 修补方法：找到下面的第一个 0f 84 并将其改为 0f 85
            // 或第一个 0f 85 并将其改为 0f 84
            CryptPos = ScanClient(0x100, CryptSig, InClient, InClientLength, 8, 0);

            // 如果未找到，尝试查找新签名
            if (CryptPos == -1)
            {
                CryptPosNew = ScanClient(0x100, CryptSigNew, InClient, InClientLength, 8, 0);
            }

            if (CryptPos != -1 && CryptPosNew != -1)
            {
                this.LAB_StatusIS.Text = "在此文件中找不到登录签名???";
            }
            else
            {
                if (CryptPos != -1)
                {
                    JNZPos = ScanClient(0x100, JNZSig, InClient, InClientLength, 2, CryptPos);
                    JEPos = ScanClient(0x100, JNESig, InClient, InClientLength, 2, CryptPos);

                    if (JEPos > JNZPos)
                    {
                        bytes[JNZPos + 1] = 0x84;
                        this.LAB_StatusIS.Text = string.Format("使用 JE (0x0F 0x84) - (15 132) 修补 @{0:X} - ({1})", JNZPos, JNZPos.ToString());
                    }
                    else if (JNZPos > JEPos)
                    {
                        bytes[JNZPos + 1] = 0x85;
                        this.LAB_StatusIS.Text = string.Format("使用 JNZ (0x0F 0x85) - (15 133) 修补 @{0:X} - ({1})", JEPos, JEPos.ToString());
                    }
                }
                else if (CryptPosNew != -1)
                {
                    bytes[CryptPosNew + 4] = 0xEB;
                    this.LAB_StatusIS.Text = string.Format("使用 (0xEB) - (235) 修补 @{0:X} - ({1})", (CryptPosNew + 4), (CryptPosNew + 4).ToString());
                    NewClient = 1;
                }
            }

            /*
             * BLOWFISH
             * 这简单的“丢失”在“发送”函数内部，就在它的上方
             * 看起来像：
             * if(GameMode != LOGIN_SOCKET) ;游戏套接字
             * {
             *    BlowfishEncrypt() ;以一个 while (> 0x522c) 开始
             *    ;一堆其他东西
             *    TwofishEncrypt() ;如果存在
             *    send()
             * }
             * else ;登录套接字
             *    send() ;yay, 一个绕过所有加密垃圾的发送
             *
             * 找到循环的开始 while(Obj->stream_pos + len > CRYPT_GAMETABLE_TRIGGER)
             * CRYPT_GAMETABLE_TRIGGER 是 0x522c
             * 找到上面的第一个 CMP EAX,EBX-JE 并将其修补为 CMP EAX,EAX
             */

            BFGamecryptPos = ScanClient(0x100, BFGamecryptSig, InClient, InClientLength, 5, 0);
            // 修复？可能需要删除
            if (BFGamecryptPos != -1)
            {
                CmpPos = ScanClient(0x100, CmpSig, InClient, InClientLength, 4, BFGamecryptPos - 0x20);
            }

            if (BFGamecryptPos == -1 || CmpPos == -1)
            {
                this.LAB_StatusIS.Text = "找不到 blowfish 签名";
            }
            else
            {
                bytes[CmpPos + 1] = 0xC0;
                this.LAB_StatusIS.Text = string.Format("使用 CMP (0xC0) - (192) 修补 @{0:X} - ({1})", CmpPos, CmpPos.ToString());
            }

            /*
             * TWOFISH
             * 修补加密函数以跳过加密
             * 该函数总是在发送之前被调用
             *
             * 找到循环的开始以及它上面的第一个 JE
             * 将 JE (0x84) 修补为 JNE (0x85)
             */

            TFGamecryptPos = ScanClient(0xCC, TFGamecryptSig, InClient, InClientLength, 14, 0);
            if (TFGamecryptPos != -1)
            {
                GJEPos = ScanClient(0xCC, JELong, InClient, InClientLength, 7, TFGamecryptPos - 0x20);
            }

            TFGamecryptPosNew = ScanClient(0xCC, TFGamecryptSigNew, InClient, InClientLength, 9, 0);

            if (TFGamecryptPos == -1 && GJEPos == -1 && TFGamecryptPosNew == -1)
            {
                this.LAB_StatusIS.Text = "找不到旧的或新的 Twofish 签名";
            }
            else
            {
                if (TFGamecryptPos != -1 && GJEPos != -1)
                {
                    bytes[GJEPos + 1] = 0x85;
                    this.LAB_StatusIS.Text = string.Format("使用 JNZ (0x85) - (133) 修补（旧 TF） @{0:X} - ({1})", (GJEPos + 1), (GJEPos + 1).ToString());
                }
                else if (TFGamecryptPosNew != -1)
                {
                    bytes[TFGamecryptPosNew] = 0xEB;
                    this.LAB_StatusIS.Text = string.Format("使用 (0xEB) - (235) 修补（新 TF） @{0:X} - ({1})", TFGamecryptPosNew, TFGamecryptPosNew.ToString());
                }
            }

            /* 游戏加密结束 */

            /* 游戏解密开始（现在是简单的部分） */

            /*
             * 搜索 4A 83 CA F0 42 8A 94 32
             * 并在其上方，85 xx 74 xx 33 xx 85 xx 7E xx
             * 第一个 TEST (85 xx) 必须破解为 CMP EAX, EAX (3B C0)
             * 如果我想像 UORice 中的 LB 那样做，我会破解
             * 第一个 CMP xx JMP xx (85 xx 74 xx) 为 CMP EAX, EAX (3B C0)
             * 这在我破解的那个下面
             */

            /* 找到 4A 83 CA F0 42 8A 94 32 */
            /* 找到它上面的 TEST（不是正上方的那个）*/

            DecryptPos = ScanClient(0x100, DecryptSig, InClient, InClientLength, 8, 0);
            DecryptPosNew = ScanClient(0x100, DecryptSigNew, InClient, InClientLength, 11, 0);
            if (DecryptPos != -1)
            {
                DectestPos = ScanClient(0xCC, DectestSig, InClient, InClientLength, 10, DecryptPos - 0x100);
            }

            if (DecryptPos == -1 && DectestPos == -1 && DecryptPosNew == -1)
            {
                this.LAB_StatusIS.Text = "找不到任何 MD5 解密签名???";
            }
            else
            {
                if (NewClient == -1)
                {
                    bytes[DectestPos] = 0x3B;
                    this.LAB_StatusIS.Text = string.Format("使用 CMP (0x3B) - (59) 修补旧的 MD5 @{0:X} - ({1})", DectestPos, DectestPos.ToString());
                }
                else if (NewClient == 1)
                {
                    bytes[DecryptPosNew + 2] = 0xEB;
                    this.LAB_StatusIS.Text = string.Format("使用 (0xEB) - (235) 修补（新 MD5 (D2+2)） @{0:X} - ({1})", DecryptPosNew, DecryptPosNew.ToString());
                }
            }

            this.LAB_StatusIS.Text = "客户端解密完成...";
        }
        private int ScanClient(int FlexByte, byte[] signature, byte[] client, long client_length, int sig_length, int startat)
        {
            int Count = 0, i = 0, j = 0;

            // 这出于某种原因造成了麻烦
            // 更容易将这2个项目放在各自的部分
            bool UseFlex = GetMyBool(FlexByte, 100);
            byte FByte = GetMyByte(FlexByte);

            if (startat == -1)
            {
                startat = 0;
            }

            for (i = startat; i < client_length - sig_length; i++)
            {
                /* 将 src 中的 src_size 字节与 buf 中的 src_size 字节进行比较 */
                // 从 i 位置开始检查签名字节
                for (j = 0; j < sig_length; j++)
                {
                    /* 如果有差异且这不是 flex_byte，则继续 */
                    if ((UseFlex && signature[j] != FByte) && signature[j] != client[i + j])
                        break;
                    else if (!UseFlex && signature[j] != client[i + j])
                        break;

                    /* 如果这是比较的最后一个字节，则所有内容都匹配 */
                    if (j == (sig_length - 1))
                    {
                        Count++;
                        if (Count >= 1)
                            return i;
                    }
                }
            }

            return -1;
        }

        private bool GetMyBool(int a, int b)
        {
            int TheBool = a & b;
            bool MyBool = Convert.ToBoolean(TheBool);
            return MyBool;
        }

        private byte GetMyByte(int flex)
        {
            int TheByte = flex & 255;
            byte MyByte = Convert.ToByte(TheByte);
            return MyByte;
        }

        private void btDecriptClientForm_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "exe 文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string clientFilePath = openFileDialog1.FileName;
                LAB_StatusIS.Text = "已找到...";
                ReadClientFile(clientFilePath); // 这里调用 ReadClientFile 方法
            }
        }
    }
}